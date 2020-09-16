export class AuditLog {
  public message: string;
  public createdDate: Date;
  public authorUsername: string;
  public affectedEntityId: string;

  constructor(data: {
    message?: string,
    createdDate?: Date,
    authorUsername?: string;
    affectedEntityId?: string
  }) {
    Object.assign(this, data);
  }
}
