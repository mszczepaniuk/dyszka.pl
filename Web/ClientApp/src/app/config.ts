export class Config {
  static baseAppUrl = "https://localhost:5001/";
  static identityServerUrl = "https://localhost:5000/";
  static localStorageAccessTokenKey = "accessToken";
  static localStorageRefreshTokenKey = "refreshToken"
  static clientScopes = "web.all offline_access";
  static clientId = "client";
  static clientSecret = "secret";
  static roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
}
