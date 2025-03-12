using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using Evaluator.Application.Interviewer.Get.v1;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    public class SearchInterviewerApplicationsCommand : PaginationFilter, IRequest<PagedList<InterviewerApplicationResponse>>
    {
        // Optional filters
        public string? Status { get; set; }
        public string? Comments { get; set; }
    }
}
