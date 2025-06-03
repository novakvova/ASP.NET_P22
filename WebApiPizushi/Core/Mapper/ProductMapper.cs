﻿using AutoMapper;
using Core.Models.Product;
using Domain.Entities;

namespace Core.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductImageEntity, ProductImageModel>();
        CreateMap<ProductEntity, ProductItemModel>()
            .ForMember(src => src.ProductImages, opt => opt
                .MapFrom(x => x.ProductImages.OrderBy(p => p.Priority)))
            .ForMember(src => src.Ingredients, opt => opt
                .MapFrom(x => x.ProductIngredients.Select(x => x.Ingredient)));

                
            //.ForMember(x => x.ProductIngredients, opt => opt.MapFrom(
            //    src => src.ProductIngredients != null ?
            //    src.ProductIngredients.Where(p => p.ProductId == src.Id)
            //    .Select(p => p.Ingredient)
            //    : new List<IngredientEntity>()));
    }
}
