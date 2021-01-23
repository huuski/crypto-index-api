using System;
using crypto_index_api.Models;
using crypto_index_api.Models.Request;
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
        [Route("btc")]
        public IActionResult Get(int? quantity)
        {
            try
            {
                var response = _cryptoService.GetCurrentPrice(quantity);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("btc")]
        public IActionResult Post([FromBody] BtcRequest btcRequest)
        {
            try
            {
                _cryptoService.UpdateCurrency(btcRequest);

                return Ok(new SuccessResponse() { Message = "Valor alterado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}
