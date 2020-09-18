export class Message{
  public authorUserName: string;
  public receirverUserName: string;
  public createdDate: Date;
  public text: string;

  constructor(data: {
    authorUserName?: string,
    receiverUserName?: string,
    createdDate?: Date,
    text?: string
  }) {
    Object.assign(this, data);
  }
}
