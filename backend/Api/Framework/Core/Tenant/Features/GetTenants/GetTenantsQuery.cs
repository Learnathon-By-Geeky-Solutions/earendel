using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenants;
[ExcludeFromCodeCoverage]
public sealed class GetTenantsQuery : IRequest<List<TenantDetail>>;
