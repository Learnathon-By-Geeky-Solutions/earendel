using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

public sealed record CreateInterviewSignatureCommand(
    string MeetingNumber,
    int Role
) : IRequest<CreateInterviewSignatureResponse>;
