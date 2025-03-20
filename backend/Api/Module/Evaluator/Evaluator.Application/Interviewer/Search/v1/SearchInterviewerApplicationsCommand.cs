using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using Evaluator.Application.Interviewer.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    [ExcludeFromCodeCoverage]
    public class SearchInterviewerApplicationsCommand : PaginationFilter, IRequest<PagedList<InterviewerApplicationResponse>>
    {
        // Optional filters
        public string? Status { get; set; }
        public string? Comments { get; set; }
    }
}
