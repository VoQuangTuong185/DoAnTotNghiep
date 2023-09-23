export class UpdateCart {
    public UserId!: number;
    public ProductId!: number;
    public Quantity!: number;
    constructor(UserId: number, ProductId: number, Quantity: number){
        this.UserId = UserId;
        this.ProductId = ProductId;
        this.Quantity = Quantity;
    }
}