using AutoMapper;
using Castle.Core.Internal;
using CategoryService.AsyncDataServices;
using DoAnTotNghiep.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Optional;
using Optional.Async.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using DoAnTotNghiep.DTOM;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;
using WebAppAPI.Repositories;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;
using System.Xml.Linq;

namespace WebAppAPI.Services.Business
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessageBusClient _messageBusClient;

        public UserService(IUnitOfWork unitOfWork, IMessageBusClient messageBusClient, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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
                var mailInformation = new MailPublishedDto("ConfirmRegister", user.Name, user.Email, "[TUONG STATIONERY STORE] Confirm Email", "Confirm Register For [TUONG STATIONERY STORE] ", confirmCode, "Mail_Published");
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
                var mailInformation = new MailPublishedDto("ConfirmForgetPassword", string.Empty, email, "[TUONG STATIONERY STORE] Confirm Email", "Confirm Forget Password For TUONG STATIONERY STORE", confirmCode, "Mail_Published");
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
                var mailInformation = new MailPublishedDto("ConfirmChangeEmail", string.Empty, email, "[TUONG STATIONERY STORE] Confirm Email", "Confirm Change Email Profile For TUONG STATIONERY STOREs", confirmCode, "Mail_Published");
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
                var mailInformation = new MailPublishedDto("ConfirmChangePassword", existedUser.Name, existedUser.Email, "[TUONG STATIONERY STORE] Confirm Email", "Confirm Change Password For TUONG STATIONERY STORE ", string.Empty, "Mail_Published");
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
        public async Task<Option<bool, string>> AddCart(AddCart addCart)
        {
            return await(addCart)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedProductInCart = _unitOfWork.Repository<Cart>().Get(x => x.ProductId == addCart.ProductId && x.UserId == addCart.UserId).FirstOrDefault();
                    if (existedProductInCart != null)
                    {
                        existedProductInCart.Quantity = existedProductInCart.Quantity + addCart.Quanity;
                        existedProductInCart.UpdatedDate = DateTime.UtcNow;
                        _unitOfWork.Repository<Cart>().Update(existedProductInCart);
                    }
                    else
                    {
                        var newCart = new Cart();
                        newCart.ProductId = addCart.ProductId;
                        newCart.UserId= addCart.UserId;
                        newCart.Quantity = addCart.Quanity;
                        newCart.CreatedDate= DateTime.UtcNow;
                        _unitOfWork.Repository<Cart>().Add(newCart);
                    }
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<IEnumerable<CartDTO>> GetCartByUserID(int userId)
        {
            var existedProductInCart = await _unitOfWork.Repository<Cart>().Get(x => x.UserId == userId).Include(x => x.product).ThenInclude(x => x.category).ToListAsync();
            var result = Enumerable.Empty<CartDTO>();
            result = existedProductInCart.Select(x => new CartDTO
            {
                UserId = userId,
                ProductId = x.ProductId,
                ProductName = x.product.ProductName,
                CategoryName = x.product.category.CategoryName,
                Image = x.product.Image,
                Price = x.product.Price,
                Discount = x.product.Discount,
                Quanity = x.Quantity
            }).ToList();
            return result;
        }
        public async Task<Option<bool, string>> InActiveCart(CartDTO cart)
        {
            return await(cart)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedCart = _unitOfWork.Repository<Cart>().FirstOrDefault(x => x.UserId == cart.UserId && x.ProductId == cart.ProductId);
                        _unitOfWork.Repository<Cart>().Delete(existedCart);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public async Task<Option<bool, string>> CreateOrder(CreateOrder order)
        {
            return await(order)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedProductInCart = _unitOfWork.Repository<Cart>().GetNoTracking(x => x.UserId == order.UserId).Include(x => x.product).ThenInclude(x => x.category).ToList();
                        var listProduct = _unitOfWork.Repository<Product>().Get(x => existedProductInCart.Select(x => x.product.Id).ToList().Contains(x.Id)).ToList();
                        bool isOverQuantity = false;
                        existedProductInCart.ForEach(async x =>
                        {
                            var existedProduct = listProduct.Where(y => y.Id == x.ProductId).FirstOrDefault();
                            if (existedProduct.Quanity < x.Quantity)
                            {
                                isOverQuantity = true;
                                x.Quantity = existedProduct.Quanity;
                            }
                        });

                        if (isOverQuantity)
                        {
                            _unitOfWork.Repository<Cart>().UpdateRange(existedProductInCart);
                            await _unitOfWork.SaveChangesAsync();
                            return Option.None<bool, string>("Số lượng bạn muốn mua đang lớn hơn số lượng sẵn có!");
                        }

                        var insertOrder = new Order();
                        insertOrder.UserId = order.UserId;
                        insertOrder.Status = "Pending";
                        insertOrder.UpdatedBy = order.UserId;
                        var timeNow = DateTime.UtcNow;
                        insertOrder.CreatedDate = timeNow;
                        insertOrder.Address = order.Streets + ", " + order.Wards + ", " + order.Districts + ", " + order.Provinces;
                        insertOrder.TotalBill = handleTotalBill(existedProductInCart);
                        insertOrder.Payment = order.Payment;
                        _unitOfWork.Repository<Order>().Add(insertOrder);
                        await _unitOfWork.SaveChangesAsync();

                        var recentlyOrder = await _unitOfWork.Repository<Order>().FirstOrDefaultAsync(x => x.UserId == order.UserId && x.CreatedDate == timeNow && x.IsActive);
                        
                        existedProductInCart.ForEach(async x =>
                        {
                            var existedProduct = listProduct.Where(y => y.Id == x.ProductId).FirstOrDefault();
                            var insertOderDtail = new OrderDetail();
                            insertOderDtail.ProductId = x.ProductId;
                            insertOderDtail.Price = x.product.Price;
                            insertOderDtail.OrderId = recentlyOrder.Id;
                            insertOderDtail.Quantity = x.Quantity;                           
                            if (existedProduct != null)
                            {
                                existedProduct.Quanity = existedProduct.Quanity - x.Quantity;
                            }                           
                            await _unitOfWork.Repository<OrderDetail>().AddAsync(insertOderDtail);
                        });
                        _unitOfWork.Repository<Product>().UpdateRange(listProduct);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            var allUser = _unitOfWork.Repository<UserAPI>().Get(x => x.IsActive && x.RoleId == 2).Include(x => x.user).ToList();
                            var listAdmin = allUser.Select(x => x.user).ToList();
                            var customerName = _unitOfWork.Repository<User>().FirstOrDefault(x => x.IsActive && x.Id == order.UserId).Name;
                            var mailInformation = new MailPublishedDto("CreateOrder", listAdmin.FirstOrDefault().Name, listAdmin.FirstOrDefault().Email, "[TUONG STATIONERY STORE] Confirm Email", "Confirm Change Email Profile For TUONG STATIONERY STOREs", customerName, "Mail_Published");
                            _messageBusClient.PublishMail(mailInformation);
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public async Task<Option<bool, string>> DeleteAllCartAfterOrder(int userId)
        {
            return await(userId)
                   .SomeNotNull().WithException("Null input")
                   .FlatMapAsync(async req =>
                   {
                       var existedProductInCart = _unitOfWork.Repository<Cart>().GetNoTracking(x => x.UserId == userId).ToList();
                       _unitOfWork.Repository<Cart>().DeleteRange(existedProductInCart);
                       if (await _unitOfWork.SaveChangesAsync())
                       {
                           return Option.Some<bool, string>(true);
                       }
                       return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                   });

        }
        public async Task<IEnumerable<OrderDTO>> GetWaitingOrderByUserID(int userId)
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.UserId == userId && x.Status == "Pending" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
            var result = existedProductInCart.Select(x => new OrderDTO
            {
                Id = x.Id,
                ProductCount = x.orderDetails.Count(),
                TotalBill = x.TotalBill,
                orderDate = x.CreatedDate,
                Payment = handlePayment(x.Payment),
                Address = x.Address,
                CustomerName = x.user?.Name,
                Email = x.user?.Email
            }).ToList();
            return result;
        }
        public async Task<IEnumerable<OrderDTO>> GetProcessingOrderByUserID(int userId)
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.UserId == userId && x.Status == "Processing" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
            var result = existedProductInCart.Select(x => new OrderDTO
            {
                Id = x.Id,
                ProductCount = x.orderDetails.Count(),
                TotalBill = x.TotalBill,
                orderDate = x.CreatedDate,
                Payment = handlePayment(x.Payment),
                Address = x.Address,
                CustomerName = x.user?.Name,
                Email = x.user?.Email
            }).ToList();
            return result;
        }
        public async Task<IEnumerable<OrderDTO>> GetSuccessOrderByUserID(int userId)
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.UserId == userId && x.Status == "Success" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
            var result = existedProductInCart.Select(x => new OrderDTO
            {
                Id = x.Id,
                ProductCount = x.orderDetails.Count(),
                TotalBill = x.TotalBill,
                orderDate = x.CreatedDate,
                Payment = handlePayment(x.Payment),
                Address = x.Address,
                CustomerName = x.user?.Name,
                Email = x.user?.Email
            }).ToList();
            return result;
        }
        public async Task<IEnumerable<OrderDTO>> GetCancelOrderByUserID(int userId)
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.UserId == userId && x.Status == "Cancel" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
            var result = existedProductInCart.Select(x => new OrderDTO
            {
                Id = x.Id,
                ProductCount = x.orderDetails.Count(),
                TotalBill = x.TotalBill,
                orderDate = x.CreatedDate,
                Payment = handlePayment(x.Payment),
                Address = x.Address,
                CustomerName = x.user?.Name,
                Email = x.user?.Email
            }).ToList();
            return result;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId)
        {
            var result = Enumerable.Empty<Product>();
            result = await _unitOfWork.Repository<Product>().Get(x => x.category.ExternalID == categoryId && x.IsActive).Include(x => x.category).Include(x => x.brand).ToListAsync();
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
        public async Task<Option<bool, string>> CancelOrder(int orderId)
        {
            return await(orderId)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().Get(x => x.Id == orderId).Include(x => x.orderDetails).FirstOrDefault();
                    var listProduct = _unitOfWork.Repository<Product>().Get(x => existedOrder.orderDetails.Select(x => x.ProductId).ToList().Contains(x.Id)).ToList();
                    var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == existedOrder.UserId);

                    existedOrder.orderDetails.ForEach(async x =>
                    {
                        var existedProduct = listProduct.Where(y => y.Id == x.ProductId).FirstOrDefault();
                        if (existedProduct != null)
                        {
                            existedProduct.Quanity = existedProduct.Quanity + x.Quantity;
                        }
                    });
                    _unitOfWork.Repository<Product>().UpdateRange(listProduct);

                    existedOrder.Status = "Cancel";
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("CancelOrder", existedUser.Name, existedUser.Email, "[TUONG STATIONERY STORE] Confirm Email", "Notification - Your order has been canceled", string.Empty, "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> ConfirmOrder(int orderId)
        {
            return await (orderId)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().FirstOrDefault(x => x.Id == orderId);
                    var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == existedOrder.UserId);
                    existedOrder.Status = "Processing";
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("ConfirmOrder", existedUser.Name, existedUser.Email, "[TUONG STATIONERY STORE] Confirm Email", "Notification - Your order has been confirmed", string.Empty, "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> SuccessOrder(int orderId)
        {
            return await (orderId)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().Get(x => x.Id == orderId).Include(x => x.orderDetails).FirstOrDefault();
                    var listProduct = _unitOfWork.Repository<Product>().Get(x => existedOrder.orderDetails.Select(x => x.ProductId).ToList().Contains(x.Id)).ToList();
                    var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == existedOrder.UserId);
                    existedOrder.Status = "Success";
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    existedOrder.orderDetails.ForEach(async x =>
                    {
                        var existedProduct = listProduct.Where(y => y.Id == x.ProductId).FirstOrDefault();
                        if (existedProduct != null)
                        {
                            existedProduct.SoldQuantity = existedProduct.SoldQuantity + x.Quantity;
                        }
                    });
                    _unitOfWork.Repository<Product>().UpdateRange(listProduct);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("SuccessOrder", existedUser.Name, existedUser.Email, "[TUONG STATIONERY STORE] Confirm Email", "Notification - Your order has been succeeded", string.Empty, "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateCart(UpdateCart cart)
        {
            return await (cart)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedCart = _unitOfWork.Repository<Cart>().FirstOrDefault(x => x.UserId == cart.UserId && x.ProductId == cart.ProductId);
                        var existedProduct = _unitOfWork.Repository<Product>().FirstOrDefault(x => x.Id == cart.ProductId && x.IsActive);
                        if (existedProduct.Quanity < cart.Quantity)
                        {
                            return Option.None<bool, string>("Số lượng bạn muốn mua đang lớn hơn số lượng sẵn có!");
                        }
                        existedCart.Quantity = cart.Quantity;
                        _unitOfWork.Repository<Cart>().Update(existedCart);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        #region Private
        double handleTotalBill(List<Cart> listCarts)
        {
            double totalBill = 0;
            foreach (var cart in listCarts)
            {
                totalBill += cart.Quantity * cart.product.Price - (cart.Quantity * cart.product.Price * cart.product.Discount / 100);
            }
            return totalBill;
        }
        string handlePayment(string payment)
        {
            string paymentMethod = null;

            switch (payment)
            {
                case "A":
                    paymentMethod = "Thanh toán khi nhân hàng";
                    break;
                case "M":
                    paymentMethod = "Thanh toán qua ngân hàng";
                    break;
                case "P":
                    paymentMethod = "Thanh toán qua momo";
                    break;
            }
            return paymentMethod;
        }
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

        public async Task<Option<bool, string>> CreateFeedback(List<FeedbackDTO> feedbacks)
        {
            return await(feedbacks)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    _unitOfWork.Repository<Feedback>().AddRange(_mapper.Map<List<Feedback>>(feedbacks));
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        #endregion
    }
}
