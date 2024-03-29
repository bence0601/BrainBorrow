﻿using Microsoft.AspNetCore.Identity;

namespace BrainBorrowAPI.Services.Authentication
{
    public interface ITokenService
    {
        public string CreateToken(IdentityUser user);
    }
}
