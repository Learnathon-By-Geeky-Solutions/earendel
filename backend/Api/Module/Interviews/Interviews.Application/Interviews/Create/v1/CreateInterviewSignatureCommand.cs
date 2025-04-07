using System;
using System.ComponentModel;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

[ExcludeFromCodeCoverage]
public sealed record CreateInterviewSignatureCommand(
    string MeetingNumber,
    int Role
) : IRequest<CreateInterviewSignatureResponse>;
