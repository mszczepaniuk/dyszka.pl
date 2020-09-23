export class Payment {
  public id: string;
  public offerId: string;
  public orderId: string;
  public receiverUsername: string;
  public createdDate: Date;
  public value: number;

  constructor(data: {
    id?: string,
    offerId?: string,
    orderId?: string,
    receiverUsername?: string,
    createdDate?: Date,
    value: number,
  }) {
    Object.assign(this, data);
  }
}
