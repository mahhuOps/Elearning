using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Common.Extension;
using System.Reflection;

namespace Common.Middlewares
{
    public class HttpResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value != null)
                {
                    if(objectResult.StatusCode == 200)
                    {
                        var originalValue = objectResult.Value;

                        if(originalValue != null && originalValue.GetValue("Data") != null) {
                            var data = originalValue.GetValue("Data");
                            SetModelDetailConfigsToNull(data);
                        }

                        objectResult.Value = originalValue;
                    }
                }
            }

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

        private void SetModelDetailConfigsToNull(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var type = obj.GetType();

            if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime))
            {
                return;
            }

            if (obj is System.Collections.IEnumerable enumerable)
            {
                foreach (var element in enumerable)
                {
                    SetModelDetailConfigsToNull(element);
                }
                return;
            }

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "ModelDetailConfigs")
                {
                    property.SetValue(obj, null);
                }
                else
                {
                    var propertyValue = property.GetValue(obj);
                    SetModelDetailConfigsToNull(propertyValue);
                }
            }
        }
    }
}
