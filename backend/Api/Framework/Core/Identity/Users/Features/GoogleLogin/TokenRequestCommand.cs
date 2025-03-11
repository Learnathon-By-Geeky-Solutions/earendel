using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser
{
    public class TokenRequestCommand
    {
        public string Token { get; set; } = string.Empty;
    }
}