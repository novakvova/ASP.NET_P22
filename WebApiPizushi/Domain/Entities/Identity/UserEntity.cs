﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserEntity : IdentityUser<long>
{
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Image { get; set; } = null;

    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    public ICollection<CartEntity>? Carts { get; set; }

    public ICollection<OrderEntity>? Orders { get; set; }
}
