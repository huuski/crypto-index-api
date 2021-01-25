using System;
using System.Text;
using crypto_index_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace crypto_index_api.Controllers
{
    [Route("")]
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                const string html = "<h1>Server is up and running</h1>";
                return Content(html, "text/html", Encoding.UTF8);
                //return Ok("App is running");
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}
