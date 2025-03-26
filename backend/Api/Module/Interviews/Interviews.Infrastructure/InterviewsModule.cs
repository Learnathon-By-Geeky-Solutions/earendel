using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Interviews.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Module.Interviews.Infrastructure.Services;
using TalentMesh.Module.Interviews.Application.Services;

namespace TalentMesh.Module.Interviews.Infrastructure;
[ExcludeFromCodeCoverage]

public static class InterviewsModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("interviews") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var interviewGroup = app.MapGroup("interviews").WithTags("interviews");
            interviewGroup.MapInterviewCreationEndpoint();
            interviewGroup.MapInterviewSignatureCreationEndpoint();
            interviewGroup.MapGetInterviewEndpoint();
            interviewGroup.MapGetInterviewListEndpoint();
            interviewGroup.MapInterviewUpdateEndpoint();
            interviewGroup.MapInterviewDeleteEndpoint();

            var interviewQuestionGroup = app.MapGroup("interviewquestions").WithTags("interviewquestions");
            interviewQuestionGroup.MapInterviewQuestionCreationEndpoint();
            interviewQuestionGroup.MapGetInterviewQuestionEndpoint();
            interviewQuestionGroup.MapGetInterviewQuestionListEndpoint();
            interviewQuestionGroup.MapInterviewQuestionUpdateEndpoint();
            interviewQuestionGroup.MapInterviewQuestionDeleteEndpoint();

            var interviewFeedbackGroup = app.MapGroup("interviewfeedbacks").WithTags("interviewfeedbacks");
            interviewFeedbackGroup.MapInterviewFeedbackCreationEndpoint();
            interviewFeedbackGroup.MapGetInterviewFeedbackEndpoint();
            interviewFeedbackGroup.MapGetInterviewFeedbackListEndpoint();
            interviewFeedbackGroup.MapInterviewFeedbackUpdateEndpoint();
            interviewFeedbackGroup.MapInterviewFeedbackDeleteEndpoint();


        }
    }
    public static WebApplicationBuilder RegisterInterviewsServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<InterviewsDbContext>();
        builder.Services.AddScoped<IDbInitializer, InterviewsDbInitializer>();

        builder.Services.AddHttpClient<IZoomService, ZoomService>();

        builder.Services.AddKeyedScoped<IRepository<Interview>, InterviewsRepository<Interview>>("interviews:interview");
        builder.Services.AddKeyedScoped<IReadRepository<Interview>, InterviewsRepository<Interview>>("interviews:interviewReadOnly");

        builder.Services.AddKeyedScoped<IRepository<InterviewQuestion>, InterviewsRepository<InterviewQuestion>>("interviewquestions:interviewquestion");
        builder.Services.AddKeyedScoped<IReadRepository<InterviewQuestion>, InterviewsRepository<InterviewQuestion>>("interviewquestions:interviewquestionReadOnly");

        builder.Services.AddKeyedScoped<IRepository<InterviewFeedback>, InterviewsRepository<InterviewFeedback>>("interviewfeedbacks:interviewfeedback");
        builder.Services.AddKeyedScoped<IReadRepository<InterviewFeedback>, InterviewsRepository<InterviewFeedback>>("interviewfeedbacks:interviewfeedbackReadOnly");

        return builder;
    }
    public static WebApplication UseInterviewsModule(this WebApplication app)
    {
        return app;
    }
}
