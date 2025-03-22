using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentMesh.Module.CandidateLogic;


class JobViewService : IRequestHandler<JobViewFilters, IEnumerable<Job.Domain.Jobs>>
{
    public Task<IEnumerable<Job.Domain.Jobs>> Handle(JobViewFilters request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

