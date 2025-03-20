using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser
{
    [ExcludeFromCodeCoverage]
    public class TokenRequestCommand
    {
        public string Token { get; set; } = string.Empty;
    }
}