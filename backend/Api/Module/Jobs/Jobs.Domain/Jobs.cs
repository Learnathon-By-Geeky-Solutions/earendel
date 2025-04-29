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

    public Jobs Update(JobUpdateDetails details)
    {
        UpdateProperty(details.Name, value => Name = value, Name);
        UpdateProperty(details.Description, value => Description = value, Description);
        UpdateProperty(details.Requirments, value => Requirments = value, Requirments);
        UpdateProperty(details.Location, value => Location = value, Location);
        UpdateProperty(details.JobType, value => JobType = value, JobType);
        UpdateProperty(details.ExperienceLevel, value => ExperienceLevel = value, ExperienceLevel);
        UpdateProperty(details.Salary, value => Salary = value, Salary);
        UpdateProperty(details.PaymentStatus, value => PaymentStatus = value, PaymentStatus);

        this.QueueDomainEvent(new JobUpdated { User = this });
        return this;
    }

    private void UpdateProperty(string? newValue, Action<string> setter, string? currentValue)
    {
        if (newValue != null && (currentValue == null || !currentValue.Equals(newValue, StringComparison.OrdinalIgnoreCase)))
        {
            setter(newValue);
        }
    }

}

