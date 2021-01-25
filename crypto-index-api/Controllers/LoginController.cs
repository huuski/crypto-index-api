using System;
using crypto_index_api.Models;
using crypto_index_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace crypto_index_api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService = new LoginService();

        [HttpPost]
        public IActionResult Login(AuthenticationRequest authenticationRequest)
        {
            try
            {
                AuthenticationResponse authenticationResponse = _loginService.Authenticate(authenticationRequest);

                return Ok(authenticationResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}