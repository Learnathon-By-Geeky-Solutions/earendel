using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Create.v1;

public sealed record CreateRubricCommand(
    Guid SubSkillId,
    Guid SeniorityId,
    [property: DefaultValue("Sample Rubric")] string Title = null!,
    [property: DefaultValue("Descriptive Description")] string RubricDescription = null!,
    [property: DefaultValue(1.0)] decimal Weight = 1.0m

) : IRequest<CreateRubricResponse>;
