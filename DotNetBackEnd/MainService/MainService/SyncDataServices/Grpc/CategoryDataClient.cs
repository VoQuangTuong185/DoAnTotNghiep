using AutoMapper;
using CategoryService;
using Grpc.Net.Client;
using Microsoft.CodeAnalysis;
using THUCTAPTOTNGHIEP.Models.Entities;
using CategoryService;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Business;

namespace THUCTAPTOTNGHIEP.SyncDataServices.Grpc
{
    public class CategoryDataClient : ICategoryDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private ILog _ILog;
        public CategoryDataClient(IConfiguration configuration, IMapper mapper) 
        {
            _configuration = configuration;
            _mapper = mapper;
            _ILog = Log.GetInstance;
        }

        public IEnumerable<Category> ReturnAllCategory()
        {
            _ILog.LogException($"--> Calling GRPC Service {_configuration["GrpcCategory"]}");
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var channel = GrpcChannel.ForAddress(_configuration["GrpcCategory"], new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new GrpcCategory.GrpcCategoryClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCategory(request);
                return _mapper.Map<IEnumerable<Category>>(reply.Category);
            }
            catch (Exception ex)
            {
                _ILog.LogException($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}
