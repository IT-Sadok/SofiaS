using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;

namespace BookingService.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ValidationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType == "application/json")
            {
                var endpoint = context.GetEndpoint();
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

                if (actionDescriptor != null)
                {
                    foreach (var parameter in actionDescriptor.Parameters)
                    {
                        var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
                        var validator = (IValidator)context.RequestServices.GetService(validatorType);

                        if (validator != null)
                        {
                            var body = await DeserializeRequestBodyAsync(context, parameter.ParameterType);
                            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(body));

                            if (!validationResult.IsValid)
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsJsonAsync(validationResult.Errors);
                                return;
                            }
                        }
                    }
                }
            }

            await _requestDelegate.Invoke(context);
        }

        private async Task<object> DeserializeRequestBodyAsync(HttpContext context, Type bodyType)
        {
            context.Request.EnableBuffering();
            var stream = new StreamReader(context.Request.Body);
            var bodyString = await stream.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return JsonConvert.DeserializeObject(bodyString, bodyType);
        }
    }
}
