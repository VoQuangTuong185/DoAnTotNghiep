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
    }
}
