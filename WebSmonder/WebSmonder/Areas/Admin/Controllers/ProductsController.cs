﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSmonder.Areas.Admin.Models.Product;
using WebSmonder.Constants;
using WebSmonder.Data;
using WebSmonder.Data.Entities;
using WebSmonder.Interfaces;

namespace WebSmonder.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class ProductsController(AppSmonderDbContext context,
        IMapper mapper, IImageService imageService) : Controller
{
    public IActionResult Index()
    {
        ViewBag.Title = "Продукти";
        var model = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Title = "Створити продукт";
        ViewBag.Categories = context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        var existingProduct = await context.Products.SingleOrDefaultAsync(x => x.Name == model.Name);

        if (existingProduct != null)
        {
            ModelState.AddModelError("Name", "Такий продукт вже є!!!");
            return View(model);
        }

        var productEntity = mapper.Map<ProductEntity>(model);

        productEntity.Category = await context.Categories
            .SingleOrDefaultAsync(x => x.Name == model.CategoryName);

        var savedImages = await Task.WhenAll(
            model.Images.Select(async image => new ProductImageEntity
            {
                Name = await imageService.SaveImageFromBase64Async(image.Name),
                Priotity = image.Priority
            })
        );

        productEntity.ProductImages = savedImages.ToList();

        context.Products.Add(productEntity);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
