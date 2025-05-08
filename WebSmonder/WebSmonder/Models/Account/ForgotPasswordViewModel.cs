﻿using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account;

public class ForgotPasswordViewModel
{
    [Display(Name = "Електронна пошта")]
    [Required(ErrorMessage = "Вкажіть електронну пошту")]
    [EmailAddress(ErrorMessage = "Пошту вказано неправильно")]
    public string Email { get; set; } = string.Empty;
}
