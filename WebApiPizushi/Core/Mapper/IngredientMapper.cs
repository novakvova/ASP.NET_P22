﻿using AutoMapper;
using Domain.Entities;
using Core.Models.Seeder;
using Core.Models.Product;
using Core.Models.Product.Ingredient;

namespace Core.Mapper;

public class IngredientMapper : Profile
{
    public IngredientMapper()
    {
        CreateMap<SeederIngredientModel, IngredientEntity>();
        CreateMap<IngredientEntity, ProductIngredientModel>();
        CreateMap<CreateIngredientModel, IngredientEntity>()
            .ForMember(x => x.Image, opt => opt.Ignore());
    }
}
