using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateCore.Attributes;
using TemplateCore.ModelBinder;

namespace TemplateCore.Model {
    public class GetFormDataInput {
        [ModelBinder(typeof(TrimModelBinder))]
        public string Name { get; set; }


        [ModelBinder(typeof(TrimModelBinder))]
        [NoTrim]
        [MyMaxLength(3, ErrorMessage = "最大长度不能超过3个字符！")]
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
