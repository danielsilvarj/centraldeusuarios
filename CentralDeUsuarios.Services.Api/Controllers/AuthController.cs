using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralDeUsuarios.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService;

        public AuthController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AutenticarUsuarioCommand command)
        {
            var model = await _usuarioAppService.AutenticarUsuario(command);

            return StatusCode(200, new 
                {
                    message = "Usuário Autenticado com Sucesso",
                    model
                }); 
        }
    }
}