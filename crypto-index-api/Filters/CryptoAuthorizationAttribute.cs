using System;
using crypto_index_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CryptoAuthorize : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Headers["token"];

        if (string.IsNullOrEmpty(token) || token.Length != 16)
        {
            context.Result = new BadRequestObjectResult(new ErrorResponse() { Message = "Token inválido" });
        }
    }
}