﻿namespace Core.Models.AdminUser;

public class AdminUserItemModel
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsLoginGoogle { get; set; } = false;
    public bool IsLoginPassword { get; set; } = false;
    public List<string> Roles { get; set; } = new();
    public List<string> LoginTypes { get; set; } = new();
}
