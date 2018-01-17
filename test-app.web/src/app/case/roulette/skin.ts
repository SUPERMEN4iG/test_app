export class Skin {
  id: number;
  marketHashName: string;
  image: string;

  constructor(id: number, obj: any) {
    this.id = id;
    this.marketHashName = obj.marketHashName;
    this.image = obj.image;
  }
}
