using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Repositories;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILog _ILog;
        public AuthController(IConfiguration configuration, IUserService userService, IUnitOfWork unitOfWork, ILogger<AuthController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _ILog = Log.GetInstance;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("check-role-for-login")]
        public async Task<ApiResult> GetRoleForLogin(string loginName)
        {            
            var result = new ApiResult();
            try
            {
                result.Data = await _userService.GetRoleForLogin(loginName);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("check-valid-token")]
        public async Task<ApiResult> RefreshToken([FromBody] string LoginName)
        {

            var result = new ApiResult();
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var loginUser = await _unitOfWork.Repository<User>().Get(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == LoginName.ToUpper().TrimStart().TrimEnd() && x.IsActive)
                                                                    .Include(x => x.UserAPIs)
                                                                    .ToListAsync();
                _ILog.LogException(refreshToken);
                _ILog.LogException(loginUser.FirstOrDefault().RefreshToken);
                if (loginUser.FirstOrDefault().RefreshToken.Equals(refreshToken))
                {
                    if (loginUser.FirstOrDefault().TokenExpires < DateTime.Now)
                    {
                        var listRole = loginUser.FirstOrDefault().UserAPIs;
                        string role = "User";
                        listRole.ForEach(x =>
                        {
                            if (x.RoleId == 2 && x.IsActive)
                            {
                                role = "Admin";
                            }
                        });                       
                        var newToken = GenerateRefreshToken();
                        string token = CreateToken(loginUser.FirstOrDefault(), role, newToken);                      
                        SetRefreshToken(newToken);
                        result.Data = token;
                        result.IsSuccess = true;
                    }
                    else {
                        var accessToken = HttpContext.GetTokenAsync("access_token");
                        result.Data = accessToken.Result;  
                        result.IsSuccess = true;
                    }
                }
                else
                {
                    result.Data = "Refresh Token không hợp lệ";
                    result.IsSuccess = false;
                    result.HttpStatusCode = 401;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("register")]
        public async Task<ApiResult> Register(RegisterUserOldDTO request)
        {
            var result = new ApiResult();
            try
            {
                result.Data = await _userService.RegisterUser(request);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("login")]
        public async Task<ApiResult> Login(UserDTO_login request)
        {
            var result = new ApiResult();
            try
            {
                var existedUser = _unitOfWork.Repository<User>().Any(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == request.LoginUser.ToUpper().TrimStart().TrimEnd() && x.IsActive);
                var existedInActiveUser = _unitOfWork.Repository<User>().Any(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == request.LoginUser.ToUpper().TrimStart().TrimEnd() && !x.IsActive);
                if (!existedUser)
                {
                    if (existedInActiveUser)
                        result.Message = "Tài khoản của bạn đã bị khoá. Hãy liên hệ quản trị viên của web để được hỗ trợ.";
                    else
                        result.Message = "Thông tin đăng nhập không hợp lệ.";

                    result.IsSuccess = false;
                    result.HttpStatusCode = 400;
                }
                else
                {
                    var loginUser = await _unitOfWork.Repository<User>().Get(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == request.LoginUser.ToUpper().TrimStart().TrimEnd() && x.IsActive)
                                                                        .Include(x => x.UserAPIs)
                                                                        .ToListAsync();
                    if (!VerifyPasswordHash(request.Password, loginUser.FirstOrDefault().PasswordHash, loginUser.FirstOrDefault().PasswordSalt))
                    {
                        result.IsSuccess = false;
                        result.HttpStatusCode = 400;
                        result.Message = "Thông tin đăng nhập không hợp lệ.";
                    }
                    else
                    {
                        var listRole = loginUser.FirstOrDefault().UserAPIs;
                        string role ="User";
                        listRole.ForEach(x =>
                        {
                            if (x.RoleId == 2 && x.IsActive)
                            {
                                role = "Admin";
                            }
                        });
                        if (role == "Admin")
                        {
                            result.Message = "Tài khoản của bạn không phải là người dùng, hãy chọn lại";
                            result.IsSuccess = false;
                        }
                        else
                        {
                            var refreshToken = GenerateRefreshToken();
                            string token = CreateToken(loginUser.FirstOrDefault(), role, refreshToken);
                            SetRefreshToken(refreshToken);
                            result.Data = token;
                            result.IsSuccess = true;
                            result.Message = "Đăng nhập thành công, đang chuyển hướng đến trang chủ";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user, string role, RefreshToken refreshToken)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("loginName", user.LoginName),
                new Claim("name", user.Name),
                new Claim("role", role),
                new Claim("expires", DateTime.Now.AddMinutes(30).ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var loginUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == user.LoginName.ToUpper().TrimStart().TrimEnd() && x.IsActive);
            loginUser.RefreshToken = refreshToken.Token;
            loginUser.TokenCreated = DateTime.Now;
            loginUser.TokenExpires = DateTime.Now.AddMinutes(30);
            _unitOfWork.Repository<User>().Update(loginUser);
            _unitOfWork.SaveChanges();
            return jwt;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(30),
                Created = DateTime.Now
            };
            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                IsEssential = true,
                Path = "/",
                Secure= true,
                SameSite = SameSiteMode.None,
            };
            HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}
