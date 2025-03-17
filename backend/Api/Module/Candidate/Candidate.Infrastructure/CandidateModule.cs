using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Candidate.Domain;
using TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Candidate.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Candidate.Infrastructure
{
    public static class CandidateModule
    {
        public class Endpoints : CarterModule
        {
            public Endpoints() : base("candidate") { }

            public override void AddRoutes(IEndpointRouteBuilder app)
            {
                // Group endpoints for candidate profiles
                var candidateGroup = app.MapGroup("candidateprofiles").WithTags("candidateprofiles");
                candidateGroup.MapCandidateProfileCreationEndpoint();
                candidateGroup.MapGetCandidateProfileEndpoint();
                candidateGroup.MapGetCandidateProfileListEndpoint();
                candidateGroup.MapCandidateProfileUpdateEndpoint();
                candidateGroup.MapCandidateProfileDeleteEndpoint();
            }
        }

        public static WebApplicationBuilder RegisterCandidateServices(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.Services.BindDbContext<CandidateDbContext>();
            builder.Services.AddScoped<IDbInitializer, CandidateDbInitializer>();

            // Register CandidateProfile repository
            builder.Services.AddKeyedScoped<IRepository<CandidateProfile>, CandidateRepository<CandidateProfile>>("candidate:candidateprofile");
            builder.Services.AddKeyedScoped<IReadRepository<CandidateProfile>, CandidateRepository<CandidateProfile>>("candidate:candidateprofileReadOnly");

            return builder;
        }

        public static WebApplication UseCandidateModule(this WebApplication app)
        {
            // Apply any candidate-specific middleware or configurations
            ArgumentNullException.ThrowIfNull(app);



            return app;
        }
    }
}
