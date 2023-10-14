using Microsoft.AspNetCore.Mvc.Filters;

namespace HagiRestApi
{
    public static class ActionExecuteAssert
    {
        public static T NotNull<T>(ActionExecutingContext context) where T : class
        {
            var type = typeof(T);
            var typeName = type.Name;

            var actionArguments = context.ActionArguments;
            var parameter = actionArguments[typeName] as T;
            return Assert.NotNull(parameter);
        }
    }
}
