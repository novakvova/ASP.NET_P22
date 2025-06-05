﻿using Core.Interfaces;
using Core.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPizushi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var model = await productService.List();

        return Ok(model);
    }
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var model = await productService.GetById(id);

        return Ok(model);
    }
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var model = await productService.GetBySlug(slug);

        return Ok(model);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] ProductCreateModel model)
    {
        if (model.ImageFiles == null)
            return BadRequest("Image files are empty!");
        if (model.ProductIngredientsId == null)
            return BadRequest("Product ingredients are empty!");
        var entity = await productService.Create(model);
        if (entity != null)
            return Ok(model);
        else return BadRequest("Error create product!");
    }
}
