export class RegisterUserDTO{
    public id:number = 0;
    public name: string = '';
    public loginName: string = '';
    public email: string = '';
    public telNum: string = '';
    public password: string = '';
    public confirmPassword: string = '';
    public roles :number = 0;
    public IsActive :boolean = true;
    public provinces: string='';
    public districts: string='';
    public wards: string='';
    public provinceCode!: number;
    public districtCode!: number;
    public wardCode!: number;
    public address: string='';
}