using AutoMapper;
using CategoryService.AsyncDataServices;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Nest;
using Optional;
using Optional.Async.Extensions;
using System.Linq;
using DoAnTotNghiep.DTO;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Repositories;
using WebAppAPI.Services.Contracts;

namespace WebAppAPI.Services.Business
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminService> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var result = Enumerable.Empty<UserDTO>();
            var listUsers = await _unitOfWork.Repository<User>().Get(x => x.LoginName != null).Include("UserAPIs").ToListAsync();
            foreach (var user in listUsers) {
                user.UserAPIs.ForEach(x => x.user = null);
            }
            result = listUsers.OrderBy(x => x.Id).Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                LoginName= x.LoginName,
                TelNum= x.TelNum,
                IsActive = x.IsActive,
                Address = x.Address,
                UserAPIs = x.UserAPIs,
            });
            return result;
        }
        public async Task<bool> ActiveOrInActiveUser(string loginName)
        {
            var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.LoginName == loginName);
            existedUser.IsActive = !existedUser.IsActive;
            _unitOfWork.Repository<User>().Update(existedUser);
            return _unitOfWork.SaveChanges();
        }
        public async Task<bool> CheckExistedLoginName(string loginName)
        {
            return _unitOfWork.Repository<User>().Any(x => x.LoginName == loginName);
        }
        public async Task<Option<bool, string>> EditUser(UserAdminDTO user)
        {
            return await (user)
                   .SomeNotNull().WithException("Null input")
                   .FlatMapAsync(async req =>
                   {
                       var existedLoginName = _unitOfWork.Repository<User>().Any(x => x.LoginName == user.LoginName && x.Id != user.Id);
                       if (existedLoginName)
                       {
                           return Optional.Option.None<bool, string>("Tài khoản đã tồn tại, hãy thử dùng tên tài khoản khác!");
                       }
                       else
                       {
                           var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == user.Id);
                           existedUser.Name = user.Name;
                           existedUser.LoginName = user.LoginName;
                           existedUser.TelNum = user.TelNum;
                           existedUser.Address = user.Address;
                           _unitOfWork.Repository<User>().Update(existedUser);
                       }
                       if (await _unitOfWork.SaveChangesAsync())
                       {
                           return Optional.Option.Some<bool, string>(true);
                       }
                       return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                   });
        }
        public async Task<Option<bool, string>> SetManagerPermisson(int userId)
        {
            return await (userId)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == userId);
                        var IsExistedPermisson = _unitOfWork.Repository<UserAPI>().Any(x => x.UserId == userId && x.RoleId == 2);
                        var existedPermisson = _unitOfWork.Repository<UserAPI>().FirstOrDefault(x => x.UserId == userId && x.RoleId == 2);
                        if (IsExistedPermisson)
                        {
                            existedPermisson.IsActive = !existedPermisson.IsActive;
                            _unitOfWork.Repository<UserAPI>().Update(existedPermisson);
                        }
                        else
                        {
                            var insertUserPermisson = new UserPermisson();
                            insertUserPermisson.UserId = userId;
                            insertUserPermisson.RoleId = 2;
                            insertUserPermisson.IsActive = true;
                            _unitOfWork.Repository<UserAPI>().Add(_mapper.Map<UserAPI>(insertUserPermisson));
                        }
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Optional.Option.Some<bool, string>(true);
                        }
                        return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });       
        }
        public async Task<bool> CreateProduct(ProductDTO product)
        {
            var existedCategory = await _unitOfWork.Repository<Category>().Get(x => x.ExternalID == product.CategoryId).FirstOrDefaultAsync();
            var insertCourse = _mapper.Map<Product>(product);
            insertCourse.CategoryId = existedCategory.Id;
            _unitOfWork.Repository<Product>().Add(insertCourse);
            return _unitOfWork.SaveChanges();
        }
        public async Task<Option<bool, string>> UpdateProduct(ProductDTO product)
        {
            return await (product)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var updatedProduct = _mapper.Map<Product>(product);
                        updatedProduct.Id = product.Id;
                        _unitOfWork.Repository<Product>().Update(updatedProduct);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Optional.Option.Some<bool, string>(true);
                        }
                        return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public async Task<int> AutoGeneratedProductID()
        {
           var result = await _unitOfWork.Repository<Product>().All().OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync() + 1;
           return result;
        }
        public async Task<Option<bool, string>> InActiveProduct(int productId)
        {
            return await (productId)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedProduct = _unitOfWork.Repository<Product>().FirstOrDefault(x => x.Id == productId);
                        if (existedProduct != null)
                        {
                            existedProduct.IsActive = !existedProduct.IsActive;
                        }
                        _unitOfWork.Repository<Product>().Update(existedProduct);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Optional.Option.Some<bool, string>(true);
                        }
                        return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public async Task<Product> GetExistedProduct(int ProductId)
        {
            return _unitOfWork.Repository<Product>().Get(x => x.Id == ProductId).Include(x => x.brand).FirstOrDefault();
        }
        public async Task<IEnumerable<Product>> GetAllProduct(string type)
        {
            var result = Enumerable.Empty<Product>();
            if (type == "admin")
            {
                result = await _unitOfWork.Repository<Product>().Get(x => true).Include(x => x.category).Include(x => x.brand).ToListAsync();
            } 
            else
            {
                var tempList = await _unitOfWork.Repository<Product>().Get(x => x.IsActive)
                                                                      .Include(x => x.category)
                                                                      .Include(x => x.brand)
                                                                      .Where(x => x.category.IsActive && x.brand.IsActive)
                                                                      .OrderByDescending(x => x.SoldQuantity)
                                                                      .ToListAsync();
                result = tempList;
            }
            return result;
        }
        public async Task<IEnumerable<Brand>> GetAllBrand(string type)
        {
            var result = Enumerable.Empty<Brand>();
            if (type == "admin")
            {
                result = await _unitOfWork.Repository<Brand>().Get(x => true).ToListAsync();
            }
            else if(type == "user")
            {
                result = await _unitOfWork.Repository<Brand>().Get(x => x.IsActive).ToListAsync();
            }           
            return result;
        }
        public async Task<Brand> GetExistedBrand(int BrandId)
        {
            return await _unitOfWork.Repository<Brand>().FirstOrDefaultAsync(x => x.Id == BrandId);
        }
        public async Task<Option<bool, string>> InActiveBrand(int brandId)
        {
            return await(brandId)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedBrand = _unitOfWork.Repository<Brand>().FirstOrDefault(x => x.Id == brandId);
                    if (existedBrand != null)
                    {
                        existedBrand.IsActive = !existedBrand.IsActive;
                    }
                    _unitOfWork.Repository<Brand>().Update(existedBrand);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Optional.Option.Some<bool, string>(true);
                    }
                    return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> CreateBrand(CreateBrandDTO brand)
        {
            return await (brand)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedBrand = await _unitOfWork.Repository<Brand>().FirstOrDefaultAsync(x => x.BrandName == brand.BrandName.TrimStart().TrimEnd() && x.IsActive);
                    if (existedBrand != null)
                    {
                        return Optional.Option.None<bool, string>("Đã tồn tại nhãn hàng này. Hãy thử lại!");
                    }
                    var insertBrand = _mapper.Map<Brand>(brand);
                    _unitOfWork.Repository<Brand>().Add(insertBrand);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Optional.Option.Some<bool, string>(true);
                    }
                    return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateBrand(BrandDTO brand)
        {
            return await(brand)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var allBrand = await _unitOfWork.Repository<Brand>().Get(x => x.IsActive).ToListAsync();
                    var existedBrand = allBrand.FirstOrDefault(x => x.Id == brand.Id);
                    var existedBrandName = allBrand.FirstOrDefault(x => x.Id != brand.Id && x.BrandName == brand.BrandName.TrimStart().TrimEnd());
                    if (existedBrandName != null)
                    {
                        return Optional.Option.None<bool, string>("Đã tồn tại nhãn hàng này. Hãy thử lại!");
                    }
                    var updatedBrand = _mapper.Map<Brand>(brand);
                    updatedBrand.IsActive = existedBrand.IsActive;
                    _unitOfWork.Repository<Brand>().Update(updatedBrand);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Optional.Option.Some<bool, string>(true);
                    }
                    return Optional.Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryID(int categoryId)
        {
            var result = Enumerable.Empty<Product>();
            result = await _unitOfWork.Repository<Product>().Get(x => x.category.ExternalID == categoryId).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<OrderDTO>> GetWaitingOrder()
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Pending" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
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
        public async Task<IEnumerable<OrderDTO>> GetProcessingOrder()
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Processing" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
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
        public async Task<IEnumerable<OrderDTO>> GetSuccessOrder()
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Success" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
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
        public async Task<IEnumerable<OrderDTO>> GetCancelOrder()
        {
            var existedProductInCart = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Cancel" && x.IsActive).Include(x => x.orderDetails).Include(x => x.user).ToListAsync();
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
        #region Private
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
        #endregion
    }
}
