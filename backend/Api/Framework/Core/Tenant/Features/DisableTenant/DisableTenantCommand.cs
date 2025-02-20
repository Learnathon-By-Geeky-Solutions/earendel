using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.DisableTenant;
public record DisableTenantCommand(string TenantId) : IRequest<DisableTenantResponse>;
