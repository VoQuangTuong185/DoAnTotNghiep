using CategoryService.DTO;

namespace CategoryService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCategory(CategoryPublishDto categoryPublishDto);
        void InactivedCategory(CategoryUpdateDto categoryUpdatedDto);
        void UpdatedCategory(CategoryUpdateDto categoryUpdatedDto);
    }
}
