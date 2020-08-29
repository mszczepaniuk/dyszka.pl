"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Config = /** @class */ (function () {
    function Config() {
    }
    Config.baseAppUrl = "https://localhost:5001/";
    Config.identityServerUrl = "https://localhost:5000/";
    Config.localStorageAccessTokenKey = "accessToken";
    Config.localStorageRefreshTokenKey = "refreshToken";
    Config.clientScopes = "web.all offline_access";
    Config.clientId = "client";
    Config.clientSecret = "secret";
    return Config;
}());
exports.Config = Config;
//# sourceMappingURL=config.js.map