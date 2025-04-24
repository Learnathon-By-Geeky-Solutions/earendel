using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Interviews.Application.Services;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

[ExcludeFromCodeCoverage]
public sealed class CreateInterviewSignatureHandler(
    IZoomService zoomService)
    : IRequestHandler<CreateInterviewSignatureCommand, CreateInterviewSignatureResponse>
{
    public async Task<CreateInterviewSignatureResponse> Handle(CreateInterviewSignatureCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var signature = await zoomService.GenerateSignatureAsync(request.MeetingNumber, request.Role);

        return new CreateInterviewSignatureResponse(signature);
    }
}
