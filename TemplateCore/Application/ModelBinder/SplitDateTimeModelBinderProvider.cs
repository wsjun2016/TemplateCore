using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Application.ModelBinder {
    public class SplitDateTimeModelBinderProvider : IModelBinderProvider {
        private readonly IModelBinder binder = new SplitDateTimeModelBinder(new SimpleTypeModelBinder(typeof(DateTime)));
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            return context.Metadata.ModelType == typeof(DateTime) ? binder : null;
        }
    }
}
