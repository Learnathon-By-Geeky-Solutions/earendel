using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Delete.v1;

public sealed record DeleteCandidateProfileCommand(
    Guid Id) : IRequest;
