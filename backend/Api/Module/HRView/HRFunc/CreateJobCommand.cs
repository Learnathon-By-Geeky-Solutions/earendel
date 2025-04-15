using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;
using System.Collections.Generic;

namespace TalentMesh.Module.HRView.HRFunc// Or your preferred namespace
{
    public record CreateJobCommand(
        // Job Properties
        string Name,
        string? Description,
        string Requirments, 
        string Location,
        string JobType,
        string ExperienceLevel,
        string? Salary, // Use string? if Salary is optional
        Guid PostedBy,
        // Associated Skill and Subskill IDs
        List<Guid> RequiredSkillIds,
        List<Guid> RequiredSubskillIds

    ) : IRequest<IResult>; // Returns the ID of the created job
}