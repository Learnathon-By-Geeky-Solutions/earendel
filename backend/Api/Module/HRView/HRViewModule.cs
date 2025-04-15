using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.HRView.HRFunc;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.HRView;
public static class HRViewModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("HR") { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // Endpoints for Jobs
            var hrView = app.MapGroup("HR").WithTags("HRView");
            hrView.MapCreateJobEndpoint();
            hrView.MapGetAllJobApplicationsEndpoint();

        }
    }

    public static WebApplicationBuilder RegisterHRViewServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<JobDbContext>();
        builder.Services.BindDbContext<QuizzesDbContext>();

        // Register repositories for Jobs

        return builder;
    }

    public static WebApplication UseHRViewModule(this WebApplication app)
    {
        return app;
    }
}

