import { CoreConstants } from './core/src/lib/core.constant';
export class RegisterConstant{
    public static apiUrl = () => `${CoreConstants.apiUrl()}`
    public static libraryApiUrlUser = () => `${RegisterConstant.apiUrl()}/api/User/`
    public static libraryApiUrlAdmin = () => `${RegisterConstant.apiUrl()}/api/Admin/`
    public static libraryApiUrlAuth = () => `${RegisterConstant.apiUrl()}/api/Auth/`

    public static apiCategoryURL = () => `${CoreConstants.apiCategoryURL()}`
    public static libraryApiUrlCategory = () => `${RegisterConstant.apiCategoryURL()}/api/Category/`
}