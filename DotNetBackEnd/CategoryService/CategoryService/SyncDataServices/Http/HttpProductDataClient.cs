using CategoryService.DTO;
using CategoryService.Services.Business;
using CategoryService.Services.Contracts;
using System.Text;
using System.Text;
using System.Text.Json;

namespace CategoryService.SyncDataServices.Http
{
    public class HttpProductDataClient : IProductDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private ILog _ILog;
        public HttpProductDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _ILog = Log.GetInstance;
        }
        public async Task SendCategoryToProduct(CategoryReadDto category)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8,"application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CategoryService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                _ILog.LogException("--> Sync POST to CategoryService was OK!");
            }
            else
            {
                _ILog.LogException("--> Sync POST to CategoryService was NOT OK!");
            }
        }
    }
}
