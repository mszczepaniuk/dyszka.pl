export class Payment {
  public id: string;
  public offerId: string;
  public orderId: string;
  public receiverUsername: string;
  public createdDate: Date;

  constructor(data: {
    id?: string,
    offerId?: string,
    orderId?: string,
    receiverUsername?: string,
    createdDate?: Date,
  }) {
    Object.assign(this, data);
  }
}
