using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.CandidateLogic.JobApplicationView;
using TalentMesh.Module.CandidateLogic.JobView;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Module.Quizzes.Api.Endpoints;

namespace TalentMesh.Module.CandidateLogic
{
    public static class CandidateLogicModule
    {
        public class Endpoints : CarterModule
        {
            public Endpoints() : base("JobView") { }

            public override void AddRoutes(IEndpointRouteBuilder app)
            {
                // Endpoints for Jobs
                var jobView = app.MapGroup("candidateview").WithTags("JobViewCandidate");
                jobView.MapJobViewEndpoints();
                jobView.MapJobApplicationViewEndpoints();
                jobView.MapStartQuizEndpoint();
                jobView.MapGetQuizQuestionEndpoint();
                jobView.MapSubmitAnswerEndpoint();

            }
        }

        public static WebApplicationBuilder RegisterCandidateLogicServices(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.Services.BindDbContext<JobDbContext>();

            // Register repositories for Jobs
            builder.Services.AddKeyedScoped<IReadRepository<Jobs>, JobRepository<Jobs>>("jobs:jobReadOnly");
            builder.Services.AddKeyedScoped<IReadRepository<JobApplication>, JobRepository<JobApplication>>("jobApplication:jobApplicationReadOnly");
         
            return builder;
        }

        public static WebApplication UseCandidateLogicModule(this WebApplication app)
        {
            return app;
        }
    }
}
