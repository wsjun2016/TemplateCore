using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BasisFrameWork.Extension;

namespace BasisFrameWork.Attribute {
    public class GreaterOrEqualAttribute : ValidationAttribute {
        public string PropertyName { get; }
        public GreaterOrEqualAttribute(string propertyName) {
            PropertyName = propertyName?.Trim() ?? "";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (PropertyName.IsNullOrEmpty())
                return new ValidationResult($"{nameof(GreaterOrEqualAttribute)}.{nameof(PropertyName)}参数不能为空值");

            //获取PropertyName的属性值
            var propertyInfo = validationContext.ObjectType.GetProperty(PropertyName);
            var propertyValue = propertyInfo?.GetValue(validationContext.ObjectInstance, null);
            //如果给定对比的值为null，则返回成功
            if (propertyValue == null) return ValidationResult.Success;

            var thisTypeCode = Convert.GetTypeCode(value);
            var thatTypeCode = Convert.GetTypeCode(propertyValue);
            if (thisTypeCode != thatTypeCode)
                return new ValidationResult($"{validationContext.ObjectType.Name}.{PropertyName}与{validationContext.ObjectType.Name}.{validationContext.DisplayName}的类型不匹配");

            dynamic thisValue = Convert.ChangeType(value, thisTypeCode);
            dynamic thatValue = Convert.ChangeType(propertyValue, thatTypeCode);
            //如果当前值大于或等于给定值，则返回成功
            if (thisValue >= thatValue) return ValidationResult.Success;
            return new ValidationResult($"{validationContext.ObjectType.Name}.{PropertyName}必须大于或等于{validationContext.ObjectType.Name}.{validationContext.DisplayName}");
        }
    }
}
