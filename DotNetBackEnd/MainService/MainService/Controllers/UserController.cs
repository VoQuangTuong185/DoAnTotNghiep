using DoAnTotNghiep.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public readonly IAdminService _IAdminService;
        private ILog _ILog;
        public UserController(IUserService IUserService, IAdminService iAdminService)
        {
            _IUserService = IUserService;
            _IAdminService = iAdminService;
            //Get the Singleton Log Instance
            _ILog = Log.GetInstance;
        }
        [HttpPost("check-existed-and-send-confirm-mail")]
        public async Task<ApiResult> CheckExistedAndSendConfirmMail(RegisterUserOldDTO user)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _IUserService.CheckExistedAndSendConfirmMail(user);
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
                    result.Message = "Thao tác thành công!";
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
        [HttpGet("confirm-order")]
        public async Task<ApiResult> ConfirmOrder(int orderId)
        {
            var result = new ApiResult();
            try
            {
                (await _IUserService.ConfirmOrder(orderId)).Match(res =>
                {
                    result.Message = "Xác nhận đơn hàng thành công!";
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
    }
}
