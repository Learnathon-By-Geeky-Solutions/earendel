using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace TalentMesh.Module.CandidateLogic.JobView;

public record JobViewFilters(
        [StringLength(100)]
        string? Name,
        [StringLength(1000)]
        string? Description,
        [StringLength(1000)]
        string? Requirements,
        [StringLength(50)]
        string? Location,
        [StringLength(50)]
        string? JobType,
        [StringLength(50)]
        string? ExperienceLevel) : IRequest<IResult>;