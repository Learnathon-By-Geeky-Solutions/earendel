using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.ActivateTenant;
[ExcludeFromCodeCoverage]
public record ActivateTenantCommand(string TenantId) : IRequest<ActivateTenantResponse>;
