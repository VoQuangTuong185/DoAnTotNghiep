import { UserRolesDTO } from "./UserRolesDTO.model";

export class AdminUserDTO {
    public id!: number;
    public name!: string;
    public telNum!: string;
    public email!: string;
    public loginName!: string;
    public password!: string;
    public IsActive!: boolean;
    public address!: string;
    public userAPIs!: UserRolesDTO[];
}