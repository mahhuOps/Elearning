using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Middlewares
{
    public class HttpResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception != null)
            {
                if(context.Exception is CustomException customException)
                {
                    object result = customException.Data.Count == 0 ? customException.Message : customException.Data;
                    context.Result = new ObjectResult(result)
                    {
                        StatusCode = 400,
                    };

                    context.ExceptionHandled = true;
                }
                else
                {
                    var result = new
                    {
                        userMsg = "Có lỗi xảy ra vui lòng liên hệ kỹ thuật viên để được giúp đỡ",
                        devMsg = context.Exception.Message,
                        errorMsg = "",
                    };

                    context.Result = new ObjectResult(result)
                    {
                        StatusCode = 500
                    };

                    context.ExceptionHandled = true;
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
