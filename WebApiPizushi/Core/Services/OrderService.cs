using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Delivery;
using Core.Models.General;
using Core.Models.Order;
using Core.SMTP;
using Domain;
using Domain.Entities;
using Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderService(IAuthService authService, ISmtpService smtpService, AppDbPizushiContext context,
    IMapper mapper) : IOrderService
{
    public async Task CreateOrderAsync(DeliveryInfoCreateModel model)
    {
        var userId = (await authService.GetUserId()).ToString();
        var user = await context.Users
            .Where(x => x.Id.ToString() == userId)
            .Include(x => x.Carts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync();

        if (user != null && user.Carts != null)
        {
            var order = new OrderEntity
            {
                UserId = user.Id,
                OrderStatusId = 1
            };
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            List<OrderItemEntity> orderItems = new();
            foreach (var item in user.Carts)
            {
                var orderItem = mapper.Map<OrderItemEntity>(item);
                orderItem.OrderId = order.Id;
                orderItems.Add(orderItem);
            }

            await context.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();

            var delInfo = mapper.Map<DeliveryInfoEntity>(model);
            delInfo.OrderId = order.Id;

            await context.DeliveryInfos.AddAsync(delInfo);
            user.Carts.Clear();
            await context.SaveChangesAsync();

            var price = orderItems.Sum(i => i.Count * i.PriceBuy);

            var productRows = string.Join("", orderItems.Select(item => $@"
    <tr style='border-bottom: 1px solid #f2f2f2;'>
        
        <td style='padding: 10px; font-weight: bold; color: #333;'>{item.Product.Name}</td>
        <td style='padding: 10px; color: #333;'>{item.PriceBuy:N2} грн</td>
        <td style='padding: 10px; color: #333;'>{item.Count}</td>
    </tr>"));

            var emailModel = new EmailMessage
            {
                To = user.Email!,
                Subject = "Успішне оформлення замовлення (PIZUSHI)",
                Body = $@"
        <div style='font-family: Arial, sans-serif; color: #333;'>
            <h2 style='color: #FF6B00;'>Дякуємо за ваше замовлення, {model.RecipientName}!</h2>
            <p>Ваше замовлення на суму <strong>{price:N2} грн</strong> було успішно оформлено.</p>
            <p>Незабаром його буде передано на доставку.</p>

            <h3 style='margin-top: 30px; color: #FF6B00;'>Деталі замовлення:</h3>
            <table style='width: 100%; border-collapse: collapse; margin-top: 10px;'>
                <thead>
                    <tr style='background-color: #FF6B00; color: white;'>
                        <th style='padding: 10px; text-align: left;'>Назва</th>
                        <th style='padding: 10px; text-align: left;'>Ціна</th>
                        <th style='padding: 10px; text-align: left;'>Кількість</th>
                    </tr>
                </thead>
                <tbody>
                    {productRows}
                </tbody>
            </table>

        </div>"
            };

            var result = await smtpService.SendEmailAsync(emailModel);

        }
    }

    public async Task<List<SimpleModel>> GetCitiesAsync(string city)
    {
        var query = context.Cities.AsQueryable();
        query = query.Where(x => x.Name.ToLower().Contains(city.ToLower()) == true || x.Name.ToLower() == city.ToLower());
        query = query.OrderBy(c =>
                c.Name.ToLower() == city.ToLower() ? 0 : 1
            );
        var cities = await query.ProjectTo<SimpleModel> (mapper.ConfigurationProvider).Take(15).ToListAsync();
        return cities;
    }

    public async Task<List<OrderModel>> GetOrdersAsync()
    {
        var userId = await authService.GetUserId();
        var orderModelList = await context.Orders
            .Where(x => x.UserId == userId)
            .ProjectTo<OrderModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        orderModelList = orderModelList
        .Select(item =>
        {
            item.TotalPrice = item.OrderItems.Sum(oi => oi.PriceBuy * oi.Count);
            return item;
        })
        .ToList();

        return orderModelList;
    }

    public async Task<List<SimpleModel>> GetPostDepartmentsAsync(PostDepartmentSearchModel model)
    {
        var query = context.PostDepartments.AsQueryable();
        query = query.Where(x => x.CityId == model.CityId);
        if (!string.IsNullOrEmpty(model.Name))
            query = query.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()) == true);
        var departments = await query.ProjectTo<SimpleModel>(mapper.ConfigurationProvider).Take(15).ToListAsync();
        return departments;
    }

    public async Task<List<SimpleModel>> GetPymentTypesAsync()
    {
        var types = await context.PaymentTypes.ProjectTo<SimpleModel>(mapper.ConfigurationProvider).ToListAsync();
        return types;
    }
}
