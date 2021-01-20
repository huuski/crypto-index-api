using System;
using System.Collections.Generic;
using crypto_index_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace crypto_index_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        CryptoService _cryptoService = new CryptoService();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var response = _cryptoService.GetCurrentPrice();

                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Error");
            }
        }

        // POST: api/Crypto
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Crypto/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
