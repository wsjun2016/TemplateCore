using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BasisFrameWork.Attribute
{
    /// <summary>
    /// 限定为去年及之前的年份
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LastYearAgoAttribute : ValidationAttribute {

        public LastYearAgoAttribute() {
            // this.ErrorMessageResourceName
        }

        public LastYearAgoAttribute(string errorMessage) {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null && int.TryParse(value.ToString(), out int year)) {
                int lastYear = DateTime.Now.Year - 1;
                if (year > lastYear)
                    return new ValidationResult($"{validationContext.MemberName}参数值只限定于{lastYear}年及之前的年份！");
            }

            return ValidationResult.Success;
        }


    }
}
