using AutoMapper;
using CategoryService.AsyncDataServices;
using DoAnTotNghiep.DTO;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async.Extensions;
using System.Security.Claims;
using System.Security.Cryptography;
using DoAnTotNghiep.DTOM;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;
using WebAppAPI.Repositories;
using WebAppAPI.Services.Contracts;

namespace WebAppAPI.Services.Business
{
    public class _UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<_UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessageBusClient _messageBusClient;

        public _UserService(IUnitOfWork unitOfWork, IMessageBusClient messageBusClient, ILogger<_UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _messageBusClient = messageBusClient;
        }
        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }
        public async Task<string> CheckExistedAndSendConfirmMail(RegisterUserOldDTO user)
        {
            bool existedUser = _unitOfWork.Repository<User>().Any(x => (x.LoginName.ToUpper().TrimStart().TrimEnd() == user.LoginName.ToUpper().TrimStart().TrimEnd() 
                                                                        || x.Email.ToUpper().TrimStart().TrimEnd() == user.Email.ToUpper().TrimStart().TrimEnd()) && x.IsActive);
            string confirmCode = string.Empty;
            if (!existedUser)
            {
                confirmCode = Get8CharacterRandomString();
                var mailInformation = new MailPublishedDto("ConfirmRegister", user.Name, user.Email, "[VĂN PHÒNG PHẨM 2023] XÁC NHẬN ĐĂNG KÝ TÀI KHOẢN", "VĂN PHÒNG PHẨM 2023", confirmCode, "Mail_Published");
                _messageBusClient.PublishMail(mailInformation);
            }
            else
            {
                confirmCode = "existed";
            }
            return confirmCode;
        }
        public async Task<bool> RegisterUser(RegisterUserOldDTO user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var insertUser = new User();
            var userAPIs = new List<UserAPI>();
            userAPIs.Add(new UserAPI()
            {
                UserId = user.Id,
                RoleId = 1,
                user = null,
                role = null,
                IsActive= true,
            });
            insertUser.Name = user.Name;
            insertUser.LoginName = user.LoginName;
            insertUser.Email = user.Email;
            insertUser.TelNum = user.TelNum;
            insertUser.PasswordHash = passwordHash;
            insertUser.PasswordSalt = passwordSalt;
            insertUser.UserAPIs = userAPIs;
            insertUser.Address = user.Address + ", " + user.Wards + ", " + user.Districts + ", " + user.Provinces;
            insertUser.AddressCode = user.WardCode.ToString() + ", " + user.DistrictCode.ToString() + ", " + user.ProvinceCode.ToString();
            _unitOfWork.Repository<User>().Add(insertUser);
            return _unitOfWork.SaveChanges();
        }
        public async Task<string> ForgetPassword(string email)
        {
            var existedUser = _unitOfWork.Repository<User>().Any(x => x.Email.ToUpper().TrimStart().TrimEnd() == email.ToUpper().TrimStart().TrimEnd() && x.IsActive);
            string confirmCode = string.Empty;
            if (existedUser)
            {
                confirmCode = Get8CharacterRandomString();
                var mailInformation = new MailPublishedDto("ConfirmForgetPassword", string.Empty, email, "[VĂN PHÒNG PHẨM 2023] XÁC NHẬN QUÊN MẬT KHẨU", "VĂN PHÒNG PHẨM 2023", confirmCode, "Mail_Published");
                _messageBusClient.PublishMail(mailInformation);
            }
            return confirmCode;
        }
        public async Task<string> CheckExistedAndSendConfirmChangeEmail(string email)
        {
            var existedUser = _unitOfWork.Repository<User>().Any(x => x.Email.ToUpper().TrimStart().TrimEnd() == email.ToUpper().TrimStart().TrimEnd() && x.IsActive);
            string confirmCode = string.Empty;
            if (existedUser)
            {
                confirmCode = "existed";
            }
            else
            {
                confirmCode = Get8CharacterRandomString();
                var mailInformation = new MailPublishedDto("ConfirmChangeEmail", string.Empty, email, "[VĂN PHÒNG PHẨM 2023] XÁC NHẬN THAY ĐỔI MẬT KHẨU CÁ NHÂN", "VĂN PHÒNG PHẨM 2023", confirmCode, "Mail_Published");
                _messageBusClient.PublishMail(mailInformation);
            }
            return confirmCode;
        }
        public async Task<bool> ChangePassword(LoginUserDTO user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Email.ToUpper().TrimStart().TrimEnd() == user.Email.ToUpper().TrimStart().TrimEnd() && x.IsActive);
            existedUser.PasswordHash = passwordHash;
            existedUser.PasswordSalt = passwordSalt;
            _unitOfWork.Repository<User>().Update(existedUser);
            bool IsChangePassword = _unitOfWork.SaveChanges();
            if (IsChangePassword)
            {
                var mailInformation = new MailPublishedDto("ConfirmChangePassword", existedUser.Name, existedUser.Email, "[VĂN PHÒNG PHẨM 2023] XÁC NHẬN THAY ĐỔI MẬT KHẨU", "VĂN PHÒNG PHẨM 2023", string.Empty, "Mail_Published");
                _messageBusClient.PublishMail(mailInformation);
            }
            return IsChangePassword;
        }
        public async Task<IEnumerable<UserAPI>> GetUserRoles()
        {
            var result = Enumerable.Empty<UserAPI>();
            result = await _unitOfWork.Repository<UserAPI>().All().ToListAsync();
            return result;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<UserProfile> GetInfoUser(int userId)
        {
            var result = new UserProfile();
            var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == userId);
            result.Id = existedUser.Id;   
            result.Name = existedUser.Name;
            result.LoginName = existedUser.LoginName;
            result.Email = existedUser.Email;
            result.TelNum= existedUser.TelNum;
            string[] splitAdress = existedUser.Address.Split(", ");
            string[] splitAdressCode = existedUser.AddressCode.Split(", ");
            result.Streets = splitAdress[0];
            result.Wards = splitAdress[1];
            result.Districts = splitAdress[2];
            result.Provinces = splitAdress[3];
            result.WardCode = int.Parse(splitAdressCode[0]);
            result.DistrictCode = int.Parse(splitAdressCode[1]);
            result.ProvinceCode = int.Parse(splitAdressCode[2]);
            return result;
        }
        public async Task<Option<bool, string>> UpdateProfile(UserProfile user)
        {
            return await(user)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedLoginName = _unitOfWork.Repository<User>().Any(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == user.LoginName.ToUpper().TrimStart().TrimEnd() && x.Id != user.Id);
                    if (existedLoginName)
                    {
                        return Option.None<bool, string>("Tài khoản đã tồn tại, hãy thử dùng tên tài khoản khác!");
                    }
                    else
                    {
                        var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == user.Id && x.IsActive);
                        existedUser.Name = user.Name;
                        existedUser.LoginName = user.LoginName;
                        existedUser.TelNum = user.TelNum;
                        existedUser.Email = user.Email;
                        existedUser.Address = user.Streets + ", " + user.Wards + ", " + user.Districts + ", " + user.Provinces;
                        existedUser.AddressCode = user.WardCode.ToString() + ", " + user.DistrictCode.ToString() + ", " + user.ProvinceCode.ToString();
                        _unitOfWork.Repository<User>().Update(existedUser);
                    }
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId)
        {
            var result = Enumerable.Empty<Product>();
            result = await _unitOfWork.Repository<Product>().Get(x => x.category.Id == categoryId && x.IsActive).Include(x => x.category).Include(x => x.brand).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<OrderDetailDTO>> GetAllProductByOrderID(int orderId)
        {
            var secondJoin = Enumerable.Empty<OrderDetailDTO>();
            var existedOrderDetail = await _unitOfWork.Repository<OrderDetail>().Get(x => x.OrderId == orderId).ToListAsync();
            var feedbacks = await _unitOfWork.Repository<Feedback>().Get(x => x.OrderId == orderId).ToListAsync();
            var existedProductInOder = await _unitOfWork.Repository<Product>().Get(x => existedOrderDetail.Select(x => x.ProductId).Any(y => y == x.Id)).ToListAsync();
            secondJoin = existedOrderDetail.GroupJoin(existedProductInOder, or => or.ProductId, pr => pr.Id, (or, pr) => new { or, pr })
                                     .SelectMany(x => x.pr.DefaultIfEmpty(), (orData, prData) => new OrderDetailDTO
                                     {
                                        OrderId = orderId,
                                        ProductId = prData.Id,
                                        ProductName = prData.ProductName,
                                        Image = prData.Image,
                                        Price = orData.or.Price,
                                        Discount = prData.Discount,
                                        Quantity = orData.or.Quantity,
                                     }).ToList();
            var result = secondJoin.GroupJoin(feedbacks,
              firstSelector => new {
                  firstSelector.OrderId,
                  firstSelector.ProductId
              },
              secondSelector => new {
                  secondSelector.OrderId,
                  secondSelector.ProductId
              },
              (product, feedback) => new { product, feedback }).SelectMany(grp => grp.feedback.DefaultIfEmpty(),
                         (pro, fed) => new OrderDetailDTO
                         {
                             OrderId = orderId,
                             ProductId = pro.product.ProductId,
                             ProductName = pro.product.ProductName,
                             Image = pro.product.Image,
                             Price = pro.product.Price,
                             Discount = pro.product.Discount,
                             Quantity = pro.product.Quantity,
                             Comments = fed?.Comments,
                             Votes = fed?.Votes
                         });
            return result;
        }
        #region Private
        public string Get8CharacterRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.Substring(0, 6);  // Return 6 character string
        }
        public async Task<List<SearchProduct>> SearchProduct(string keyWord)
        {
            return await _unitOfWork.Repository<Product>().Get(x => (x.ProductName.ToUpper().TrimStart().TrimEnd() == keyWord.ToUpper().TrimStart().TrimEnd() 
                                                                     || x.ProductName.ToUpper().TrimStart().TrimEnd().Contains(keyWord.ToUpper().TrimStart().TrimEnd())) 
                                                                     && x.IsActive)
                                                          .Select(x => new SearchProduct()
                                                          {
                                                              ProductName = x.ProductName,
                                                              Id = x.Id,
                                                              BrandName = x.brand.BrandName
                                                          }).ToListAsync();
        }
        #endregion
    }
}
