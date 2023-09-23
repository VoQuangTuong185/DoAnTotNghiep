
import { CategoryDTO } from "./CategoryDTO.model";

export class ProductDTO{
    public Id!:number;
    public ProductName: string = '';
    public Description: string = '';
    public Image: string = '';
    public Price!: number;
    public Discount!: number;
    public Quanity!: number;
    public SoldQuantity!: number;
    public CategoryId!: number;
    public BrandName?: string;
    public BrandId?: number;
    public CreatedDate!: Date;
    public UpdatedDate!:Date;
    public category!:CategoryDTO;
    public IsActive: boolean = true;
}