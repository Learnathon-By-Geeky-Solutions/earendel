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
using TalentMesh.Module.Experties.Infrastructure.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure;
[ExcludeFromCodeCoverage]
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
            subSkillGroup.MapSubSkillCreationEndpoint();
            subSkillGroup.MapGetSubSkillEndpoint();
            subSkillGroup.MapGetSubSkillListEndpoint();
            subSkillGroup.MapSubSkillUpdateEndpoint();
            subSkillGroup.MapSubSkillDeleteEndpoint();

            var rubricGroup = app.MapGroup("rubrics").WithTags("rubrics");
            rubricGroup.MapRubricCreationEndpoint();
            rubricGroup.MapGetRubricEndpoint();
            rubricGroup.MapGetRubricListEndpoint();
            rubricGroup.MapRubricUpdateEndpoint();
            rubricGroup.MapRubricDeleteEndpoint();

            var seniorityGroup = app.MapGroup("seniorities").WithTags("seniorities");
            seniorityGroup.MapSeniorityCreationEndpoint();
            seniorityGroup.MapGetSeniorityEndpoint();
            seniorityGroup.MapGetSeniorityListEndpoint();
            seniorityGroup.MapSeniorityUpdateEndpoint();
            seniorityGroup.MapSeniorityDeleteEndpoint();

            var seniorityLevelJunctionGroup = app.MapGroup("seniorityleveljunctions").WithTags("seniorityleveljunctions");
            seniorityLevelJunctionGroup.MapSeniorityLevelJunctionCreationEndpoint();
            seniorityLevelJunctionGroup.MapGetSeniorityLevelJunctionEndpoint();
            seniorityLevelJunctionGroup.MapGetSeniorityLevelJunctionListEndpoint();
            seniorityLevelJunctionGroup.MapSeniorityLevelJunctionUpdateEndpoint();
            seniorityLevelJunctionGroup.MapSeniorityLevelJunctionDeletionEndpoint();

        }
    }
    public static WebApplicationBuilder RegisterExpertiesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<ExpertiesDbContext>();
        builder.Services.AddScoped<IDbInitializer, ExpertiesDbInitializer>();

        builder.Services.AddHostedService<SkillsCreatedConsumer>();

        builder.Services.AddKeyedScoped<IRepository<Skill>, ExpertiesRepository<Skill>>("skills:skill");
        builder.Services.AddKeyedScoped<IReadRepository<Skill>, ExpertiesRepository<Skill>>("skills:skillReadOnly");

        builder.Services.AddKeyedScoped<IRepository<SubSkill>, ExpertiesRepository<SubSkill>>("subskills:subskill");
        builder.Services.AddKeyedScoped<IReadRepository<SubSkill>, ExpertiesRepository<SubSkill>>("subskills:subskillReadOnly");

        builder.Services.AddKeyedScoped<IRepository<Rubric>, ExpertiesRepository<Rubric>>("rubrics:rubric");
        builder.Services.AddKeyedScoped<IReadRepository<Rubric>, ExpertiesRepository<Rubric>>("rubrics:rubricReadOnly");

        builder.Services.AddKeyedScoped<IRepository<Seniority>, ExpertiesRepository<Seniority>>("seniorities:seniority");
        builder.Services.AddKeyedScoped<IReadRepository<Seniority>, ExpertiesRepository<Seniority>>("seniorities:seniorityReadOnly");

        builder.Services.AddKeyedScoped<IRepository<SeniorityLevelJunction>, ExpertiesRepository<SeniorityLevelJunction>>("seniorityleveljunctions:seniorityleveljunction");
        builder.Services.AddKeyedScoped<IReadRepository<SeniorityLevelJunction>, ExpertiesRepository<SeniorityLevelJunction>>("seniorityleveljunctions:seniorityleveljunctionReadOnly");
        return builder;
    }
    public static WebApplication UseExpertiesModule(this WebApplication app)
    {
        return app;
    }
}
