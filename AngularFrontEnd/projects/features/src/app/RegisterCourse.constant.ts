import { CoreConstants } from './core/src/lib/core.constant';
export class RegisterConstant{
    public static apiUrl = () => `${CoreConstants.apiUrl()}`
    public static libraryApiUrlUser = () => `${RegisterConstant.apiUrl()}/api/User/`
    public static libraryApiUrlAdmin = () => `${RegisterConstant.apiAdminURL()}/api/Admin/`
    public static libraryApiUrlAuth = () => `${RegisterConstant.apiUrl()}/api/Auth/`

    public static apiAdminURL = () => `${CoreConstants.apiAdminURL()}`
    public static libraryAdminApiUrlAuth = () => `${RegisterConstant.apiAdminURL()}/api/Auth/`
    public static libraryApiUrlCategory = () => `${RegisterConstant.apiAdminURL()}/api/Category/`
}