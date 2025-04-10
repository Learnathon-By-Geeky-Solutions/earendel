// const base = 'https://localhost:7100';

const base = 'http://173.249.54.173';

export const endpoint = {
  googleLoginUrl: `${base}/api/users/google-login`,
  githubLoginUrl: `${base}/api/users/github-login`,
  userRegistrationUrl: `${base}/api/users/self-register`,
  userLoginUrl: `${base}/api/token`,
  hrDetailsUrl: `${base}/api/users/hr`,
  createavailabilityUrl: `${base}/api/v1/evaluator/intervieweravailabilities/search`,
};
