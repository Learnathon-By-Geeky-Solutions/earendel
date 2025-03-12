using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using Evaluator.Application.Interviewer.Get.v1;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    public class SearchInterviewerEntryFormsCommand : PaginationFilter, IRequest<PagedList<InterviewerEntryFormResponse>>
    {
        // Optional filters: search by AdditionalInfo and/or Status.
        public string? AdditionalInfo { get; set; }
        public string? Status { get; set; }
    }
}
