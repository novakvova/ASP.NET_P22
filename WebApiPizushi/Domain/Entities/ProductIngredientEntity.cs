﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity;

[Table("tblProductIngredients")]
public class ProductIngredientEntity
{
    [ForeignKey("Product")]
    public long ProductId { get; set; }
    [ForeignKey("Ingredient")]
    public long IngredientId { get; set; }

    public virtual ProductEntity? Product { get; set; }
    public virtual IngredientEntity? Ingredient { get; set; }
}
