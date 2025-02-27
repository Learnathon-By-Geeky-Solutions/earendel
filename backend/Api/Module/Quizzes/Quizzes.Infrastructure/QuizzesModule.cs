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

namespace TalentMesh.Module.Quizzes.Infrastructure;
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

        }
    }
    public static WebApplicationBuilder RegisterQuizzesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<QuizzesDbContext>();
        builder.Services.AddScoped<IDbInitializer, QuizzesDbInitializer>();

        builder.Services.AddKeyedScoped<IRepository<QuizAttempt>, QuizzesRepository<QuizAttempt>>("quizattempts:quizattempt");
        builder.Services.AddKeyedScoped<IReadRepository<QuizAttempt>, QuizzesRepository<QuizAttempt>>("quizattempts:quizattemptReadOnly");

        return builder;
    }
    public static WebApplication UseQuizzesModule(this WebApplication app)
    {
        return app;
    }
}
