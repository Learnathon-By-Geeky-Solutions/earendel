using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenants;
public sealed class GetTenantsQuery : IRequest<List<TenantDetail>>;
