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
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;
using DoAnTotNghiep.DTOM;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace WebAppAPI.Services.Business
{
    public class _AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<_AdminService> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        public _AdminService(IUnitOfWork unitOfWork, ILogger<_AdminService> logger, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }
        public async Task<Option<bool, string>> CheckExistedAndSendConfirmMail(RegisterUserOldDTO user)
        {
            return await (user)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedUser = await _unitOfWork.Repository<User>().Get(x => (x.LoginName.ToUpper().TrimStart().TrimEnd() == user.LoginName.ToUpper().TrimStart().TrimEnd()
                                                                        || x.Email.ToUpper().TrimStart().TrimEnd() == user.Email.ToUpper().TrimStart().TrimEnd()
                                                                        || x.TelNum.TrimEnd() == user.TelNum.TrimEnd())
                                                                        && x.IsActive)
                               .ToListAsync();
                    if (existedUser.Any())
                    {
                        if (existedUser.FirstOrDefault().LoginName.ToUpper().TrimStart().TrimEnd() == user.LoginName.ToUpper().TrimStart().TrimEnd())
                        {
                            return Option.None<bool, string>("Tên tài khoản này đã tồn tại!");
                        }
                        if (existedUser.FirstOrDefault().Email.ToUpper().TrimStart().TrimEnd() == user.Email.ToUpper().TrimStart().TrimEnd())
                        {
                            return Option.None<bool, string>("Địa chỉ email này đã tồn tại!");
                        }
                        if (existedUser.FirstOrDefault().TelNum.TrimEnd() == user.TelNum.TrimEnd())
                        {
                            return Option.None<bool, string>("Số điện thoại này đã tồn tại!");
                        }
                    }
                    return Option.Some<bool, string>(true);
                });
        }
        public async Task<string> SendConfirmCodeRegister(RegisterUserOldDTO user)
        {
            var confirmCode = Get8CharacterRandomString();
            var mailInformation = new MailPublishedDto("ConfirmRegister", user.Name, user.Email, "[VĂN PHÒNG PHẨM 2023] XÁC NHẬN ĐĂNG KÝ TÀI KHOẢN", "VĂN PHÒNG PHẨM 2023", confirmCode, "Mail_Published");
            _messageBusClient.PublishMail(mailInformation);
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
                IsActive = true,
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
            insertUser.VipsId = 14;
            _unitOfWork.Repository<User>().Add(insertUser);
            var mailInformation = new MailPublishedDto("RegisterSuccessfullyEmail", user.Name, user.Email, "[VĂN PHÒNG PHẨM 2023] ĐĂNG KÝ TÀI KHOẢN THÀNH CÔNG", "VĂN PHÒNG PHẨM 2023", string.Empty, "Mail_Published");
            _messageBusClient.PublishMail(mailInformation);
            return _unitOfWork.SaveChanges();
        }
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var result = Enumerable.Empty<UserDTO>();
            var listUsers = await _unitOfWork.Repository<User>().Get(x => x.LoginName != null).Include("UserAPIs").Include(x => x.vips).ToListAsync();
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
                Discount = x.vips?.Discount
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
                           return Option.None<bool, string>("Tài khoản đã tồn tại, hãy thử dùng tên tài khoản khác!");

                       var existedPhoneNumber = _unitOfWork.Repository<User>().Any(x => x.TelNum.TrimStart().TrimEnd() == user.TelNum.TrimStart().TrimEnd() && x.Id != user.Id);
                       if (existedPhoneNumber)
                           return Option.None<bool, string>("Số điện thoại đã tồn tại, hãy thử dùng số điện thoại khác!");
                       
                        var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == user.Id);
                        existedUser.Name = user.Name;
                        existedUser.LoginName = user.LoginName;
                        existedUser.TelNum = user.TelNum;
                        _unitOfWork.Repository<User>().Update(existedUser);
                       
                       if (await _unitOfWork.SaveChangesAsync())
                           return Option.Some<bool, string>(true);

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
        public async Task<Option<bool, string>> CreateProduct(ProductDTOCreate product)
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
                                                          .Include(x => x.feedbacks)
                                                          .FirstOrDefaultAsync();
            var result = _mapper.Map<ProductDTOShow>(existedProduct);
            if (existedProduct.feedbacks.Any())
            {
                result.AverageVote = (int)Math.Ceiling(existedProduct.feedbacks.Select(x => x.Votes).Average());
            }
            return result;
        }
        public async Task<IEnumerable<Brand>> GetAllBrand(string type)
        {
            var result = Enumerable.Empty<Brand>();
            if (type == "all")
            {
                result = await _unitOfWork.Repository<Brand>().Get(x => true).ToListAsync();
            }
            else if(type == "active")
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
            var existedOrderByUserId = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Pending" && x.IsActive)
                                                                            .Include(x => x.orderDetails)
                                                                            .Include(x => x.user)
                                                                            .ToListAsync();
            return getListOrder(existedOrderByUserId);
        }
        public async Task<IEnumerable<OrderDTO>> GetProcessingOrder()
        {
            var existedOrderByUserId = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Processing" && x.IsActive)
                                                                            .Include(x => x.orderDetails)
                                                                            .Include(x => x.user)
                                                                            .ToListAsync();
            return getListOrder(existedOrderByUserId);
        }
        public async Task<IEnumerable<OrderDTO>> GetSuccessOrder()
        {
            var existedOrderByUserId = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Success" && x.IsActive)
                                                                            .Include(x => x.orderDetails)
                                                                            .Include(x => x.user)
                                                                            .ToListAsync();
            return getListOrder(existedOrderByUserId);
        }
        public async Task<IEnumerable<OrderDTO>> GetCancelOrder()
        {
            var existedOrderByUserId = await _unitOfWork.Repository<Order>().Get(x => x.Status == "Cancel" && x.IsActive)
                                                                            .Include(x => x.orderDetails)
                                                                            .Include(x => x.user)
                                                                            .ToListAsync();
            return getListOrder(existedOrderByUserId);
        }
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _unitOfWork.Repository<Category>().Get(x => true).ToListAsync();
            //result = await _unitOfWork.Repository<Category>().Get(x => x.IsActive).ToListAsync();
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
                        return Option.None<bool, string>("Đã tồn tại danh mục " + existedCategoryName.CategoryName + ". Hãy thử lại!");

                    var insertCategory = new Category();
                    insertCategory.CategoryName = category.CategoryName;
                    insertCategory.Description = category.Description; 
                    insertCategory.Image = category.Image;

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
                    var existedCategory = _unitOfWork.Repository<Category>().FirstOrDefault(x => x.Id == categoryId);
                    if (existedCategory != null)
                    {
                        existedCategory.IsActive = !existedCategory.IsActive;
                    }
                    _unitOfWork.Repository<Category>().Update(existedCategory);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
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
                                         Discount = orData.or.Discount,
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
        public async Task<Option<bool, string>> CancelOrder(AdminOrderModel order)
        {
            return await (order)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().Get(x => x.Id == order.OrderId).Include(x => x.orderDetails).FirstOrDefault();
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
                    existedOrder.UpdatedDate = DateTime.UtcNow;
                    existedOrder.UpdatedBy = order.UpdateBy;
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("CancelOrder", existedUser.Name, existedUser.Email, "[VĂN PHÒNG PHẨM 2023] ĐƠN HÀNG ĐÃ BỊ HUỶ", "VĂN PHÒNG PHẨM 2023", order.OrderId.ToString(), "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> ConfirmOrder(AdminOrderModel order)
        {
            return await (order)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().FirstOrDefault(x => x.Id == order.OrderId);
                    var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == existedOrder.UserId);
                    existedOrder.Status = "Processing";
                    existedOrder.UpdatedDate = DateTime.UtcNow;
                    existedOrder.UpdatedBy = order.UpdateBy;
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("ConfirmOrder", existedUser.Name, existedUser.Email, "[VĂN PHÒNG PHẨM 2023] ĐƠN HÀNG ĐÃ ĐƯỢC XÁC NHẬN", "VĂN PHÒNG PHẨM 2023", order.OrderId.ToString(), "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> SuccessOrder(AdminOrderModel order)
        {
            return await (order)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedOrder = _unitOfWork.Repository<Order>().Get(x => x.Id == order.OrderId).Include(x => x.orderDetails).FirstOrDefault();
                    var listProduct = _unitOfWork.Repository<Product>().Get(x => existedOrder.orderDetails.Select(x => x.ProductId).ToList().Contains(x.Id)).ToList();
                    var user = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == existedOrder.UserId);

                    if (user == null)
                        return Option.None<bool, string>("Lỗi không tìm thấy tài khoản mua hàng!");

                    var allVIP = _unitOfWork.Repository<VIP>().Get(x => x.IsActive).ToList();
                    var allOrder = _unitOfWork.Repository<Order>().Get(x => x.UserId == user.Id && x.Status == "Success" && x.IsActive).ToList();
                    user.VipsId = handleUserVip(allOrder, existedOrder, allVIP);
                    _unitOfWork.Repository<User>().Update(user);

                    existedOrder.Status = "Success";
                    existedOrder.UpdatedDate = DateTime.UtcNow;
                    existedOrder.UpdatedBy = order.UpdateBy;
                    _unitOfWork.Repository<Order>().Update(existedOrder);

                    existedOrder.orderDetails.ForEach(async x =>
                    {
                        var existedProduct = listProduct.Where(y => y.Id == x.ProductId).FirstOrDefault();
                        if (existedProduct != null)
                            existedProduct.SoldQuantity = existedProduct.SoldQuantity + x.Quantity;
                    });
                    _unitOfWork.Repository<Product>().UpdateRange(listProduct);
                    if (await _unitOfWork.SaveChangesAsync())
                    {
                        var mailInformation = new MailPublishedDto("SuccessOrder", user.Name, user.Email, "[VĂN PHÒNG PHẨM 2023] ĐƠN HÀNG ĐÃ HOÀN TẤT", "VĂN PHÒNG PHẨM 2023", order.OrderId.ToString(), "Mail_Published");
                        _messageBusClient.PublishMail(mailInformation);
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<IEnumerable<FeedbackShowDetail>> GetFeedbackByProductId(int ProductId)
        {
            var existedFeedbacks = await _unitOfWork.Repository<Feedback>().Get(x => x.ProductId == ProductId).Include(x => x.users).ToListAsync();
            return existedFeedbacks.Select(x => new FeedbackShowDetail()
            {
                ProductId = x.ProductId,
                UserId = x.UserId,
                UserName = x.users.Name,
                LoginName = x.users.LoginName,
                Comments = x.Comments,
                Votes = x.Votes,
                OrderId = x.OrderId,
                AdminReply = x.AdminReply,
                CreatedDate = x.UpdatedDate != null ? x.UpdatedDate : x.CreatedDate,
                ReplyDate = x.ReplyDate
            }).OrderByDescending(x => x.CreatedDate).ToList();
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
        public async Task<Option<bool, string>> ReplyFeedback(FeedbackShowDetail feedback)
        {
            return await(feedback)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var replyFeedback = await _unitOfWork.Repository<Feedback>()
                        .FirstOrDefaultAsync(x => x.UserId == feedback.UserId && x.OrderId == feedback.OrderId && x.ProductId == feedback.ProductId);

                    if (replyFeedback == null)
                        return Option.None<bool, string>("Không tìm thấy đánh giá này!");

                    replyFeedback.AdminReply = feedback.AdminReply;
                    replyFeedback.ReplyDate = DateTime.UtcNow;

                    _unitOfWork.Repository<Feedback>().Update(replyFeedback);
                    if (await _unitOfWork.SaveChangesAsync())
                        return Option.Some<bool, string>(true);

                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public string Get8CharacterRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.Substring(0, 6);  // Return 6 character string
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
        public async Task<UserProfile> GetInfoUser(int userId)
        {
            var result = new UserProfile();
            var existedUser = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == userId);
            result.Id = existedUser.Id;
            result.Name = existedUser.Name;
            result.LoginName = existedUser.LoginName;
            result.Email = existedUser.Email;
            result.TelNum = existedUser.TelNum;
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
        public async Task<IEnumerable<OrderStatistical>> GetOrderStatisticalsByFilter(OrderStatisticalFilter filter)
        {
            var result = new List<OrderStatistical>();
            var existedOrder = await _unitOfWork.Repository<Order>().Get(x => true).ToListAsync();
            if (filter.Method == "year")
            {
                var tempResult = existedOrder.Where(x => x.CreatedDate.Year == filter.DateFrom.Year).ToList().GroupBy(order => order.Status)
                                        .OrderBy(group => group.Key)
                                        .Select(group => Tuple.Create(group.Key, group.Count())).ToList();
                result = tempResult.Select(x => new OrderStatistical(x.Item1, x.Item2)).ToList();
            }
            else if (filter.Method == "day")
            {
                var tempResult = existedOrder.Where(x => x.CreatedDate.Day == filter.DateFrom.Day).ToList().GroupBy(order => order.Status)
                                        .OrderBy(group => group.Key)
                                        .Select(group => Tuple.Create(group.Key, group.Count())).ToList();
                result = tempResult.Select(x => new OrderStatistical(x.Item1, x.Item2)).ToList();
            }
            else if (filter.Method == "range")
            {
                var tempResult = existedOrder.Where(x => x.CreatedDate >= filter.DateFrom && x.CreatedDate <= filter.DateTo).ToList().GroupBy(order => order.Status)
                                        .OrderBy(group => group.Key)
                                        .Select(group => Tuple.Create(group.Key, group.Count())).ToList();
                result = tempResult.Select(x => new OrderStatistical(x.Item1, x.Item2)).ToList();
            }
            if (!result.Any(x => x.Status == "Cancel")){
                result.Add(new OrderStatistical("Cancel", 0));
            }
            if (!result.Any(x => x.Status == "Processing"))
            {
                result.Add(new OrderStatistical("Processing", 0));
            }
            if (!result.Any(x => x.Status == "Pending"))
            {
                result.Add(new OrderStatistical("Pending", 0));
            }
            if (!result.Any(x => x.Status == "Success"))
            {
                result.Add(new OrderStatistical("Success", 0));
            }
            return result.OrderBy(x => x.Status);
        }
        #region Private
        string handlePayment(string payment)
        {
            string paymentMethod = null;

            switch (payment)
            {
                case "A":
                    paymentMethod = "Thanh toán khi nhận hàng";
                    break;
                case "M":
                    paymentMethod = "Thanh toán qua ngân hàng";
                    break;
                case "P":
                    paymentMethod = "Thanh toán qua VNPAY";
                    break;
            }
            return paymentMethod;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        IEnumerable<OrderDTO> getListOrder(List<Order> listOrder)
        {
            return listOrder.Select(x => new OrderDTO
            {
                Id = x.Id,
                ProductCount = x.orderDetails.Count(),
                TotalBill = x.TotalBill,
                orderDate = x.CreatedDate,
                Payment = handlePayment(x.Payment),
                Address = x.Address,
                CustomerName = x.user?.Name,
                Email = x.user?.Email,
                DiscountVIP = x.DiscountVIP
            })
            .OrderByDescending(x => x.orderDate)
            .ToList();
        }
        int handleUserVip(IList<Order> allOrder, Order newOrder, List<VIP> allVIP)
        {
            var sum = allOrder.Select(x => x.TotalBill).Sum() + newOrder.TotalBill;
            var correctVIP = allVIP.FirstOrDefault(x => (x.PriceFrom <= sum && x.PriceTo >= sum) || (x.PriceFrom <= sum && x.PriceTo == 0));

            if (correctVIP != null) 
                return correctVIP.Id;

            return 14;
        }
        #endregion
    }
}
