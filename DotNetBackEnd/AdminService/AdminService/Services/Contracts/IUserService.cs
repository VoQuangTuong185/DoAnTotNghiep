using DoAnTotNghiep.DTO;
using Optional;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;

namespace WebAppAPI.Services.Contracts
{
    public interface IUserService
    {
        string GetMyName();
        Task<bool> RegisterUser(RegisterUserOldDTO user);
        Task<string> CheckExistedAndSendConfirmMail(RegisterUserOldDTO user);
        Task<IEnumerable<UserAPI>> GetUserRoles();
        Task<string> ForgetPassword(string email);
        Task<bool> ChangePassword(LoginUserDTO user);
        Task<UserProfile> GetInfoUser(int userId);
        Task<string> CheckExistedAndSendConfirmChangeEmail(string email);
        Task<Option<bool, string>> UpdateProfile(UserProfile user);
        Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId);
        Task<IEnumerable<OrderDetailDTO>> GetAllProductByOrderID(int orderId);
        Task<List<SearchProduct>> SearchProduct(string keyWord);
    }
}
