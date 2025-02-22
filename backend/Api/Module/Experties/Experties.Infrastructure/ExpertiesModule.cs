using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Experties.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Experties.Infrastructure;
public static class ExpertiesModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("experties") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var skillGroup = app.MapGroup("skills").WithTags("skills");
            skillGroup.MapSkillCreationEndpoint();
            skillGroup.MapGetSkillEndpoint();
            skillGroup.MapGetSkillListEndpoint();
            skillGroup.MapSkillUpdateEndpoint();
            skillGroup.MapSkillDeleteEndpoint();

            var subSkillGroup = app.MapGroup("subskills").WithTags("subskills");
            skillGroup.MapSubSkillCreationEndpoint();
            skillGroup.MapGetSubSkillEndpoint();
            skillGroup.MapGetSubSkillListEndpoint();
            skillGroup.MapSubSkillUpdateEndpoint();
            skillGroup.MapSubSkillDeleteEndpoint();

        }
    }
    public static WebApplicationBuilder RegisterExpertiesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<ExpertiesDbContext>();
        builder.Services.AddScoped<IDbInitializer, ExpertiesDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Skill>, ExpertiesRepository<Skill>>("skills:skill");
        builder.Services.AddKeyedScoped<IReadRepository<Skill>, ExpertiesRepository<Skill>>("skills:skillReadOnly");
        builder.Services.AddKeyedScoped<IRepository<SubSkill>, ExpertiesRepository<SubSkill>>("subskills:subskill");
        builder.Services.AddKeyedScoped<IReadRepository<SubSkill>, ExpertiesRepository<SubSkill>>("subskills:subskillReadOnly");
        return builder;
    }
    public static WebApplication UseExpertiesModule(this WebApplication app)
    {
        return app;
    }
}
