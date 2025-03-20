using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using Evaluator.Application.Interviewer.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    [ExcludeFromCodeCoverage]
    public class SearchInterviewerAvailabilitiesCommand : PaginationFilter, IRequest<PagedList<InterviewerAvailabilityResponse>>
    {
        // Optional filter: search by InterviewerId and/or availability status.
        public Guid? InterviewerId { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
