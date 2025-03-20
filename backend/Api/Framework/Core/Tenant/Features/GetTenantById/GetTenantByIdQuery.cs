using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenantById;
[ExcludeFromCodeCoverage]
public record GetTenantByIdQuery(string TenantId) : IRequest<TenantDetail>;
