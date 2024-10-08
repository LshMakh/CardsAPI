﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AuthProjWebApi.Auth
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred");

            var response = new
            {
                Message = "An unexpected error occurred. Please try again later.",
                Detailed = context.Exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
