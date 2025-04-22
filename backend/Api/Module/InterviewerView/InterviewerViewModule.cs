using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Evaluator.Application.Interviewer;
using TalentMesh.Module.Evaluator.Infrastructure.Endpoints; // Ensure this is using
using TalentMesh.Module.Evaluator.Infrastructure.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;
using Carter.OpenApi;
using TalentMesh.Module.Interviews.Application;

namespace TalentMesh.Module.InterviewerView// Corrected namespace based on file context
{
    public static class InterviewerViewModule
    {
        public class Endpoints : CarterModule
        {
            // Keep the base route if other endpoints in this module need it
            public Endpoints() : base("InterviewerView") { }

            public override void AddRoutes(IEndpointRouteBuilder app)
            {
                // Keep endpoints specific to the Interviewer View under this group
                var interviewerViewGroup = app.MapGroup("Interviewer").WithTags("View");
                interviewerViewGroup.MapApproveInterviewer(); // Assuming these exist
                interviewerViewGroup.MapRejectInterviewer(); // Assuming these exist
                interviewerViewGroup.MapRequestInterviewer(); // Assuming these exist
                interviewerViewGroup.MapInterviewQuestion();
                interviewerViewGroup.MapInterviewFeedback();
                interviewerViewGroup.MapUpcomingInterviews();
                // Register the UploadCv endpoint directly on the app builder
                // This respects the absolute path defined within MapUploadCv
                //app.MapUploadCv();
                // ^^^ MOVED HERE ^^^
            }
        }

        // ... rest of the RegisterInterviewerViewServices and UseInterviewerViewModule methods remain the same
        public static WebApplicationBuilder RegisterInterviewerViewServices(this WebApplicationBuilder builder)
        {
            // --- Bind EF Core contexts ---
            builder.Services.BindDbContext<JobDbContext>();         // your job store
            builder.Services.BindDbContext<QuizzesDbContext>();      // quizzes store
            builder.Services.BindDbContext<EvaluatorDbContext>();    // interviewer-entry store

            // --- Register repositories for all your contexts ---
            builder.Services.AddScoped(
                typeof(IReadRepository<>),
                typeof(EvaluatorRepository<>)); // Assuming EvaluatorRepository exists
            builder.Services.AddScoped(
                typeof(IRepository<>),
                typeof(EvaluatorRepository<>)); // Assuming EvaluatorRepository exists

            // --- MediatR handlers (if not already added elsewhere) ---
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<RequestInterviewerCommand>()); // Or UploadCvCommand

            return builder;
        }


        public static WebApplication UseInterviewerViewModule(this WebApplication app)
        {
            // If you have middleware specific to this module, add it here.
            // Otherwise, this might just be mapping endpoints via Carter.
            // Ensure Carter is mapped globally, e.g., app.MapCarter(); in Program.cs
            return app;
        }
    }
}