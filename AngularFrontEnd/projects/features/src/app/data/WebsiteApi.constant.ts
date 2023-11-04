import { CoreConstants } from '../core/src/lib/core.constant';
export class Constant{
    public static apiUrl = () => `${CoreConstants.apiUrl()}`
    public static libraryApiUrlUser = () => `${Constant.apiUrl()}/api/User/`
    public static libraryApiUrlAdmin = () => `${Constant.apiAdminURL()}/api/Admin/`
    public static libraryApiUrlAuth = () => `${Constant.apiUrl()}/api/Auth/`

    public static apiAdminURL = () => `${CoreConstants.apiAdminURL()}`
    public static libraryAdminApiUrlAuth = () => `${Constant.apiAdminURL()}/api/Auth/`
    public static libraryApiUrlCategory = () => `${Constant.apiAdminURL()}/api/Category/`
}