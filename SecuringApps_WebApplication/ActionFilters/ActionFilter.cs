
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.Application.Interfaces;
using System;

namespace WebApplication.ActionFilters
{
    public class ActionFilter : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                ValidateStudentView(context);
            }
            catch (Exception example)
            {
                context.Result = new BadRequestObjectResult("Bad Request = "+example.Message);
            }
        }

        private static void ValidateStudentView(ActionExecutingContext ctxt)
        {
            byte[] encodedId = Convert.FromBase64String(ctxt.ActionArguments["id"].ToString());
            var decodedId = new Guid(System.Text.Encoding.UTF8.GetString(encodedId));

            var currentLoggedUser = ctxt.HttpContext.User.Identity.Name;

            IStudentAssignmentsService studentsService = (IStudentAssignmentsService)ctxt.HttpContext.RequestServices.GetService(typeof(IStudentAssignmentsService));
            var studentAssignment = studentsService.GetStudentAssignment(decodedId);

            if (studentAssignment.Student.Email == currentLoggedUser || studentAssignment.Student.Teacher.Email == currentLoggedUser)
            {
                //log to file
            }
            else
            {  // Log to file
                ctxt.Result = new UnauthorizedObjectResult("Access Denied");
            }

        }
    }
}
