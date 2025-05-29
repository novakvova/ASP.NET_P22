﻿using Domain.Entities.Identity;

namespace Core.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(UserEntity user);
}
