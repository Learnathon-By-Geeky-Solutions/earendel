using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;

namespace TalentMesh.Module.CandidateLogic.JobView;

public record JobViewFilters(
        string? Name,
        string? Description,
        string? Requirements,
        string? Location,
        string? JobType,
        string? ExperienceLevel) : IRequest<IResult>;