﻿using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Areas.Admin.Models.Product;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Обов'язкове поле")]
    [DataType(DataType.Text)]
    [Display(Name = "Назва продукту")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Категорія")]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Зображення")]
    public List<CreateProductImageViewModel> Images { get; set; } = new();
}
