﻿using Optional;
using DoAnTotNghiep.DTO;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using DoAnTotNghiep.Models.Entities;
using AdminService.DTO;

namespace WebAppAPI.Services.Contracts
{
    public interface IAdminService
    {
        Task<string> ForgetPassword(string email);
        Task<bool> ChangePassword(LoginUserDTO user);
        Task<UserProfile> GetInfoUser(int userId);
        Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId);
        Task<IEnumerable<OrderDetailDTO>> GetAllProductByOrderID(int orderId);
        Task<List<SearchProduct>> SearchProduct(string keyWord);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<bool> ActiveOrInActiveUser(string loginName);
        Task<Option<bool, string>> EditUser(UserAdminDTO user);
        Task<bool> CheckExistedLoginName(string loginName);
        Task<Option<bool, string>> SetManagerPermisson(int userId);
        Task<int> AutoGeneratedProductID();
        Task<Option<bool, string>> CreateProduct(ProductDTOShow product);     
        Task<Option<bool, string>> UpdateProduct(ProductDTOUpdate product);                  
        Task<Option<bool, string>> InActiveProduct(int productId);
        Task<Option<bool, string>> InActiveBrand(int brandId);       
        Task<IEnumerable<Brand>> GetAllBrand(string type);
        Task<Brand> GetExistedBrand(int BrandId);
        Task<ProductDTOShow> GetExistedProduct(int ProductId);
        Task<Option<bool, string>> CreateBrand(CreateBrandDTO brand);
        Task<Option<bool, string>> UpdateBrand(BrandDTO brand);
        Task<IEnumerable<OrderDTO>> GetWaitingOrder();
        Task<IEnumerable<OrderDTO>> GetProcessingOrder();
        Task<IEnumerable<OrderDTO>> GetSuccessOrder();
        Task<IEnumerable<OrderDTO>> GetCancelOrder();
        Task<IEnumerable<Category>> GetAllCategory();
        Task<Option<bool, string>> CreateCategory(CategoryCreateDto category);
        Task<Option<bool, string>> UpdateCategory(Category category);
        Category GetCategoryById(int id);
        Task<Option<bool, string>> InactiveCategory(int categoryId);
        Task<Option<bool, string>> CancelOrder(int orderId);
        Task<Option<bool, string>> SuccessOrder(int orderId);
        Task<Option<bool, string>> ConfirmOrder(int orderId);
        Task<IEnumerable<FeedbackShowDetail>> GetFeedbackByProductId(int ProductId);
        Task<Option<bool, string>> ReplyFeedback(FeedbackShowDetail feedback);
    }
}
