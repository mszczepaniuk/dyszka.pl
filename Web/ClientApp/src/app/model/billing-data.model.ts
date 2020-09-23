export class BillingData {
  public name: string;
  public bankAccountNumber: string;
  public street: string;
  public city: string;
  public postalCode: string;

  constructor(data: {
    name?: string;
    bankAccountNumber?: string;
    street?: string;
    city?: string;
    postalCode?: string;
  }) {
    Object.assign(this, data);
  }
}
