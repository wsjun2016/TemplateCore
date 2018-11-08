using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.ModelBinder {
    public class SplitDateTimeModelBinder : IModelBinder
    {
        private readonly IModelBinder fallbackBinder;

        public SplitDateTimeModelBinder(IModelBinder binder) {
            this.fallbackBinder = binder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext) {
            var datepartName = $"{bindingContext.ModelName}.Date";
            var timepartName = $"{bindingContext.ModelName}.Time";
            var datepartValue = bindingContext.ValueProvider.GetValue(datepartName);
            var timepartValue = bindingContext.ValueProvider.GetValue(timepartName);
            if (datepartValue.Length == 0 || timepartValue.Length == 0) //return Task.CompletedTask;
                return fallbackBinder.BindModelAsync(bindingContext);

            DateTime.TryParse(datepartValue.FirstValue, out var parsedDateValue);
            DateTime.TryParse(timepartValue.FirstValue,  out var parsedTimeValue);

            var result = new DateTime(parsedDateValue.Year, parsedDateValue.Month, parsedDateValue.Day, parsedTimeValue.Hour, parsedTimeValue.Minute, parsedTimeValue.Second);
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result, $"{datepartValue.FirstValue} {timepartValue.FirstValue}");
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
