
//using Castle.Core.Logging;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Diagnostics;

//namespace WebApplication.ActionFilters
//{
//    public class ActionFilter : ActionFilterAttribute
//    {
//        private readonly ILogger _logger;

//        public ActionFilter(ILoggerFactory loggerFactory)
//        {
//            _logger = loggerFactory.Create("ValidatePayloadTypeFilter");
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var commandDto = context.ActionArguments["commandDto"] as CommandDto;
//            if (commandDto == null)
//            {
//                context.HttpContext.Response.StatusCode = 400;
//                context.Result = new ContentResult()
//                {
//                    Content = "The body is not a CommandDto type"
//                };
//                return;
//            }

//            _logger.LogDebug("validating CommandType");
//            if (!CommandTypes.AllowedTypes.Contains(commandDto.CommandType))
//            {
//                context.HttpContext.Response.StatusCode = 400;
//                context.Result = new ContentResult()
//                {
//                    Content = "CommandTypes not allowed"
//                };
//                return;
//            }

//            _logger.LogDebug("validating PayloadType");
//            if (!PayloadTypes.AllowedTypes.Contains(commandDto.PayloadType))
//            {
//                context.HttpContext.Response.StatusCode = 400;
//                context.Result = new ContentResult()
//                {
//                    Content = "PayloadType not allowed"
//                };
//                return;
//            }

//            base.OnActionExecuting(context);
//        }
//    }
//}
