using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Evaluator.Infrastructure.Persistence;
using TalentMesh.Module.Evaluator.Infrastructure.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class EvaluatorModule
    {
        public class Endpoints : CarterModule
        {
            public Endpoints() : base("evaluator") { }

            public override void AddRoutes(IEndpointRouteBuilder app)
            {
                // Group endpoints for Interviewer Applications
                var applicationGroup = app.MapGroup("interviewerapplications").WithTags("interviewerapplications");
                applicationGroup.MapInterviewerApplicationCreationEndpoint();
                applicationGroup.MapGetInterviewerApplicationEndpoint();
                applicationGroup.MapGetInterviewerApplicationListEndpoint();
                applicationGroup.MapInterviewerApplicationUpdateEndpoint();
                applicationGroup.MapInterviewerApplicationDeleteEndpoint();

                // Group endpoints for Interviewer Availabilities
                var availabilityGroup = app.MapGroup("intervieweravailabilities").WithTags("intervieweravailabilities");
                availabilityGroup.MapInterviewerAvailabilityCreationEndpoint();
                availabilityGroup.MapGetInterviewerAvailabilityEndpoint();
                availabilityGroup.MapGetInterviewerAvailabilityListEndpoint();
                availabilityGroup.MapInterviewerAvailabilityUpdateEndpoint();
                availabilityGroup.MapInterviewerAvailabilityDeleteEndpoint();

                // Group endpoints for Interviewer Entry Forms
                var entryFormGroup = app.MapGroup("interviewerentryforms").WithTags("interviewerentryforms");
                entryFormGroup.MapInterviewerEntryFormCreationEndpoint();
                entryFormGroup.MapGetInterviewerEntryFormEndpoint();
                entryFormGroup.MapGetInterviewerEntryFormListEndpoint();
                entryFormGroup.MapInterviewerEntryFormUpdateEndpoint();
                entryFormGroup.MapInterviewerEntryFormDeleteEndpoint();
            }
        }

        public static WebApplicationBuilder RegisterEvaluatorServices(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.Services.BindDbContext<EvaluatorDbContext>();
            builder.Services.AddScoped<IDbInitializer, EvaluatorDbInitializer>();

            builder.Services.AddHostedService<InterviewerFormConsumer>();

            // Register keyed repositories for InterviewerApplication
            builder.Services.AddKeyedScoped<IRepository<InterviewerApplication>, EvaluatorRepository<InterviewerApplication>>("interviews:interviewerapplication");
            builder.Services.AddKeyedScoped<IReadRepository<InterviewerApplication>, EvaluatorRepository<InterviewerApplication>>("interviews:interviewerapplicationReadOnly");

            // Register keyed repositories for InterviewerAvailability
            builder.Services.AddKeyedScoped<IRepository<InterviewerAvailability>, EvaluatorRepository<InterviewerAvailability>>("interviews:intervieweravailability");
            builder.Services.AddKeyedScoped<IReadRepository<InterviewerAvailability>, EvaluatorRepository<InterviewerAvailability>>("interviews:intervieweravailabilityReadOnly");

            // Register keyed repositories for InterviewerEntryForm
            builder.Services.AddKeyedScoped<IRepository<InterviewerEntryForm>, EvaluatorRepository<InterviewerEntryForm>>("interviews:interviewerentryform");
            builder.Services.AddKeyedScoped<IReadRepository<InterviewerEntryForm>, EvaluatorRepository<InterviewerEntryForm>>("interviews:interviewerentryformReadOnly");

            return builder;
        }

        public static WebApplication UseEvaluatorModule(this WebApplication app)
        {
            return app;
        }
    }
}
