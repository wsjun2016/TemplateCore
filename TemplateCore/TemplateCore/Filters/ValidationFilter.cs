using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasisFrameWork.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TemplateCore.Filters {
    public class ValidationFilter:ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = context.ModelState.Values.Take(1).SingleOrDefault();
                context.Result = new ContentResult
                {
                    Content = message?.Errors.Where(it => !it.ErrorMessage.IsNullOrEmpty()).Take(1).SingleOrDefault()
                                  ?.ErrorMessage ?? message.Errors.LastOrDefault()?.Exception.Message,
                    ContentType = null
                };
                   ;
            }
            base.OnActionExecuting(context);
        }
    }
}
