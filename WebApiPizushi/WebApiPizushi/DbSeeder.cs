﻿using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiPizushi.Constants;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Seeder;

namespace WebApiPizushi;

public static class DbSeeder
{
    public static async Task SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
        var context = scope.ServiceProvider.GetRequiredService<AppDbPizushiContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        context.Database.Migrate();

        if (!context.Categories.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                    var entityItems = mapper.Map<List<CategoryEntity>>(categories);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Categories.json");
            }
        }

        if (!context.Ingredients.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Ingredients.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var items = JsonSerializer.Deserialize<List<SeederIngredientModel>>(jsonData);
                    var entityItems = mapper.Map<List<IngredientEntity>>(items);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.Ingredients.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Ingredients.json");
            }
        }

        if (!context.ProductSizes.Any())
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "ProductSizes.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var items = JsonSerializer.Deserialize<List<SeederProductSizeModel>>(jsonData);
                    var entityItems = mapper.Map<List<ProductSizeEntity>>(items);
                    await context.ProductSizes.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File ProductSizes.json");
            }
        }

        if (context.Products.Count()==0)
        {
            var сaesar = new ProductEntity
            {
                Name = "Цезаре",
                Slug = "caesar",
                Price = 195,
                Weight = 540,
                CategoryId = 1, // Assuming the first category is for Caesar
                ProductSizeId = 1 // Assuming the first size is for Caesar
            };

            context.Products.Add(сaesar);
            await context.SaveChangesAsync();

            var ingredients = context.Ingredients.ToList();

            foreach(var ingredient in ingredients)
            {
                var productIngredient = new ProductIngredientEntity
                {
                    ProductId = сaesar.Id,
                    IngredientId = ingredient.Id
                };
                context.ProductIngredients.Add(productIngredient);
            }
            await context.SaveChangesAsync();

            string[] images = {
                "https://lovepizza.com.ua/images/virtuemart/product/cezar.500x500.png",
                "https://cipollino.ua/content/uploads/images/pytstsa-tsezar-svezhest-khrustiashchest-y-sytost-cipollino.jpg",
                "https://cipollino.ua/content/uploads/images/pytstsa-tsezar-svezhest-khrustiashchest-y-sytost-2-cipollino.jpg"
            };

            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            foreach (var imageUrl in images)
            {
                try
                {
                    var productImage = new ProductImageEntity
                    {
                        ProductId = сaesar.Id,
                        Name = await imageService.SaveImageFromUrlAsync(imageUrl)
                    };
                    context.ProductImages.Add(productImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Save Image {0} - {1}", imageUrl, ex.Message);
                }
            }
            await context.SaveChangesAsync();

        }

        if (context.Products.Count() == 1)
        {
            var сaesar = new ProductEntity
            {
                Name = "Цезаре",
                Slug = "caesar",
                Price = 355,
                Weight = 1080,
                CategoryId = 1, // Assuming the first category is for Caesar
                ProductSizeId = 2 // Assuming the first size is for Caesar
            };

            context.Products.Add(сaesar);
            await context.SaveChangesAsync();

            var ingredients = context.Ingredients.ToList();

            foreach (var ingredient in ingredients)
            {
                var productIngredient = new ProductIngredientEntity
                {
                    ProductId = сaesar.Id,
                    IngredientId = ingredient.Id
                };
                context.ProductIngredients.Add(productIngredient);
            }
            await context.SaveChangesAsync();

            string[] images = {
                "https://kvadratsushi.com/wp-content/uploads/2020/06/chezar_1200x800.jpg",
                "https://kingpizza.kh.ua/resources/products/20210810115749_ca6b6cbe9b.jpg"
            };

            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            foreach (var imageUrl in images)
            {
                try
                {
                    var productImage = new ProductImageEntity
                    {
                        ProductId = сaesar.Id,
                        Name = await imageService.SaveImageFromUrlAsync(imageUrl)
                    };
                    context.ProductImages.Add(productImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Save Image {0} - {1}", imageUrl, ex.Message);
                }
            }
            await context.SaveChangesAsync();

        }


        if (!context.Roles.Any())
        {
            foreach (var roleName in Roles.AllRoles)
            {
                var result = await roleManager.CreateAsync(new(roleName));
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Create Role {0}", roleName);
                }
            }
        }

        if (!context.Users.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var user in users)
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
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }


    }
}
