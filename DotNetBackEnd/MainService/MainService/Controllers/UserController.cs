using DoAnTotNghiep.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;

namespace WebAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _IUserService;
        private ILog _ILog;
        public UserController(IUserService IUserService)
        {
            _IUserService = IUserService;
            _ILog = Log.GetInstance;
        }
        [HttpPost("check-existed-loginame-telnum-email")]
        public async Task<ApiResult> CheckExistedAndSendConfirmMail(RegisterUserOldDTO user)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.CheckExistedAndSendConfirmMail(user)).Match(res =>
                {
                    result.Message = "Kiểm tra thông tin thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("send-confirm-code-register")]
        public async Task<ApiResult> SendConfirmCodeRegister(RegisterUserOldDTO user)
        {
            var result = new ApiResult();
            try
            {
                result.Data = _IUserService.SendConfirmCodeRegister(user);
                result.Message = "Mã xác nhận đăng ký đã được gửi đi!";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("register-user")]
        public async Task<ApiResult> RegisterUser(RegisterUserOldDTO user)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.RegisterUser(user);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
		[Authorize]
		[HttpGet("get-user-roles")]
        public async Task<ApiResult> GetUserRoles()
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetUserRoles();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("send-forget-code")]
        public async Task<ApiResult> ForgetPassword(string email)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.ForgetPassword(email);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("change-password")]
        public async Task<ApiResult> ChangePassword(LoginUserDTO user)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.ChangePassword(user);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
		[Authorize]
		[HttpGet("get-info-user")]
        public async Task<ApiResult> GetInfoUser(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetInfoUser(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
		[Authorize]
		[HttpGet("check-existed-and-send-confirm-change-email")]
        public async Task<ApiResult> CheckExistedAndSendConfirmChangeEmail(string email)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.CheckExistedAndSendConfirmChangeEmail(email);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
		[Authorize]
		[HttpPost("update-profile")]
        public async Task<ApiResult> UpdateProfile(UserProfile user)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.UpdateProfile(user)).Match(res =>
                {
                    result.Message = "Chỉnh sửa thông tin cá nhân thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpPost("add-cart")]
        public async Task<ApiResult> AddCart(AddCart addCart)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.AddCart(addCart)).Match(res =>
                {
                    result.Message = "Thêm sản phẩm vào giỏ hàng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });             
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-cart-by-user-id")]
        public async Task<ApiResult> GetCartByUserID(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetCartByUserID(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpPost("inactive-cart")]
        public async Task<ApiResult> InActiveCart(CartDTO cart)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.InActiveCart(cart)).Match(res =>
                {
                    result.Message = "Xoá sản phẩm ra khỏi giỏ hàng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpPost("update-cart")]
        public async Task<ApiResult> UpdateCart(UpdateCart cart)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.UpdateCart(cart)).Match(res =>
                {
                    result.Message = "Chỉnh sửa số lượng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpPost("create-order")]
        public async Task<ApiResult> CreateOrder(CreateOrder order)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.CreateOrder(order)).Match(res =>
                {
                    result.Message = "Tạo đơn hàng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-waiting-order-by-user-id")]
        public async Task<ApiResult> GetWaitingOrderByUserID(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetWaitingOrderByUserID(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-processing-order-by-user-id")]
        public async Task<ApiResult> GetProcessingOrderByUserID(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetProcessingOrderByUserID(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-success-order-by-user-id")]
        public async Task<ApiResult> GetSuccessOrderByUserID(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetSuccessOrderByUserID(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-cancel-order-by-user-id")]
        public async Task<ApiResult> GetCancelOrderByUserID(int userId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetCancelOrderByUserID(userId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("delete-all-cart-after-order-by-user-id")]
        public async Task<ApiResult> DeleteAllCartAfterOrder(int userId)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.DeleteAllCartAfterOrder(userId)).Match(res =>
                {
                    result.Message = "Tạo đơn hàng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-products-by-category-id")]
        public async Task<ApiResult> GetProductsByCategoryID(int categoryId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetProductsByCategoryID(categoryId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("get-all-product-by-oder-id")]
        public async Task<ApiResult> GetAllProductByOrderID(int orderId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetAllProductByOrderID(orderId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("cancel-order")]
        public async Task<ApiResult> CancelOrder(int orderId)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.CancelOrder(orderId)).Match(res =>
                {
                    result.Message = "Huỷ đơn hàng thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpGet("success-order")]
        public async Task<ApiResult> SuccessOrder(int orderId)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.SuccessOrder(orderId)).Match(res =>
                {
                    result.Message = "Hoàn tất đơn hành thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("search-product")]
        public async Task<ApiResult> SearchProduct(string keyWord)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.SearchProduct(keyWord);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [Authorize]
        [HttpPost("create-feedback")]
        public async Task<ApiResult> CreateFeedback(List<FeedbackDTO> feedbacks)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.CreateFeedback(feedbacks)).Match(res =>
                {
                    result.Message = "Đánh giá thành công!";
                    result.Data = res;
                    result.IsSuccess = true;
                }, ex =>
                {
                    result.HttpStatusCode = 500;
                    result.Message = ex;
                    result.IsSuccess = false;
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-existed-product")]
        public async Task<ApiResult> GetExistedProduct(int productId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetExistedProduct(productId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-all-product")]
        public async Task<ApiResult> GetAllProduct()
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetAllProduct();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-month-best-seller-products")]
        public async Task<ApiResult> GetMonthBestSellerProducts()
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetMonthBestSellerProducts();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-all-category")]
        public async Task<ApiResult> GetAllCategory()
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetAllCategory();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-feedback-by-productId")]
        public async Task<ApiResult> GetFeedbackByProductId(int productId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetFeedbackByProductId(productId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpGet("get-all-vip")]
        public async Task<ApiResult> GetAllVIP()
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.GetAllVIP();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
    }
}
