export class Comment {
  public id: string;
  public text: string;
  public isPositive: boolean;
  public createdDate: Date;
  public authorUserName: string;

  constructor(data: {
    id?: string,
    text?: string,
    isPositive?: boolean,
    createdDate?: Date,
    authorUserName?: string,
  }) {
    Object.assign(this, data);
  }
}
