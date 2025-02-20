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

namespace TalentMesh.Module.Job.Infrastructure;
public static class JobModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("job") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("jobs").WithTags("jobs");
            productGroup.MapJobCreationEndpoint();
            productGroup.MapGetJobEndpoint();
            productGroup.MapGetJobListEndpoint();
            productGroup.MapJobUpdateEndpoint();
            productGroup.MapJobDeleteEndpoint();


        }
    }
    public static WebApplicationBuilder RegisterJobServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<JobDbContext>();
        builder.Services.AddScoped<IDbInitializer, JobDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Jobs>, JobRepository<Jobs>>("jobs:job");
        builder.Services.AddKeyedScoped<IReadRepository<Jobs>, JobRepository<Jobs>>("jobs:jobReadOnly");
        return builder;
    }
    public static WebApplication UseJobModule(this WebApplication app)
    {
        return app;
    }
}
