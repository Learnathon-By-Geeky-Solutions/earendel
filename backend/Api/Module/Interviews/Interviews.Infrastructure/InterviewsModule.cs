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

namespace TalentMesh.Module.Interviews.Infrastructure;
public static class InterviewsModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("interviews") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var interviewGroup = app.MapGroup("interviews").WithTags("interviews");
            interviewGroup.MapInterviewCreationEndpoint();
            interviewGroup.MapGetInterviewEndpoint();
            interviewGroup.MapGetInterviewListEndpoint();
            interviewGroup.MapInterviewUpdateEndpoint();
            interviewGroup.MapInterviewDeleteEndpoint();


        }
    }
    public static WebApplicationBuilder RegisterInterviewsServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<InterviewsDbContext>();
        builder.Services.AddScoped<IDbInitializer, InterviewsDbInitializer>();

        builder.Services.AddKeyedScoped<IRepository<Interview>, InterviewsRepository<Interview>>("interviews:interview");
        builder.Services.AddKeyedScoped<IReadRepository<Interview>, InterviewsRepository<Interview>>("interviews:interviewReadOnly");

        return builder;
    }
    public static WebApplication UseInterviewsModule(this WebApplication app)
    {
        return app;
    }
}
