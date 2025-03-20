using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.DisableTenant;
[ExcludeFromCodeCoverage]
public record DisableTenantCommand(string TenantId) : IRequest<DisableTenantResponse>;
