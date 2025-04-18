const base = 'https://talentmesh.genericsolution.net/';
const quizBase = `${base}/api/v1/JobView/candidateview/quiz-attempts`;

export const endpoint = {
  googleLoginUrl: `${base}/api/users/google-login`,
  githubLoginUrl: `${base}/api/users/github-login`,
  userRegistrationUrl: `${base}/api/users/self-register`,
  userLoginUrl: `${base}/api/token`,
  hrDetailsUrl: `${base}/api/users/hr`,
  interviewerAvailabilityCreateUrl: `${base}/api/v1/evaluator/intervieweravailabilities`,
  interviewerAvailabilitySearchUrl: `${base}/api/v1/evaluator/intervieweravailabilities/search`,
  jobViewUrl: `${base}/api/v1/JobView/candidateview/JobView`,
  // Quiz related endpoints
  quizStartUrl: `${quizBase}/start`,
  quizQuestionUrl: quizBase,
  quizSubmitUrl: quizBase,
  quizAttemptsUrl: `${base}/api/v1/JobView/candidateview/users`,
};
