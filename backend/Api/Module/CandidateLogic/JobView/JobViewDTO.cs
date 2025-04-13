using System;
using System.Collections.Generic;
using TalentMesh.Module.Job.Domain; // Assuming Jobs properties are needed

namespace TalentMesh.Module.CandidateLogic.JobView // Or your preferred namespace
{
    public class JobViewDto
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Requirments { get; set; } = default!; 
        public string Location { get; set; } = default!;
        public string JobType { get; set; } = default!;
        public string ExperienceLevel { get; set; } = default!;
        public DateTimeOffset CreatedOn { get; set; } 

        public List<Guid> RequiredSkillIds { get; set; } = new List<Guid>();
        public List<Guid> RequiredSubskillIds { get; set; } = new List<Guid>();
    }
}