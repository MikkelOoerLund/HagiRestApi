using Azure;
using Azure.Core;
using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace HagiRestApi
{
    public class ValidateUserName : IAsyncActionFilter
    {
        private UserRepository _userRepository;
        private readonly string _letterNumberRegex;

        public ValidateUserName(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _letterNumberRegex = "^[a-zA-Z0-9]+$";
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var userAuthentication = ActionExecuteAssert.NotNull<UserAuthenticationDTO>(context);
            var userName = userAuthentication.UserName;

            var isUserNameValid = Regex.IsMatch(userName, _letterNumberRegex);

            if (isUserNameValid == false)
            {
                var result = new BadRequestObjectResult("Username may only contain letters and numbers");
                context.Result = result;
                return;
            }

            var isUserNameTaken = await _userRepository.HasUserWithNameAsync(userName);

            if (isUserNameTaken)
            {
                var result = new BadRequestObjectResult("Username is taken");
                context.Result = result;
                return;
            }


            var resultContext = await next();
        }
    }
}
