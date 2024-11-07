// src/oktaConfig.js
const oktaConfig = {
  clientId: "0oajszoshjcCZIqkn5d7",
  issuer: "https://dev-93378940.okta.com/oauth2/ausjszru5jA7nhqRh5d7",
  redirectUri: window.location.origin + "/callback",
  scopes: ["openid", "profile", "email"],
  pkce: true,
};

export default oktaConfig;
