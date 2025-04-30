using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Shared.Authorization;

namespace TalentMesh.Shared.Authorization;

[ExcludeFromCodeCoverage]
public static class TMPermissions
{
    private static readonly FshPermission[] AllPermissions =
    {
        // Existing permissions
        new(TMActions.View, TMResources.Jobs, "View Jobs", IsBasic: true),
        new(TMActions.Create, TMResources.Jobs, "Create Jobs"),
        new(TMActions.Update, TMResources.Jobs, "Update Jobs"),
        new(TMActions.Delete, TMResources.Jobs, "Delete Jobs"),

        new(TMActions.View,   TMResources.CandidateProfiles, "View Candidate Profiles"),
        new(TMActions.Create, TMResources.CandidateProfiles, "Create Candidate Profiles"),
        new(TMActions.Update, TMResources.CandidateProfiles, "Update Candidate Profiles"),
        new(TMActions.Delete, TMResources.CandidateProfiles, "Delete Candidate Profiles"), // optional if you’ll ever need Delete


        new(TMActions.View, TMResources.JobApplications, "View Job Applications", IsBasic: true),
        new(TMActions.Create, TMResources.JobApplications, "Create Job Applications"),
        new(TMActions.Update, TMResources.JobApplications, "Update Job Applications"),
        new(TMActions.Delete, TMResources.JobApplications, "Delete Job Applications"),

        new(TMActions.View, TMResources.Users, "View Users"),
        new(TMActions.Create, TMResources.Users, "Create Users"),
        new(TMActions.Update, TMResources.Users, "Update Users"),
        new(TMActions.Delete, TMResources.Users, "Delete Users"),

        new(TMActions.View, TMResources.Roles, "View Roles"),
        new(TMActions.Create, TMResources.Roles, "Create Roles"),
        new(TMActions.Update, TMResources.Roles, "Update Roles"),
        new(TMActions.Delete, TMResources.Roles, "Delete Roles"),

        new(TMActions.View, TMResources.Candidates, "View Candidates"),
        new(TMActions.Create, TMResources.Candidates, "Create Candidates"),
        new(TMActions.Update, TMResources.Candidates, "Update Candidates"),
        new(TMActions.Delete, TMResources.Candidates, "Delete Candidates"),

        // Interviewer Applications
        new(TMActions.View,   TMResources.InterviewerApplications, "View Interviewer Applications"),
        new(TMActions.Create, TMResources.InterviewerApplications, "Create Interviewer Applications"),
        new(TMActions.Update, TMResources.InterviewerApplications, "Update Interviewer Applications"),
        new(TMActions.Delete, TMResources.InterviewerApplications, "Delete Interviewer Applications"),

        // Interviewer Availabilities
        new(TMActions.View,   TMResources.InterviewerAvailabilities, "View Interviewer Availabilities"),
        new(TMActions.Create, TMResources.InterviewerAvailabilities, "Create Interviewer Availabilities"),
        new(TMActions.Update, TMResources.InterviewerAvailabilities, "Update Interviewer Availabilities"),
        new(TMActions.Delete, TMResources.InterviewerAvailabilities, "Delete Interviewer Availabilities"),

        // Interviewer Entry Forms
        new(TMActions.View,   TMResources.InterviewerEntryForms, "View Interviewer Entry Forms"),
        new(TMActions.Create, TMResources.InterviewerEntryForms, "Create Interviewer Entry Forms"),
        new(TMActions.Update, TMResources.InterviewerEntryForms, "Update Interviewer Entry Forms"),
        new(TMActions.Delete, TMResources.InterviewerEntryForms, "Delete Interviewer Entry Forms"),


        new(TMActions.Create,   TMResources.ApproveInterviewers, "Create Interviewer Entry Forms"),
        new(TMActions.Create,   TMResources.RejectInterviewers, "Create Interviewer Entry Forms"),


        // — New full CRUD for Rubrics
        new(TMActions.View,   TMResources.Rubrics, "View Rubrics"),
        new(TMActions.Create, TMResources.Rubrics, "Create Rubrics"),
        new(TMActions.Update, TMResources.Rubrics, "Update Rubrics"),
        new(TMActions.Delete, TMResources.Rubrics, "Delete Rubrics"),

        // — New full CRUD for Seniorities
        new(TMActions.View,   TMResources.Seniorities, "View Seniorities"),
        new(TMActions.Create, TMResources.Seniorities, "Create Seniorities"),
        new(TMActions.Update, TMResources.Seniorities, "Update Seniorities"),
        new(TMActions.Delete, TMResources.Seniorities, "Delete Seniorities"),

        // — New full CRUD for SeniorityLevelJunctions
        new(TMActions.View,   TMResources.SeniorityLevelJunctions, "View Seniority-Level Associations"),
        new(TMActions.Create, TMResources.SeniorityLevelJunctions, "Create Seniority-Level Associations"),
        new(TMActions.Update, TMResources.SeniorityLevelJunctions, "Update Seniority-Level Associations"),
        new(TMActions.Delete, TMResources.SeniorityLevelJunctions, "Delete Seniority-Level Associations"),

        // — New full CRUD for Skills
        new(TMActions.View,   TMResources.Skills, "View Skills"),
        new(TMActions.Create, TMResources.Skills, "Create Skills"),
        new(TMActions.Update, TMResources.Skills, "Update Skills"),
        new(TMActions.Delete, TMResources.Skills, "Delete Skills"),

        // — New full CRUD for Subskills
        new(TMActions.View,   TMResources.Subskills, "View Subskills"),
        new(TMActions.Create, TMResources.Subskills, "Create Subskills"),
        new(TMActions.Update, TMResources.Subskills, "Update Subskills"),
        new(TMActions.Delete, TMResources.Subskills, "Delete Subskills"),

        new(TMActions.View, TMResources.Interviews, "View Interviews"),
        new(TMActions.Create, TMResources.Interviews, "Create Interviews"),
        new(TMActions.Update, TMResources.Interviews, "Update Interviews"),
        new(TMActions.Delete, TMResources.Interviews, "Delete Interviews"), // Not needed now but adding for consistency

// — New permissions for InterviewFeedbacks
new(TMActions.View, TMResources.InterviewFeedbacks, "View Interview Feedbacks"),
new(TMActions.Create, TMResources.InterviewFeedbacks, "Create Interview Feedbacks"),
new(TMActions.Update, TMResources.InterviewFeedbacks, "Update Interview Feedbacks"),
new(TMActions.Delete, TMResources.InterviewFeedbacks, "Delete Interview Feedbacks"), // For consistency

// — New permissions for InterviewQuestions
new(TMActions.View, TMResources.InterviewQuestions, "View Interview Questions"),
new(TMActions.Create, TMResources.InterviewQuestions, "Create Interview Questions"),
new(TMActions.Update, TMResources.InterviewQuestions, "Update Interview Questions"),
new(TMActions.Delete, TMResources.InterviewQuestions, "Delete Interview Questions"), // For consistency


new(TMActions.View, TMResources.Notifications, "View Notification Questions"),
new(TMActions.Create, TMResources.Notifications, "Create Notification Questions"),
new(TMActions.Update, TMResources.Notifications, "Update Notification Questions"),
new(TMActions.Delete, TMResources.Notifications, "Delete Notification Questions"),


// — New permissions for InterviewSignature
new(TMActions.View, TMResources.InterviewSignatures, "View Interview Signature"),
new(TMActions.Create, TMResources.InterviewSignatures, "Create Interview Signature"),
new(TMActions.Update, TMResources.InterviewSignatures, "Update Interview Signature"),
new(TMActions.Delete, TMResources.InterviewSignatures, "Delete Interview Signature"),

    // — New: JobRequiredSkill
    new(TMActions.View, TMResources.JobRequiredSkills, "View Job Required Skills"),
    new(TMActions.Create, TMResources.JobRequiredSkills, "Create Job Required Skills"),

    // — New: JobRequiredSubskill
    new(TMActions.View, TMResources.JobRequiredSubskills, "View Job Required Subskills"),
    new(TMActions.Create, TMResources.JobRequiredSubskills, "Create Job Required Subskills"),


    // — New full CRUD for QuizAttempts
new(TMActions.Create, TMResources.QuizAttempts,       "Create Quiz Attempts"),
new(TMActions.View,   TMResources.QuizAttempts,       "View Quiz Attempts"),
new(TMActions.Update, TMResources.QuizAttempts,       "Update Quiz Attempts"),
new(TMActions.Delete, TMResources.QuizAttempts,       "Delete Quiz Attempts"),

// — New full CRUD for QuizAttemptAnswers
new(TMActions.Create, TMResources.QuizAttemptAnswers, "Create Quiz Attempt Answers"),
new(TMActions.View,   TMResources.QuizAttemptAnswers, "View Quiz Attempt Answers"),
new(TMActions.Update, TMResources.QuizAttemptAnswers, "Update Quiz Attempt Answers"),
new(TMActions.Delete, TMResources.QuizAttemptAnswers, "Delete Quiz Attempt Answers"),

// (QuizQuestions: added to AllPermissions if you want to define them,
//  but NOT included in any RolePermissions grants them effectively none.)
new(TMActions.View,   TMResources.QuizQuestions,      "View Quiz Questions"),
new(TMActions.Create, TMResources.QuizQuestions,      "Create Quiz Questions"),
new(TMActions.Update, TMResources.QuizQuestions,      "Update Quiz Questions"),
new(TMActions.Delete, TMResources.QuizQuestions,      "Delete Quiz Questions"),


    };

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());

    public static Dictionary<string, List<FshPermission>> RolePermissions { get; } = new()
    {
        [TMRoles.Admin] = AllPermissions.Where(p => !p.IsRoot).ToList(),

        [TMRoles.Basic] = AllPermissions.Where(p => p.IsBasic).ToList(),

        [TMRoles.HR] = AllPermissions.Where(p =>
            p.Resource == TMResources.Jobs ||
            p.Resource == TMResources.JobApplications ||
            (p.Resource == TMResources.Candidates && p.Action == TMActions.View) ||
            (p.Resource == TMResources.InterviewerApplications && p.Action == TMActions.View) ||
            (p.Resource == TMResources.InterviewerAvailabilities && p.Action == TMActions.View) ||
            (p.Resource == TMResources.InterviewerEntryForms && p.Action == TMActions.View) ||

            // — New resources for HR: view only
            (p.Resource == TMResources.Rubrics && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Seniorities && p.Action == TMActions.View) ||
            (p.Resource == TMResources.SeniorityLevelJunctions && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Skills && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Subskills && p.Action == TMActions.View) ||
            (p.Resource == TMResources.QuizQuestions && p.Action == TMActions.View) ||

            (p.Resource == TMResources.Notifications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||


             (p.Resource == TMResources.Interviews && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

           // — HR access to InterviewFeedbacks (view only)
           (p.Resource == TMResources.InterviewFeedbacks && p.Action == TMActions.View) ||

    // — HR access to InterviewQuestions (view only)
    (p.Resource == TMResources.InterviewQuestions && p.Action == TMActions.View) ||

    // — HR access to InterviewSignature (view only)
    (p.Resource == TMResources.InterviewSignatures && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

      (p.Resource == TMResources.JobRequiredSkills && p.Action == TMActions.View) ||
    (p.Resource == TMResources.JobRequiredSubskills && p.Action == TMActions.View) ||

     (p.Resource == TMResources.QuizAttempts && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||
    (p.Resource == TMResources.QuizAttemptAnswers && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update))
        ).ToList(),

        [TMRoles.Candidate] = AllPermissions.Where(p =>
            (p.Resource == TMResources.Jobs && p.Action == TMActions.View) ||
            (p.Resource == TMResources.JobApplications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||
            (p.Resource == TMResources.InterviewerAvailabilities && p.Action == TMActions.View) ||

            (p.Resource == TMResources.Notifications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||

            (p.Resource == TMResources.InterviewerEntryForms && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||

            (p.Resource == TMResources.InterviewerApplications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||


            // — New resources for Candidate: view only
            (p.Resource == TMResources.Rubrics && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Seniorities && p.Action == TMActions.View) ||
            (p.Resource == TMResources.SeniorityLevelJunctions && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Skills && p.Action == TMActions.View) ||
            (p.Resource == TMResources.Subskills && p.Action == TMActions.View) ||
            (p.Resource == TMResources.QuizQuestions && p.Action == TMActions.View) ||

    // — Candidate view Interviews
    (p.Resource == TMResources.Interviews && p.Action == TMActions.View) ||

    // — Candidate view InterviewFeedbacks
    (p.Resource == TMResources.InterviewFeedbacks && p.Action == TMActions.View) ||

    // — Candidate view InterviewQuestions
    (p.Resource == TMResources.InterviewQuestions && p.Action == TMActions.View) ||

    // — Candidate view InterviewSignature
    (p.Resource == TMResources.InterviewSignatures && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

     (p.Resource == TMResources.JobRequiredSkills && p.Action == TMActions.View) ||
    (p.Resource == TMResources.JobRequiredSubskills && p.Action == TMActions.View) ||
     (p.Resource == TMResources.QuizAttempts && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||
    (p.Resource == TMResources.QuizAttemptAnswers && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) || (p.Resource == TMResources.CandidateProfiles &&
    (p.Action == TMActions.View ||
     p.Action == TMActions.Create ||
     p.Action == TMActions.Update))

    ).ToList(),


        [TMRoles.Interviewer] = AllPermissions.Where(p =>
            (p.Resource == TMResources.Jobs && p.Action == TMActions.View) ||
            (p.Resource == TMResources.JobApplications && (p.Action == TMActions.View || p.Action == TMActions.Update)) ||
            (p.Resource == TMResources.Candidates && p.Action == TMActions.View) ||
            (p.Resource == TMResources.InterviewerApplications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||
            (p.Resource == TMResources.InterviewerAvailabilities && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||
            (p.Resource == TMResources.InterviewerEntryForms && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

            (p.Resource == TMResources.Notifications && (p.Action == TMActions.View || p.Action == TMActions.Create)) ||

            // — New resources for Interviewer: full CRUD
            (p.Resource == TMResources.Rubrics && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update || p.Action == TMActions.Delete)) ||
            (p.Resource == TMResources.Seniorities && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update || p.Action == TMActions.Delete)) ||
            (p.Resource == TMResources.SeniorityLevelJunctions && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update || p.Action == TMActions.Delete)) ||
            (p.Resource == TMResources.Skills && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update || p.Action == TMActions.Delete)) ||
            (p.Resource == TMResources.Subskills && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update || p.Action == TMActions.Delete)) ||

            (p.Resource == TMResources.Interviews && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

    // — Interviewer access to InterviewFeedbacks (view, create, update)
    (p.Resource == TMResources.InterviewFeedbacks && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

    // — Interviewer access to InterviewQuestions (view, create, update)
    (p.Resource == TMResources.InterviewQuestions && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

    // — Interviewer access to InterviewSignature (view, create, update)
    (p.Resource == TMResources.InterviewSignatures && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||

    (p.Resource == TMResources.JobRequiredSkills && p.Action == TMActions.View) ||
    (p.Resource == TMResources.JobRequiredSubskills && p.Action == TMActions.View) ||

 (p.Resource == TMResources.QuizAttempts && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update)) ||
    (p.Resource == TMResources.QuizAttemptAnswers && (p.Action == TMActions.View || p.Action == TMActions.Create || p.Action == TMActions.Update))
).ToList(),
    };

    public static IReadOnlyList<FshPermission> GetPermissionsForRole(string roleName)
    {
        if (RolePermissions.TryGetValue(roleName, out var permissions))
            return new ReadOnlyCollection<FshPermission>(permissions);
        return new ReadOnlyCollection<FshPermission>(new List<FshPermission>());
    }
}

[ExcludeFromCodeCoverage]
public record FshPermission(string Action, string Resource, string Description, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
