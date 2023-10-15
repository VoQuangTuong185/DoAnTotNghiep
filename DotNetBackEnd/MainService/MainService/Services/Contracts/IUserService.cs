using DoAnTotNghiep.DTO;
using Optional;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;
using WebAppAPI.Services.Model;

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
        Task<Option<bool, string>> AddCart(AddCart addCart);
        Task<IEnumerable<CartDTO>> GetCartByUserID(int userId);
        Task<IEnumerable<OrderDTO>> GetWaitingOrderByUserID(int userId);
        Task<IEnumerable<OrderDTO>> GetProcessingOrderByUserID(int userId);
        Task<IEnumerable<OrderDTO>> GetSuccessOrderByUserID(int userId);
        Task<IEnumerable<OrderDTO>> GetCancelOrderByUserID(int userId);
        Task<Option<bool, string>> DeleteAllCartAfterOrder(int userId);
        Task<Option<bool, string>> InActiveCart(CartDTO cart);
        Task<Option<bool, string>> CreateOrder(CreateOrder order);
        Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId);
        Task<IEnumerable<OrderDetailDTO>> GetAllProductByOrderID(int orderId);
        Task<Option<bool, string>> CancelOrder(int orderId);
        Task<Option<bool, string>> ConfirmOrder(int orderId);
        Task<Option<bool, string>> SuccessOrder(int orderId);
        Task<Option<bool, string>> UpdateCart(UpdateCart cart);
        Task<List<SearchProduct>> SearchProduct(string keyWord);
        Task<Option<bool, string>> CreateFeedback(List<FeedbackDTO> feedback);
    }
}
