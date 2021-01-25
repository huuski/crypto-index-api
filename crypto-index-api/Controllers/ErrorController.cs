using crypto_index_api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public ErrorResponse Error()
    {
        var code = 404; // Not Found Status Code

        Response.StatusCode = code;

        return new ErrorResponse() { Message = "Endpoint não encontrado" };
    }
}