import { Config } from "../../config";
import { User } from "../user.model";

export class UserBuilder {
  private user: User;

  constructor(user?: User) {
    this.user = user || new User();
  }

  public addDataFromToken(token: any): UserBuilder {
    this.user.id = token["sub"];
    this.user.userName = token["userName"];
    this.user.roles = token[Config.roleClaimType];
    this.user.isBanned = token["isBanned"] === "True";
    return this;
  }

  public addApplicationData(data: any): UserBuilder {
    this.user.description = data["description"];
    return this;
  }

  public build(): User {
    return this.user;
  }
}
