using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using Evaluator.Application.Interviewer.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    [ExcludeFromCodeCoverage]
    public class SearchInterviewerEntryFormsCommand : PaginationFilter, IRequest<PagedList<InterviewerEntryFormResponse>>
    {
        public Guid? UserId { get; set; } 
        public string? AdditionalInfo { get; set; }
        public string? Status { get; set; }
    }
}
