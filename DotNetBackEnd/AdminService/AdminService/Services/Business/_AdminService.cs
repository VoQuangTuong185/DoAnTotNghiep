using AutoMapper;
using CategoryService.AsyncDataServices;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async.Extensions;
using DoAnTotNghiep.DTO;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Repositories;
using WebAppAPI.Services.Contracts;
using AdminService.DTO;
using Nest;

namespace WebAppAPI.Services.Business
{
    public class _AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<_AdminService> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IUserService _IUserService;
        public _AdminService(IUnitOfWork unitOfWork, ILogger<_AdminService> logger, IMapper mapper, IMessageBusClient messageBusClient, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _IUserService = userService;
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
            var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == loginName.ToUpper().TrimStart().TrimEnd());
            existedUser.IsActive = !existedUser.IsActive;
            _unitOfWork.Repository<User>().Update(existedUser);
            return _unitOfWork.SaveChanges();
        }
        public async Task<bool> CheckExistedLoginName(string loginName)
        {
            return _unitOfWork.Repository<User>().Any(x => x.LoginName.ToUpper().TrimStart().TrimEnd() == loginName.ToUpper().TrimStart().TrimEnd());
        }
        public async Task<Option<bool, string>> EditUser(UserAdminDTO user)
        {
            return await (user)
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
                           var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == user.Id);
                           existedUser.Name = user.Name;
                           existedUser.LoginName = user.LoginName;
                           existedUser.TelNum = user.TelNum;
                           existedUser.Address = user.Address;
                           _unitOfWork.Repository<User>().Update(existedUser);
                       }
                       if (await _unitOfWork.SaveChangesAsync())
                       {
                           return Option.Some<bool, string>(true);
                       }
                       return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
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
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });       
        }
        public async Task<Option<bool, string>> CreateProduct(ProductDTOShow product)
        {
            return await (product)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedProduct = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(x => x.ProductName.ToUpper().TrimStart().TrimEnd() == product.ProductName.ToUpper().TrimStart().TrimEnd() &&
                                                                                                          x.BrandId == product.BrandId && x.IsActive);
                    if (existedProduct != null)
                    {
                        return Option.None<bool, string>("Đã tồn tại sản phẩm thuộc nhãn hàng " + existedProduct.brand.BrandName + ". Hãy thử lại!");
                    }
                    var existedCategory = await _unitOfWork.Repository<Category>().Get(x => x.Id == product.CategoryId).FirstOrDefaultAsync();
                    var insertCourse = _mapper.Map<Product>(product);
                    insertCourse.CategoryId = existedCategory.Id;
                    _unitOfWork.Repository<Product>().Add(insertCourse);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateProduct(ProductDTOUpdate product)
        {
            return await (product)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var allProduct = await _unitOfWork.Repository<Product>().Get(x => true).ToListAsync();
                        var existedProduct = allProduct.FirstOrDefault(x => x.Id == product.Id);
                        var existedProductName = allProduct.FirstOrDefault(x => x.Id != product.Id && x.ProductName.ToUpper().TrimStart().TrimEnd() == product.ProductName.ToUpper().TrimStart().TrimEnd()
                                                                                && x.BrandId == product.BrandId);
                        if (existedProductName != null)
                        {
                            return Option.None<bool, string>("Đã tồn tại sản phẩm thuộc nhãn hàng " + existedProduct.brand.BrandName + ". Hãy thử lại!");
                        }
                        existedProduct.ProductName = product.ProductName;
                        existedProduct.Description = product.Description;
                        existedProduct.Image = product.Image;
                        existedProduct.Price = product.Price;
                        existedProduct.Discount = product.Discount;
                        existedProduct.Quanity = product.Quanity;
                        existedProduct.CategoryId = product.CategoryId;
                        existedProduct.BrandId = product.BrandId;
                        existedProduct.ImageDetail = String.Join(",", product.ImageDetail);


                        _unitOfWork.Repository<Product>().Update(existedProduct);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
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
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public async Task<ProductDTOShow> GetExistedProduct(int ProductId)
        {
            var existedProduct = await _unitOfWork.Repository<Product>().Get(x => x.Id == ProductId)
                                                          .Include(x => x.brand)
                                                          .Include(x => x.category)
                                                          .FirstOrDefaultAsync();
            return _mapper.Map<ProductDTOShow>(existedProduct);
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
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> CreateBrand(CreateBrandDTO brand)
        {
            return await (brand)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedBrand = await _unitOfWork.Repository<Brand>().FirstOrDefaultAsync(x => x.BrandName.ToUpper().TrimStart().TrimEnd() == brand.BrandName.ToUpper().TrimStart().TrimEnd() && x.IsActive);
                    if (existedBrand != null)
                    {
                        return Option.None<bool, string>("Đã tồn tại nhãn hàng này. Hãy thử lại!");
                    }
                    var insertBrand = _mapper.Map<Brand>(brand);
                    _unitOfWork.Repository<Brand>().Add(insertBrand);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateBrand(BrandDTO brand)
        {
            return await(brand)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var allBrand = await _unitOfWork.Repository<Brand>().Get(x => true).ToListAsync();
                    var existedBrand = allBrand.FirstOrDefault(x => x.Id == brand.Id);
                    var existedBrandName = allBrand.FirstOrDefault(x => x.Id != brand.Id && x.BrandName.ToUpper().TrimStart().TrimEnd() == brand.BrandName.ToUpper().TrimStart().TrimEnd());
                    if (existedBrandName != null)
                    {
                        return Option.None<bool, string>("Đã tồn tại nhãn hàng này. Hãy thử lại!");
                    }
                    var updatedBrand = _mapper.Map<Brand>(brand);
                    updatedBrand.IsActive = existedBrand.IsActive;
                    _unitOfWork.Repository<Brand>().Update(updatedBrand);
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
            result = await _unitOfWork.Repository<Product>().Get(x => x.category.Id == categoryId).ToListAsync();
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
        public async Task<IEnumerable<Category>> GetAllCategory(string type)
        {
            var result = Enumerable.Empty<Category>();
            if (type == "admin")
            {
                result = await _unitOfWork.Repository<Category>().Get(x => true).ToListAsync();
            }
            else if (type == "user")
            {
                result = await _unitOfWork.Repository<Category>().Get(x => x.IsActive).ToListAsync();
            }
            return result;
        }
        public async Task<Option<bool, string>> CreateCategory(CategoryCreateDto category)
        {
            return await (category)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedCategoryName = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(x => x.CategoryName.ToUpper().TrimStart().TrimEnd()
                                                                                                            == category.CategoryName.ToUpper().TrimStart().TrimEnd() && x.IsActive);
                    if (existedCategoryName != null)
                    {
                        return Option.None<bool, string>("Đã tồn tại sản phẩm thuộc nhãn hàng " + existedCategoryName.CategoryName + ". Hãy thử lại!");
                    }
                    var insertCategory = _mapper.Map<Category>(category);
                    _unitOfWork.Repository<Category>().Add(insertCategory);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateCategory(Category category)
        {
            return await (category)
                    .SomeNotNull().WithException("Null input")
                    .FlatMapAsync(async req =>
                    {
                        var existedCategoryName = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.CategoryName.ToUpper().TrimStart().TrimEnd() 
                                                                                        == category.CategoryName.ToUpper().TrimStart().TrimEnd() && c.Id != category.Id);

                        var existedCategory = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == category.Id);

                        if (existedCategoryName != null)
                        {
                            return Option.None<bool, string>("Đã tồn tại tên danh mục này. Hãy thử lại!");
                        }                       

                        if (existedCategory != null)
                        {
                            existedCategory.CategoryName = category.CategoryName;
                            existedCategory.Description = category.Description;
                            existedCategory.Image = category.Image;
                        }

                        _unitOfWork.Repository<Category>().Update(existedCategory);
                        if (await _unitOfWork.SaveChangesAsync())
                        {
                            return Option.Some<bool, string>(true);
                        }
                        return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                    });
        }
        public Category GetCategoryById(int id)
        {
            return _unitOfWork.Repository<Category>().FirstOrDefault(x => x.Id == id);
        }
        public async Task<Option<bool, string>> InactiveCategory(int categoryId)
        {
            return await(categoryId)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedCategory = _unitOfWork.Repository<Product>().FirstOrDefault(x => x.Id == categoryId);
                    if (existedCategory != null)
                    {
                        existedCategory.IsActive = !existedCategory.IsActive;
                    }
                    _unitOfWork.Repository<Product>().Update(existedCategory);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
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
