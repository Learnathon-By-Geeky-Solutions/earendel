﻿using Microsoft.AspNetCore.Identity;

namespace TalentMesh.Framework.Infrastructure.Identity.Users;
public class TMUser : IdentityUser
{
    public Uri? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }
    
}
