export class Order {
  public id: string;
  public offerId: string;
  public offerAuthorUserName: string;
  public offerTitle: string;
  public createdDate: Date;
  public doneTime: Date;
  public authorUserName: string;

  constructor(data: {
    id?: string;
    offerId?: string,
    offerAuthorUserName?: string,
    offerTitle?: string,
    createdDate?: Date,
    doneTime?: Date,
    authorUserName?: string,
  }) {
    Object.assign(this, data);
  }
}
