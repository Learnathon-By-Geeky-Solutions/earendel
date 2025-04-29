namespace TalentMesh.Shared.Authorization;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public static class TMResources
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Jobs = nameof(Jobs);
    public const string JobApplications = nameof(JobApplications);
    public const string Candidates = nameof(Candidates);
    public const string InterviewerApplications = nameof(InterviewerApplications);
    public const string InterviewerAvailabilities = nameof(InterviewerAvailabilities);
    public const string InterviewerEntryForms = nameof(InterviewerEntryForms);
    public const string Rubrics = nameof(Rubrics);
    public const string Seniorities = nameof(Seniorities);
    public const string SeniorityLevelJunctions = nameof(SeniorityLevelJunctions);
    public const string Skills = nameof(Skills);
    public const string Subskills = nameof(Subskills);
    public const string Interviews = nameof(Interviews); 
    public const string InterviewFeedbacks = nameof(InterviewFeedbacks); 
    public const string InterviewQuestions = nameof(InterviewQuestions); 
    public const string InterviewSignatures = nameof(InterviewSignatures); 
    public const string JobRequiredSkills = nameof(JobRequiredSkills); 
    public const string JobRequiredSubskills = nameof(JobRequiredSubskills); 
    public const string Notifications = nameof(Notifications); 
    public const string QuizAttemptAnswers = nameof(QuizAttemptAnswers); 
    public const string QuizAttempts = nameof(QuizAttempts); 
    public const string QuizQuestions = nameof(QuizQuestions); 
    public const string ApproveInterviewers = nameof(ApproveInterviewers); 
    public const string RejectInterviewers = nameof(RejectInterviewers); 
    public const string CandidateProfiles = nameof(CandidateProfiles); 
    public const string AuditTrails = nameof(AuditTrails);
}
