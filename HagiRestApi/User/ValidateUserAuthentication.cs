
using Azure;
using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace HagiRestApi
{
    public class ValidateUserAuthentication : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userAuthentication = ActionExecuteAssert.NotNull<UserAuthenticationDTO>(context);

            var emptyString = string.Empty;
            var response = emptyString;

            if (userAuthentication.Salt == emptyString) response = "Salt can't be empty";
            if (userAuthentication.UserName == emptyString) response = "UserName can't be empty";
            if (userAuthentication.HashPassword == emptyString) response = "HashPassword can't be empty";

            if (response == emptyString) return;

            var result = new BadRequestObjectResult(response);
            context.Result = result;
        }
    }
}
