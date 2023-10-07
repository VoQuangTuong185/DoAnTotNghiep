﻿using AutoMapper;
using CategoryService.AsyncDataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DoAnTotNghiep.DTOM;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Model;

namespace DoAnTotNghiep.Controllers
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
    }
}
