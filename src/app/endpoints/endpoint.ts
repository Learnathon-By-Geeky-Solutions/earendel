const base = 'https://talentmesh.genericsolution.net/';

export const endpoint = {
  googleLoginUrl: `${base}/api/users/google-login`,
  githubLoginUrl: `${base}/api/users/github-login`,
  userRegistrationUrl: `${base}/api/users/self-register`,
  userLoginUrl: `${base}/api/token`,
  hrDetailsUrl: `${base}/api/users/hr`,
  interviewerAvailabilityCreateUrl: `${base}/api/v1/evaluator/intervieweravailabilities`,
  interviewerAvailabilitySearchUrl: `${base}/api/v1/evaluator/intervieweravailabilities/search`,
};
