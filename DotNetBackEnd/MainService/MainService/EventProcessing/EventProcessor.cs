using AutoMapper;
using Microsoft.CodeAnalysis;
using DoAnTotNghiep.Data;
using DoAnTotNghiep.DTOM;
using System.Text.Json;
using System.Drawing.Design;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Business;

namespace DoAnTotNghiep.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private ILog _ILog;
        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _ILog = Log.GetInstance;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CategoryPublished:
                    addCategory(message);
                    break;
                case EventType.CategoryUpdated:
                    updateCategory(message);
                    break;
                case EventType.MailPublished:
                    _ILog.LogException("--> Don't have any action!!");
                    break;
                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notifcationMessage)
        {
            _ILog.LogException("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch (eventType.Event)
            {
                case "Category_Published":
                    _ILog.LogException("--> Category Published Event Detected");
                    return EventType.CategoryPublished;
                case "Category_Updated":
                    _ILog.LogException("--> Category Updated Event Detected");
                    return EventType.CategoryUpdated;
                case "Mail_Published":
                    _ILog.LogException("--> Mail Published Event Detected");
                    return EventType.MailPublished;
                default:
                    _ILog.LogException("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        private void addCategory(string categoryPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var categoryPublishedDto = JsonSerializer.Deserialize<CategoryPublishedDto>(categoryPublishedMessage);

                try
                {
                    var category = _mapper.Map<Category>(categoryPublishedDto);
                    if (!repo.ExternalCategoryExists(category.ExternalID))
                    {
                        repo.CreateCategory(category);
                        repo.SaveChanges();
                        _ILog.LogException("--> Category added!");
                    }
                    else
                    {
                        _ILog.LogException("--> Category already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    _ILog.LogException($"--> Could not add Category to DB {ex.Message}");
                }
            }
        }
        private void updateCategory(string categoryPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var categoryUpdatedDto = JsonSerializer.Deserialize<CategoryUpdateDto>(categoryPublishedMessage);

                try
                {
                    var existedCategory = repo.GetCategoryById(categoryUpdatedDto.Id);
                    existedCategory.CategoryName = categoryUpdatedDto.CategoryName;
                    existedCategory.ExternalID = categoryUpdatedDto.Id;
                    existedCategory.IsActive = categoryUpdatedDto.IsActive;
                    if (repo.ExternalCategoryExists(existedCategory.ExternalID))
                    {
                        repo.UpdateCategory(existedCategory);
                        repo.SaveChanges();
                        _ILog.LogException("--> Category updated!");
                    }
                    else
                    {
                        _ILog.LogException("--> Update Category fail...");
                    }

                }
                catch (Exception ex)
                {
                    _ILog.LogException($"--> Could not update Category to DB {ex.Message}");
                }
            }
        }
        enum EventType
        {
            CategoryPublished,
            CategoryUpdated,
            MailPublished,
            Undetermined
        }
    }
}
