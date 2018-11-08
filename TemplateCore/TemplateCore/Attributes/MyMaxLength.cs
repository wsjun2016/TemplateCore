using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateCore.Attributes {
    public class MyMaxLengthAttribute:ValidationAttribute {
        public int MaxLength { get; private set; }

        public MyMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return value.ToString().Length <= MaxLength;
        }
    }
}
