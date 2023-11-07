using DoAnTotNghiep.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;

namespace WebAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IAdminService _IAdminService;
        private ILog _ILog;
        public UserController(IAdminService iAdminService)
        {
            _IAdminService = iAdminService;
            //Get the Singleton Log Instance
            _ILog = Log.GetInstance;
        }
    }
}
