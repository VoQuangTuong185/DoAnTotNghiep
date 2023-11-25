import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constant } from './WebsiteApi.constant';
import { ProductDTO } from './ProductDTO.model';
import { LoginUserDTO } from './LoginUserDTO.model';
import { RegisterUserDTO } from './RegisterUserDTO.model';
import { AdminUserDTO } from './AdminUserDTO';
import { UserProfile } from './UserProfile.model';
import { CategoryDTO } from './CategoryDTO.model';
import { AddCart } from './AddCart.model';
import { CartDTO } from './CartDTO.model';
import { CreateOrder } from './CreateOrder.model';
import { UpdateCart } from './UpdateCart.model';
import { FeedbackDTO } from './FeedbackDTO.model.js';
import { FeedbackDetailShow } from './FeedbackDetailShow.model';
@Injectable({
  providedIn: 'root',
})
export class WebsiteAPIService{
    private httpHeaders: HttpHeaders;
    private urlLoginUser = 'login-user';
    private urlCheckExisted = 'check-existed-loginame-telnum-email';
    private urlSendConfirmCodeRegister = 'send-confirm-code-register';
    private urlCheckExistedAndSendConfirmChangeMail = 'check-existed-and-send-confirm-change-email?email=';
    private urlRegisterUser = 'register-user';
    private urlGetUsers = 'get-users';
    private urlSendForgetCode = 'send-forget-code?email=';
    private urlChangePassword = 'change-password';
    private urlUpdateProfile = 'update-profile';
    private urlGetInfoUser = 'get-info-user?userId=';    
    private urlgetAllProduct = 'get-all-product';
    private urlgetMonthBestSellerProducts = 'get-month-best-seller-products';
    private urlgetExistedProduct = 'get-existed-product?productId=';
    private urlGetProductsByCategoryID = 'get-products-by-category-id?categoryId=';
    private urlAddCard = 'add-cart';
    private urlGetCartByUserID = 'get-cart-by-user-id?userId=';
    private urlGetWaitingOrderByUserID = 'get-waiting-order-by-user-id?userId=';
    private urlGetProcessingOrderByUserID = 'get-processing-order-by-user-id?userId=';
    private urlGetSuccessOrderByUserID = 'get-success-order-by-user-id?userId=';
    private urlGetCancelOrderByUserID = 'get-cancel-order-by-user-id?userId=';
    private urlDeleteAllCartAfterOrder = 'delete-all-cart-after-order-by-user-id?userId=';
    private urlInActiveCart = 'inactive-cart';
    private urlUpdateCart = 'update-cart';
    private urlCreateOrder = 'create-order';
    private urlCancelOrder = 'cancel-order?orderId=';
    private urlConfirmOrder = 'confirm-order?orderId=';
    private urlSuccessOrder = 'success-order?orderId=';
    private urlSearchProduct = 'search-product?keyWord=';
    private urlCreateFeedback = 'create-feedback';
    private urlgetAllCategoryUser = 'get-all-category?'; 
    private urlgetFeedbackByProductId = 'get-feedback-by-productId?productId=';
    private urlgetAllVIP = 'get-all-vip?';

    //admin service
    private urlCreateBrand = 'create-brand'; 
    private urlUpdateBrand = 'update-brand';   
    private urlGetAllBrand = 'get-all-brand?type=';
    private urlGetExistedBrand = 'get-existed-brand?brandId=';
    private urlinActiveBrand = 'inactive-brand?brandId=';
    private urlAutoGeneratedProductID = 'auto-generated-product-id'; 
    private urlinActiveProduct = 'inactive-product?productId=';
    private urlSetManagerPermisson = 'set-manager-permisson?userId='; 
    private urlCreateProduct = 'create-product'; 
    private urlUpdateProduct = 'update-product';   
    private urlEditUser = 'edit-user';
    private urlInActiveUser = 'active-or-inactive-user?loginName=';
    private urlgetAllCategoryAdmin = 'get-all-category?'; 
    private urlinActiveCategory = 'inactive-category?categoryId=';
    private urlgetExistedCategory = 'get-existed-category?categoryId=';
    private urlCreateCategory = 'create-category';
    private urlUpdateCategory = 'update-category';  
    private urlGetWaitingOrder = 'get-waiting-order';
    private urlGetProcessingOrder = 'get-processing-order';
    private urlGetSuccessOrder = 'get-success-order';
    private urlGetCancelOrder = 'get-cancel-order';
    private urlGetAllProductByOrderID = 'get-all-product-by-oder-id?orderId=';
    private urlReplyFeedback = 'reply-feedback'; 

