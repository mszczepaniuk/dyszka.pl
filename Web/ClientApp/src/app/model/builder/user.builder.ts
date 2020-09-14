import { Config } from '../../config';
import { User } from '../user.model';

export class UserBuilder {
  private user: User;

  constructor(user?: User) {
    this.user = user || new User();
  }

  public addIdentityData(token: any): UserBuilder {
    this.user.identityId = token['sub'];
    this.user.userName = this.user.userName || token['userName'];
    this.user.roles = token[Config.roleClaimType] || [];
    if (!Array.isArray(this.user.roles)) {
      this.user.roles = [this.user.roles as any];
    }
    this.user.isBanned = token['isBanned'].toString().toLowerCase() === 'true';
    return this;
  }

  public addApplicationData(data: any): UserBuilder {
    this.user.description = data['description'];
    this.user.applicationId = data['id'];
    this.user.userName = this.user.userName || data['userName'];
    this.user.telephoneNumber = data['telephoneNumber'];
    this.user.profileImage = data['profileImage'];
    this.user.email = data['email'];
    return this;
  }

  public build(): User {
    return this.user;
  }
}
