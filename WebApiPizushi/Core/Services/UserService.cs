﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus;
using Core.Constants;
using Core.Interfaces;
using Core.Models.AdminUser;
using Core.Models.Search;
using Core.Models.Search.Params;
using Core.Models.Seeder;
using Domain;
using Domain.Entities.Identity;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using static Bogus.DataSets.Name;

namespace Core.Services;

public class UserService(UserManager<UserEntity> userManager,
    IMapper mapper,
    IImageService imageService,
    RoleManager<RoleEntity> roleManager,
    AppDbPizushiContext context) : IUserService
{
    public async Task<List<AdminUserItemModel>> GetAllUsersAsync()
    {
        var users = await userManager.Users
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await context.UserLogins.ForEachAsync(login =>
        {
            var user = users.FirstOrDefault(u => u.Id == login.UserId);
            if (user != null)
            {
                user.LoginTypes.Add(login.LoginProvider);
            }
        });

        await context.Users
       .ForEachAsync(user =>
       {
           var adminUser = users.FirstOrDefault(u => u.Id == user.Id);
           if (adminUser != null)
           {
               if (!string.IsNullOrEmpty(user.PasswordHash))
               {
                   adminUser.LoginTypes.Add("Password");
               }
           }
       });

        return users;
    }

    public async Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            string nameFilter = model.Name.Trim().ToLower().Normalize();

            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).ToLower().Contains(nameFilter) ||
                u.FirstName.ToLower().Contains(nameFilter) ||
                u.LastName.ToLower().Contains(nameFilter));
        }

        if (model?.StartDate != null)
        {
            query = query.Where(u => u.DateCreated >= model.StartDate);
        }

        if (model?.EndDate != null)
        {
            query = query.Where(u => u.DateCreated <= model.EndDate);
        }

        if (model.Roles != null && model.Roles.Any())
        {
            var roles = model.Roles.Where(x=>!string.IsNullOrEmpty(x));
            if(roles.Count() > 0)
                query = query.Where(user => roles.Any(role => user.UserRoles.Select(x=>x.Role.Name).Contains(role)));
        }

        var totalCount = await query.CountAsync();

        var safeItemsPerPage = model.ItemPerPAge < 1 ? 10 : model.ItemPerPAge;
        var totalPages = (int)Math.Ceiling(totalCount / (double)safeItemsPerPage);
        var safePage = Math.Min(Math.Max(1, model.Page), Math.Max(1, totalPages));

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((safePage - 1) * safeItemsPerPage)
            .Take(safeItemsPerPage)
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

       //await LoadLoginsAndRolesAsync(users);

        return new SearchResult<AdminUserItemModel>
        {
            Items = users,
            Pagination = new PaginationModel
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                ItemsPerPage = safeItemsPerPage,
                CurrentPage = safePage
            }
        };
    }

    public async Task<string> SeedAsync(SeedItemsModel model)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var fakeUsers = new Faker<SeederUserModel>("uk")
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
           //Pick some fruit from a basket
           .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
           .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
           .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
           .RuleFor(u => u.Password, (f, u) => f.Internet.Password(8))
           .RuleFor(u => u.Roles, f => new List<string>() { f.PickRandom(Constants.Roles.AllRoles) })
           .RuleFor(u => u.Image, f => "https://thispersondoesnotexist.com");
            
        var genUsers = fakeUsers.Generate(model.Count);

        try
        {
            foreach (var user in genUsers)
            {
                var entity = mapper.Map<UserEntity>(user);
                entity.UserName = user.Email;
                entity.Image = await imageService.SaveImageFromUrlAsync(user.Image);
                var result = await userManager.CreateAsync(entity, user.Password);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Create User {0}", user.Email);
                    continue;
                }
                foreach (var role in user.Roles)
                {
                    if (await roleManager.RoleExistsAsync(role))
                    {
                        await userManager.AddToRoleAsync(entity, role);
                    }
                    else
                    {
                        Console.WriteLine("Not Found Role {0}", role);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Json Parse Data {0}", ex.Message);
        }

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        return elapsedTime;
    }
}