    constructor(private http: HttpClient){
        this.httpHeaders = new HttpHeaders({
            "ngrok-skip-browser-warning":"any",
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'POST, PUT, GET, OPTIONS',
            'Access-Control-Allow-Headers': 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With'
        });
    }
    loginUser(user: LoginUserDTO){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlLoginUser, user);
    }
    checkExistedLoginNameTelNumEmail(user: RegisterUserDTO){
        let apiUrl = localStorage.getItem('userRole') == 'admin' ?  Constant.libraryApiUrlAdmin() : Constant.libraryApiUrlUser();
        return this.http.post<any>(apiUrl + this.urlCheckExisted, user);
    }
    sendConfirmCodeRegister(user: RegisterUserDTO){
        let apiUrl = localStorage.getItem('userRole') == 'admin' ?  Constant.libraryApiUrlAdmin() : Constant.libraryApiUrlUser();
        return this.http.post<any>(apiUrl + this.urlSendConfirmCodeRegister, user);
    }
    checkExistedAndSendChangeConfirmMail(email: string){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlCheckExistedAndSendConfirmChangeMail + email, {headers: this.httpHeaders});
    }
    registerUser(user: RegisterUserDTO){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlRegisterUser, user);
    }
    sendForgetCode(email: any){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlSendForgetCode + email, {headers: this.httpHeaders});
    }
    changePassword(user: LoginUserDTO){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlChangePassword, user);
    }
    updateProfile(user: UserProfile){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlUpdateProfile, user);
    }
    getInfoUser(userId : number){
        let apiUrl = localStorage.getItem('userRole') == 'admin' ?  Constant.libraryAdminApiUrlAuth() : Constant.libraryApiUrlUser();
        return this.http.get<any>(apiUrl + this.urlGetInfoUser + userId, {headers: this.httpHeaders});
    }
    getAllProduct(){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetAllProduct, {headers: this.httpHeaders});
    }
    getMonthBestSellerProducts(){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetMonthBestSellerProducts, {headers: this.httpHeaders});
    }
    getExistedProductUser(productId: number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetExistedProduct + productId, {headers: this.httpHeaders});
    }
    addCart(product: AddCart){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlAddCard, product);
    }
    getCartByUserID(userId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetCartByUserID + userId, {headers: this.httpHeaders});
    }
    inActiveCart(cart: CartDTO){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlInActiveCart, cart);
    }
    updateCart(cart: UpdateCart){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlUpdateCart, cart);
    }
    createOrder(order: CreateOrder){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlCreateOrder, order);
    }
    getWaitingOrderByUserID(userId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetWaitingOrderByUserID + userId, {headers: this.httpHeaders});
    }
    getProcessingOrderByUserID(userId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetProcessingOrderByUserID + userId, {headers: this.httpHeaders});
    }
    getSuccessOrderByUserID(userId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetSuccessOrderByUserID + userId, {headers: this.httpHeaders});
    }
    getCancelOrderByUserID(userId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetCancelOrderByUserID + userId, {headers: this.httpHeaders});
    }
    deleteAllCartAfterOrder(userId:number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlDeleteAllCartAfterOrder + userId, {headers: this.httpHeaders});
    }
    getProductsByCategoryIDUser(categoryId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetProductsByCategoryID + categoryId, {headers: this.httpHeaders});
    }
    getAllProductByOrderIDUser(orderId :number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlGetAllProductByOrderID + orderId, {headers: this.httpHeaders});
    }
    cancelOrderUser(orderId: number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlCancelOrder + orderId, {headers: this.httpHeaders});
    }
    successOrderUser(orderId: number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlSuccessOrder + orderId, {headers: this.httpHeaders});
    }
    getSearchProductUser(keyWord: string){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlSearchProduct + keyWord, {headers: this.httpHeaders});
    }
    createFeedback(feedback: FeedbackDTO[]){
        return this.http.post<any>(Constant.libraryApiUrlUser() + this.urlCreateFeedback, feedback);
    }
    getAllCategoryUser(){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetAllCategoryUser, {headers: this.httpHeaders});
    }
    getFeedbackByProductIdUser(productId: number){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetFeedbackByProductId + productId, {headers: this.httpHeaders});
    }
    getAllVIP(){
        return this.http.get<any>(Constant.libraryApiUrlUser() + this.urlgetAllVIP, {headers: this.httpHeaders});
    }

    //admin Service 
    getUsers(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetUsers,{ headers: this.httpHeaders});
    }
    getAllCategoryAdmin(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlgetAllCategoryAdmin, {headers: this.httpHeaders});
    }
    inActiveCategory(categoryId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlinActiveCategory + categoryId, {headers: this.httpHeaders});
    }
    getExistedCategory(categoryId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlgetExistedCategory + categoryId, {headers: this.httpHeaders});
    }
    createCategory(category: CategoryDTO){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlCreateCategory, category);
    }
    updateCategory(category: ProductDTO){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlUpdateCategory, category);
    }
    getWaitingOrder(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetWaitingOrder, {headers: this.httpHeaders});
    }
    getProcessingOrder(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetProcessingOrder, {headers: this.httpHeaders});
    }
    getSuccessOrder(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetSuccessOrder, {headers: this.httpHeaders});
    }
    getCancelOrder(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetCancelOrder, {headers: this.httpHeaders});
    }
    createBrand(brand: any){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlCreateBrand, brand);
    }
    updateBrand(brand: any){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlUpdateBrand, brand);
    }
    getAllBrand(type:string){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetAllBrand + type, {headers: this.httpHeaders});
    }
    getExistedBrand(brandId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetExistedBrand + brandId, {headers: this.httpHeaders});
    }
    autoGeneratedProductID(){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlAutoGeneratedProductID,{headers: this.httpHeaders}); 
    }
    inActiveBrand(brandId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlinActiveBrand + brandId, {headers: this.httpHeaders});
    }
    setManagerPermisson(userId : number){ 
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlSetManagerPermisson + userId, {headers: this.httpHeaders});
    }
    createProduct(product: ProductDTO){
        delete product.BrandName;
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlCreateProduct, product);
    }
    updateProduct(product: ProductDTO){
        delete product.BrandName;
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlUpdateProduct, product);
    }
    inActiveProduct(productId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlinActiveProduct + productId, {headers: this.httpHeaders});
    }
    editUser(user: AdminUserDTO){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlEditUser, user);
    }
    activeOrInActiveUser(loginName : string){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlInActiveUser + loginName, {headers: this.httpHeaders});
    }
    getExistedProductAdmin(productId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlgetExistedProduct + productId, {headers: this.httpHeaders});
    }
    getProductsByCategoryID(categoryId :number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetProductsByCategoryID + categoryId, {headers: this.httpHeaders});
    }
    getAllProductByOrderIDAdmin(orderId :number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlGetAllProductByOrderID + orderId, {headers: this.httpHeaders});
    }
    getSearchProductAdmin(keyWord: string){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlSearchProduct + keyWord, {headers: this.httpHeaders});
    }
    cancelOrderAdmin(orderId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlCancelOrder + orderId, {headers: this.httpHeaders});
    }
    confirmOrderAdmin(orderId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlConfirmOrder + orderId, {headers: this.httpHeaders});
    }
    successOrderAdmin(orderId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlSuccessOrder + orderId, {headers: this.httpHeaders});
    }
    getFeedbackByProductIdAdmin(productId: number){
        return this.http.get<any>(Constant.libraryApiUrlAdmin() + this.urlgetFeedbackByProductId + productId, {headers: this.httpHeaders});
    }
    replyFeedback(feedback: FeedbackDetailShow){
        return this.http.post<any>(Constant.libraryApiUrlAdmin() + this.urlReplyFeedback, feedback);
    }
}
