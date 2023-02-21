using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralDeUsuarios.Services.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200);
        }
    }
}