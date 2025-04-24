using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;


namespace TalentMesh.Module.Job.Domain;

public class JobInfo
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Requirments { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string JobType { get; set; } = default!;
    public string ExperienceLevel { get; set; } = default!;
    public string? Salary { get; set; }
    public Guid PostedById { get; set; }
}
public class Jobs : AuditableEntity, IAggregateRoot
{
    // Add properties for the job entity
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Requirments { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string JobType { get; set; } = default!;
    public string ExperienceLevel { get; private set; } = default!;
    public string? Salary { get; private set; } = default!;
    public Guid PostedById { get; private set; }
    public string PaymentStatus { get; set; } = default!;
    public static Jobs Create(JobInfo info)
    {

        var job = new Jobs
        {
            Name = info.Name,
            Description = info.Description,
            Requirments = info.Requirments,
            Location = info.Location,
            JobType = info.JobType,
            ExperienceLevel = info.ExperienceLevel,
            Salary = info.Salary ?? string.Empty,
            PostedById = info.PostedById,
            PaymentStatus = "Pending"
        };

        job.QueueDomainEvent(new JobCreated() { User = job });

        return job;
    }

    public Jobs Update(
        string? name, string? description, string? requirments,
        string? location, string? jobType, string? experienceLevel, string? salary, string? paymentStatus = default!
        )
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;
        if (requirments is not null && Requirments?.Equals(requirments, StringComparison.OrdinalIgnoreCase) is not true) Requirments = requirments;
        if (location is not null && Location?.Equals(location, StringComparison.OrdinalIgnoreCase) is not true) Location = location;
        if (jobType is not null && JobType?.Equals(jobType, StringComparison.OrdinalIgnoreCase) is not true) JobType = jobType;
        if (experienceLevel is not null && ExperienceLevel?.Equals(experienceLevel, StringComparison.OrdinalIgnoreCase) is not true) ExperienceLevel = experienceLevel;
        if (salary is not null && Salary?.Equals(salary, StringComparison.OrdinalIgnoreCase) is not true) Salary = salary;
        if (paymentStatus is not null && PaymentStatus?.Equals(paymentStatus, StringComparison.OrdinalIgnoreCase) is not true) PaymentStatus = paymentStatus;

        this.QueueDomainEvent(new JobUpdated() { User = this });

        return this;
    }

}

