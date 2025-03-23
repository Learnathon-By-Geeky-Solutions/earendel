using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Infrastructure.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure
{
    [ExcludeFromCodeCoverage]

    public static class JobModule
    {
        public class Endpoints : CarterModule
        {
            public Endpoints() : base("job") { }

            public override void AddRoutes(IEndpointRouteBuilder app)
            {
                // Endpoints for Jobs
                var jobGroup = app.MapGroup("jobs").WithTags("jobs");
                jobGroup.MapJobCreationEndpoint();
                jobGroup.MapGetJobEndpoint();
                jobGroup.MapGetJobListEndpoint();
                jobGroup.MapJobUpdateEndpoint();
                jobGroup.MapJobDeleteEndpoint();

                // Endpoints for Job Applications
                var jobApplicationGroup = app.MapGroup("jobapplications").WithTags("jobapplications");
                jobApplicationGroup.MapJobApplicationCreationEndpoint();
                jobApplicationGroup.MapGetJobApplicationEndpoint();
                jobApplicationGroup.MapGetJobApplicationListEndpoint();
                jobApplicationGroup.MapJobApplicationUpdateEndpoint();
                jobApplicationGroup.MapJobApplicationDeleteEndpoint();

                // Endpoints for Job Required Skills
                var jobRequiredSkillGroup = app.MapGroup("jobrequiredskills").WithTags("jobrequiredskills");
                jobRequiredSkillGroup.MapJobRequiredSkillCreationEndpoint();
                jobRequiredSkillGroup.MapGetJobRequiredSkillEndpoint();
                jobRequiredSkillGroup.MapGetJobRequiredSkillListEndpoint();
                jobRequiredSkillGroup.MapJobRequiredSkillUpdateEndpoint();
                jobRequiredSkillGroup.MapJobRequiredSkillDeleteEndpoint();

                // Endpoints for Job Required Subskills
                var jobRequiredSubskillGroup = app.MapGroup("jobrequiredsubskills").WithTags("jobrequiredsubskills");
                jobRequiredSubskillGroup.MapJobRequiredSubskillCreationEndpoint();
                jobRequiredSubskillGroup.MapGetJobRequiredSubskillEndpoint();
                jobRequiredSubskillGroup.MapGetJobRequiredSubskillListEndpoint();
                jobRequiredSubskillGroup.MapJobRequiredSubskillUpdateEndpoint();
                jobRequiredSubskillGroup.MapJobRequiredSubskillDeleteEndpoint();
            }
        }

        public static WebApplicationBuilder RegisterJobServices(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.Services.BindDbContext<JobDbContext>();
            builder.Services.AddScoped<IDbInitializer, JobDbInitializer>();

            builder.Services.AddHostedService<InterviewApplicationConsumer>();

            // Register repositories for Jobs
            builder.Services.AddKeyedScoped<IRepository<Jobs>, JobRepository<Jobs>>("jobs:job");
            builder.Services.AddKeyedScoped<IReadRepository<Jobs>, JobRepository<Jobs>>("jobs:jobReadOnly");

            // Register repositories for JobApplications
            builder.Services.AddKeyedScoped<IRepository<JobApplication>, JobRepository<JobApplication>>("jobs:jobapplication");
            builder.Services.AddKeyedScoped<IReadRepository<JobApplication>, JobRepository<JobApplication>>("jobs:jobApplicationReadOnly");

            // Register repositories for JobRequiredSkill
            builder.Services.AddKeyedScoped<IRepository<JobRequiredSkill>, JobRepository<JobRequiredSkill>>("jobs:jobrequiredskill");
            builder.Services.AddKeyedScoped<IReadRepository<JobRequiredSkill>, JobRepository<JobRequiredSkill>>("jobs:jobrequiredskillReadOnly");

            // Register repositories for JobRequiredSubskill
            builder.Services.AddKeyedScoped<IRepository<JobRequiredSubskill>, JobRepository<JobRequiredSubskill>>("jobs:jobrequiredsubskill");
            builder.Services.AddKeyedScoped<IReadRepository<JobRequiredSubskill>, JobRepository<JobRequiredSubskill>>("jobs:jobrequiredsubskillReadOnly");

            return builder;
        }

        public static WebApplication UseJobModule(this WebApplication app)
        {
            return app;
        }
    }
}
