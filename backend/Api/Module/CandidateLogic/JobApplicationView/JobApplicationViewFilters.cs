using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace TalentMesh.Module.CandidateLogic.JobApplicationView 
{
    public record JobApplicationViewFilters(
        Guid? JobId,
        Guid? CandidateId, // User ID
        [DataType(DataType.Date)]
        DateTime? ApplicationDateStart,
        [DataType(DataType.Date)]
        DateTime? ApplicationDateEnd,
        [StringLength(50)]
        string? Status
        
        ) : IRequest<IResult>;
}