using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.GithubLogin
{
    [ExcludeFromCodeCoverage]
    public class GithubRequestCommand
    {
        public string Code { get; set; } = string.Empty;
    }
}