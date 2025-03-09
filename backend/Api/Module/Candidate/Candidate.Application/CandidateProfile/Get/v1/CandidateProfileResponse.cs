using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

public sealed record CandidateProfileResponse(Guid Id, string Resume, string Skills, string Experience, string Education, Guid UserId);

