﻿using Microsoft.Extensions.Options;
using ExceptionHandlerOptions = Light.AspNetCore.ExceptionHandlers.ExceptionHandlerOptions;

namespace WebApi.TestOption
{
    public class ErrorHandlerOptions(IConfiguration configuration) : IConfigureOptions<ExceptionHandlerOptions>
    {
        public void Configure(ExceptionHandlerOptions options)
        {
            var readConfig = configuration.GetValue<bool>("HideUnidentifiedException");
            options.HideUndentifyException = readConfig;
        }
    }
}
