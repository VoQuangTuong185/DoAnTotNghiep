using AutoMapper;
using CategoryService.AsyncDataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THUCTAPTOTNGHIEP.DTOM;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;

namespace THUCTAPTOTNGHIEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        private ILog _ILog;
        public MailController(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
            _ILog = Log.GetInstance;
        }
        [HttpPost("test")]
        public async Task<ApiResult> CreateCategory(MailPublishedDto mailPublishedDto)
        {
            var result = new ApiResult();
            //Send Async Message
            try
            {
                mailPublishedDto.Event = "Mail_Published";
                _messageBusClient.PublishMail(mailPublishedDto);
            }
            catch (Exception ex)
            {
                _ILog.LogException($"--> Could not send asynchronously: {ex.Message}");
            }

            return result;
        }
    }
}
