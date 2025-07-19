using Core.Interfaces;
using Core.Models.Delivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPizushi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetUserOrders()
    {
        var model = await orderService.GetOrdersAsync();

        return Ok(model);
    }

    [HttpGet("search-city")]
    public async Task<IActionResult> GetCities([FromQuery] string? city)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city))
            return BadRequest(city);
        var model = await orderService.GetCitiesAsync(city);
        return Ok(model);
    }

    [HttpGet("post-departments")]
    public async Task<IActionResult> GetPostDepartments([FromQuery] PostDepartmentSearchModel model)
    {
        if (model == null)
            return BadRequest(model);
        var response = await orderService.GetPostDepartmentsAsync(model);
        return Ok(response);
    }

    [HttpGet("payment-types")]
    public async Task<IActionResult> GetPaymentTypes()
    {
        var model = await orderService.GetPymentTypesAsync();
        return Ok(model);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Order([FromForm] DeliveryInfoCreateModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(model);
        await orderService.CreateOrderAsync(model);
        return Ok();
    }
}
