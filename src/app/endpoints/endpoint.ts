// const base = 'https://localhost:7100';

const base = 'http://173.249.54.173';

export const endpoint = {
  googleLoginUrl: `${base}/api/users/google-login`,
  githubLoginUrl: `${base}/api/users/github-login`,
  userRegistrationUrl: `${base}/api/users/self-register`,
  userLoginUrl: `${base}/api/token`,
  refreshTokenUrl: `${base}/api/token/refresh`,
  hrDetailsUrl: `${base}/api/users/hr`,

  interviewerAvailabilityCreateUrl: `${base}/api/v1/evaluator/intervieweravailabilities`,
  interviewerAvailabilitySearchUrl: `${base}/api/v1/evaluator/intervieweravailabilities/search`,

  skillDetailsUrl: `${base}/api/v1/experties/skills/search`,
  skillCreatedUrl: `${base}/api/v1/experties/skills`,
  skillUpdatedUrl: `${base}/api/v1/experties/skills`,
  skillDeletedUrl: `${base}/api/v1/experties/skills`,

  subskillCreatedUrl: `${base}/api/v1/experties/subskills`,
  subskillUpdatedUrl: `${base}/api/v1/experties/subskills`,
  subskillDeletedUrl: `${base}/api/v1/experties/subskills`,

  seniorityDetailsUrl: `${base}/api/v1/experties/seniorities/search`,
  seniorityCreatedUrl: `${base}/api/v1/experties/seniorities`,
  seniorityUpdatedsUrl: `${base}/api/v1/experties/seniorities`,
  seniorityDeletedUrl: `${base}/api/v1/experties/seniorities`,

  searchRubricUrl: `${base}/api/v1/experties/rubrics/search`,
  rubricCreatedUrl: `${base}/api/v1/experties/rubrics`,
  rubricUpdatedUrl: `${base}/api/v1/experties/rubrics`,
  rubricDeletedUrl: `${base}/api/v1/experties/rubrics`,

  quizQuestionDetailsUrl: `${base}/api/v1/quizzes/quizquestions/search`,
  quizQuestionCreatedUrl: `${base}/api/v1/quizzes/quizquestions`,
  quizQuestionUpdatedUrl: `${base}/api/v1/quizzes/quizquestions`,
  quizQuestionDeletedUrl: `${base}/api/v1/quizzes/quizquestions`,

  getZoomSignatureUrl: `${base}/api/v1/interviews/interviews/signature`,

  getInterviewerEntryFormList: `${base}/api/v1/evaluator/interviewerentryforms/search`,
  downloadInterviewerEntryFormPdf: `${base}/api/v1/interviewers`,
  uploadInterviewerEntryFormPdf: `${base}/api/v1/interviewers`,
  approveInterviewerEntryForm: `${base}/api/v1/InterviewerView/Interviewer/api/interviewers`,
  rejectInterviewerEntryForm: `${base}/api/v1/InterviewerView/Interviewer/api/interviewers`,
  requestInterviewerEntryFormData: `${base}/api/v1/InterviewerView/Interviewer/api/interviewers/request`,

  userProfile: `${base}/api/users`,
};
