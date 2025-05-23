using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1
{
    [ExcludeFromCodeCoverage]
    public sealed class UpdateSeniorityLevelJunctionHandler : IRequestHandler<UpdateSeniorityLevelJunctionCommand, UpdateSeniorityLevelJunctionResponse>
    {
        private readonly ILogger<UpdateSeniorityLevelJunctionHandler> _logger;
        private readonly IRepository<SeniorityLevelJunction> _repository;

        public UpdateSeniorityLevelJunctionHandler(
            ILogger<UpdateSeniorityLevelJunctionHandler> logger,
            [FromKeyedServices("seniorityleveljunctions:seniorityleveljunction")] IRepository<SeniorityLevelJunction> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<UpdateSeniorityLevelJunctionResponse> Handle(UpdateSeniorityLevelJunctionCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // List all junctions for the given SkillId.
            var existingJunctions = await _repository.ListAsync(cancellationToken) ?? new List<SeniorityLevelJunction>();

            // Filter the junctions in-memory based on SkillId.
            var junctionsToRemove = existingJunctions.Where(j => j.SkillId == request.SkillId).ToList();

            // Check if there are any junctions to remove.
            if (junctionsToRemove.Count == 0)
            {
                throw new SeniorityLevelJunctionNotFoundException(request.SkillId);
            }

            foreach (var junction in junctionsToRemove)
            {
                await _repository.DeleteAsync(junction, cancellationToken);
            }

            // Create new junction records for each provided seniority level
            var createdJunctionIds = new List<Guid>();
            foreach (var seniorityLevelId in request.SeniorityLevelIds)
            {
                var junction = SeniorityLevelJunction.Create(seniorityLevelId, request.SkillId);
                await _repository.AddAsync(junction, cancellationToken);
                createdJunctionIds.Add(junction.Id);
                _logger.LogInformation("Created Seniority Level Junction with ID: {JunctionId}", junction.Id);
            }

            return new UpdateSeniorityLevelJunctionResponse(createdJunctionIds);
        }
    }
}
