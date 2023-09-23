using AutoMapper;
using CategoryService.Data;
using CategoryService.Data.Entities;
using Grpc.Core;
namespace CategoryService.SyncDataServices.Grpc
{
    public class GrpcCategoryService : GrpcCategory.GrpcCategoryBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;
        public GrpcCategoryService(ICategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public override Task<CategoryResponse> GetAllCategory(GetAllRequest request, ServerCallContext context)
        {
            var response = new CategoryResponse();
            var categories = _repository.GetAllCategories("admin");

            foreach (var category in categories)
            {
                response.Category.Add(_mapper.Map<GrpcCategoryModel>(category));
            }

            return Task.FromResult(response);
        }
    }
}
