using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;


namespace TalentMesh.Module.Job.Domain;
public class Jobs : AuditableEntity, IAggregateRoot
{
    public string Name { get;  set; } = default!;
    public string? Description { get;  set; }
    public string Requirments { get;  set; } = default!;
    public string Location { get;  set; } = default!;
    public string JobType { get;  set; } = default!;
    public string ExperienceLevel { get; private set; } = default!;

    public static Jobs Create(
        string name, string? description, string requirments, 
        string location, string jobType, string experienceLevel
        )
    {
        var user = new Jobs
        {
            Name = name,
            Description = description,
            Requirments = requirments,
            Location = location,
            JobType = jobType,
            ExperienceLevel = experienceLevel

        };

        user.QueueDomainEvent(new JobCreated() { User = user });

        return user;
    }

    public Jobs Update(
        string? name, string? description, string? requirments,
        string? location, string? jobType, string? experienceLevel
        )
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;
        if (requirments is not null && Requirments?.Equals(requirments, StringComparison.OrdinalIgnoreCase) is not true) Requirments = requirments;
        if (location is not null && Location?.Equals(location, StringComparison.OrdinalIgnoreCase) is not true) Location = location;
        if (jobType is not null && JobType?.Equals(jobType, StringComparison.OrdinalIgnoreCase) is not true) JobType = jobType;
        if (experienceLevel is not null && ExperienceLevel?.Equals(experienceLevel, StringComparison.OrdinalIgnoreCase) is not true) ExperienceLevel = experienceLevel;

        this.QueueDomainEvent(new JobUpdated() { User = this });

        return this;
    }


}

