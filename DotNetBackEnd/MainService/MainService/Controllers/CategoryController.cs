using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DoAnTotNghiep.Data;
using DoAnTotNghiep.DTOM;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;

namespace DoAnTotNghiep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;
        private ILog _ILog;
        public CategoryController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _ILog = Log.GetInstance;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoryReadDto>> GetCategory()
        {
            _ILog.LogException("--> Getting Category from CategoryService");

            var categoryItems = _repository.GetAllCategory();

            return Ok(_mapper.Map<IEnumerable<CategoryReadDto>>(categoryItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            _ILog.LogException("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controler");
        }
    }
}
