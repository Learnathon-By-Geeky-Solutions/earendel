using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenantById;
public record GetTenantByIdQuery(string TenantId) : IRequest<TenantDetail>;
