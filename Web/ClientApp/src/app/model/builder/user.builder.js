"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var config_1 = require("../../config");
var user_model_1 = require("../user.model");
var UserBuilder = /** @class */ (function () {
    function UserBuilder(user) {
        this.user = user || new user_model_1.User();
    }
    UserBuilder.prototype.addIdentityData = function (token) {
        this.user.identityId = token['sub'];
        this.user.userName = token['userName'];
        this.user.roles = token[config_1.Config.roleClaimType];
        this.user.isBanned = token['isBanned'].toLowerCase() === 'true';
        return this;
    };
    UserBuilder.prototype.addApplicationData = function (data) {
        this.user.description = data['description'];
        this.user.applicationId = data['id'];
        this.user.telephoneNumber = data['telephoneNumber'];
        this.user.profileImage = data['profileImage'];
        this.user.email = data['email'];
        return this;
    };
    UserBuilder.prototype.build = function () {
        return this.user;
    };
    return UserBuilder;
}());
exports.UserBuilder = UserBuilder;
//# sourceMappingURL=user.builder.js.map