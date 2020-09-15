export class Offer {
  public id: string;
  public authorUserName: string;
  public createdDate: Date;
  public image: string;
  public title: string;
  public tags: string[];
  public description: string;
  public shortDescription: string;
  public price: number;
  public isBlocked: boolean;
  public isHidden: boolean;

  constructor(data: {
    id?: string,
    authorUserName?: string,
    createdDate?: Date,
    image?: string,
    title?: string,
    tags?: string[],
    desciption?: string,
    shortDescription?: string,
    price?: number,
    isBlocked?: boolean,
    isHidden?: boolean,
  }) {
    Object.assign(this, data);
  }
}
