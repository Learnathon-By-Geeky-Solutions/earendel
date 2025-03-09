using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Candidate.Domain.Exceptions;

public sealed class CandidateProfileNotFoundException : NotFoundException
{
    public CandidateProfileNotFoundException(Guid id)
        : base($"CandidateProfile with id {id} not found")
    {
    }
}

