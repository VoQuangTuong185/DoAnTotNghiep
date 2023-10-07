﻿using AutoMapper;
using CategoryService.AsyncDataServices;
using CategoryService.Data;
using CategoryService.Data.Entities;
using CategoryService.DTO;
using CategoryService.Model.Services;
using CategoryService.Services.Business;
using CategoryService.Services.Contracts;
using CategoryService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace CategoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;
        private readonly IProductDataClient _productDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private ILog _ILog;
        public CategoryController(ICategoryRepo repository, IMapper mapper, IProductDataClient productDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _productDataClient = productDataClient;
            _messageBusClient = messageBusClient;
            _ILog = Log.GetInstance;
        }
        [HttpGet]
        public async Task<ApiResult> GetCategories(string type)
        {
            var result = new ApiResult();
            try
            {
                result.Data = _repository.GetAllCategories(type);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
            }
            return result;
        }
        [HttpPost]
        public async Task<ApiResult> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var result = new ApiResult();
            var categoryModel = _mapper.Map<Category>(categoryCreateDto);
            try
            {
                (await _repository.CreateCategory(categoryModel)).Match(res =>
                {
                    _repository.SaveChanges();
                    result.Message = "Tạo danh mục " + categoryModel.CategoryName + " thành công!";
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
            };
            if (result.IsSuccess)
            {
                var categoryReadDto = _mapper.Map<CategoryReadDto>(categoryModel);
                // Send Sync Message
                try
                {
                    await _productDataClient.SendCategoryToProduct(categoryReadDto);
                }
                catch (Exception ex)
                {
                    _ILog.LogException($"--> Could not send synchronously: {ex.Message}");
                }
                //Send Async Message
                try
                {
                    var platformPublishedDto = _mapper.Map<CategoryPublishDto>(categoryReadDto);
                    platformPublishedDto.Event = "Category_Published";
                    _messageBusClient.PublishNewCategory(platformPublishedDto);
                }
                catch (Exception ex)
                {
                    _ILog.LogException($"--> Could not send asynchronously: {ex.Message}");
                }
            }
            return result;
        }
        [HttpGet("inactive-category")]
        public async Task<ApiResult> InActivedCategory(int categoryId)
        {
            var result = new ApiResult();
            try
            {
                var recentlyUpdatedCategory = _repository.InactiveCategory(categoryId);

                var sentUpdatedCategory = new Category();
                sentUpdatedCategory.Id = recentlyUpdatedCategory.Id;
                sentUpdatedCategory.CategoryName = recentlyUpdatedCategory.CategoryName; 
                sentUpdatedCategory.IsActive= recentlyUpdatedCategory.IsActive;
                
                var platformPublishedDto = _mapper.Map<CategoryUpdateDto>(sentUpdatedCategory);
                platformPublishedDto.Event = "Category_Updated";
                _messageBusClient.InactivedCategory(platformPublishedDto);
                result.Message = "Thao thác thành công!";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _ILog.LogException(ex.Message);
            }
            return result;
        }
        [HttpPost("update-category")]
        public async Task<ApiResult> UpdateCategory(Category category)
        {
            var result = new ApiResult();
            try
            {
                (await _repository.UpdateCategory(category)).Match(res =>
                {
                    _repository.SaveChanges();
                    result.Message = "Chỉnh sửa danh mục " + category.CategoryName + " thành công!";
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
            if (result.IsSuccess)
            {
                var allCategory = _repository.GetAllCategories("Admin");
                var existedCategory = allCategory.FirstOrDefault(x => x.Id == category.Id);

                var sentUpdatedCategory = new Category();
                sentUpdatedCategory.Id = existedCategory.Id;
                sentUpdatedCategory.CategoryName = existedCategory.CategoryName;
                sentUpdatedCategory.Description = existedCategory.Description;
                sentUpdatedCategory.Image = existedCategory.Image;
                sentUpdatedCategory.IsActive = existedCategory.IsActive;

                var platformPublishedDto = _mapper.Map<CategoryUpdateDto>(sentUpdatedCategory);
                platformPublishedDto.Event = "Category_Updated";
                _messageBusClient.UpdatedCategory(platformPublishedDto);
            }
            return result;
        }
        [HttpGet("get-existed-category")]
        public async Task<ApiResult> GetCategoryById(int categoryId)
        {
            var result = new ApiResult();
            try
            {
                result.Data = _repository.GetCategoryById(categoryId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
            }
            return result;
        }
        [HttpPost("upload-course-image"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
