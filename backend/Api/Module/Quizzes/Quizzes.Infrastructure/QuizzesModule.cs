using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure;
[ExcludeFromCodeCoverage]

public static class QuizzesModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("quizzes") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var quizAttemptGroup = app.MapGroup("quizattempts").WithTags("quizattempts");
            quizAttemptGroup.MapQuizAttemptCreationEndpoint();
            quizAttemptGroup.MapGetQuizAttemptEndpoint();
            quizAttemptGroup.MapGetQuizAttemptListEndpoint();
            quizAttemptGroup.MapQuizAttemptUpdateEndpoint();
            quizAttemptGroup.MapQuizAttemptDeleteEndpoint();

            var quizQuestionGroup = app.MapGroup("quizquestions").WithTags("quizquestions");
            quizQuestionGroup.MapQuizQuestionCreationEndpoint();
            quizQuestionGroup.MapGetQuizQuestionEndpoint();
            quizQuestionGroup.MapGetQuizQuestionListEndpoint();
            quizQuestionGroup.MapQuizQuestionUpdateEndpoint();
            quizQuestionGroup.MapQuizQuestionDeleteEndpoint();

            var quizAttemptAnswerGroup = app.MapGroup("quizattemptanswers").WithTags("quizattemptanswers");
            quizAttemptAnswerGroup.MapQuizAttemptAnswerCreationEndpoint();
            quizAttemptAnswerGroup.MapGetQuizAttemptAnswerEndpoint();
            quizAttemptAnswerGroup.MapGetQuizAttemptAnswerListEndpoint();
            quizAttemptAnswerGroup.MapQuizAttemptAnswerUpdateEndpoint();
            quizAttemptAnswerGroup.MapQuizAttemptAnswerDeleteEndpoint();

        }
    }
    public static WebApplicationBuilder RegisterQuizzesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<QuizzesDbContext>();
        builder.Services.AddScoped<IDbInitializer, QuizzesDbInitializer>();

        builder.Services.AddKeyedScoped<IRepository<QuizAttempt>, QuizzesRepository<QuizAttempt>>("quizattempts:quizattempt");
        builder.Services.AddKeyedScoped<IReadRepository<QuizAttempt>, QuizzesRepository<QuizAttempt>>("quizattempts:quizattemptReadOnly");

        builder.Services.AddKeyedScoped<IRepository<QuizQuestion>, QuizzesRepository<QuizQuestion>>("quizquestions:quizquestion");
        builder.Services.AddKeyedScoped<IReadRepository<QuizQuestion>, QuizzesRepository<QuizQuestion>>("quizquestions:quizquestionReadOnly");

        builder.Services.AddKeyedScoped<IRepository<QuizAttemptAnswer>, QuizzesRepository<QuizAttemptAnswer>>("quizattemptanswers:quizattemptanswer");
        builder.Services.AddKeyedScoped<IReadRepository<QuizAttemptAnswer>, QuizzesRepository<QuizAttemptAnswer>>("quizattemptanswers:quizattemptanswerReadOnly");

        return builder;
    }
    public static WebApplication UseQuizzesModule(this WebApplication app)
    {
        return app;
    }
}
