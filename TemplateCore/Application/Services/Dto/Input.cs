using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BasisFrameWork.Attribute;
using BasisFrameWork.Extension;

namespace Application.Services.Dto {

    public class NotSameAttribute : ValidationAttribute
    {
        public string PropertyName { get; set; }

        public NotSameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //选出需要对比的值
            var propertyInfo = validationContext.ObjectType.GetProperty(PropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            dynamic value1 = Convert.ChangeType(value, Convert.GetTypeCode(value));
            dynamic value2 = Convert.ChangeType(propertyValue, Convert.GetTypeCode(propertyValue));
            if (value1 != value2)
                return ValidationResult.Success;
            return new ValidationResult($"{PropertyName}不能与{validationContext.DisplayName}相同！");
        }
    }

    //大于或等于的特性验证
    public class GreaterOrEqualAttribute : ValidationAttribute
    {
        //需要被对比的属性名称
        public string PropertyName { get;}
        public GreaterOrEqualAttribute(string propertyName) {
            PropertyName = propertyName?.Trim()??"";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(PropertyName.IsNullOrEmpty())
                return new ValidationResult($"{nameof(GreaterOrEqualAttribute)}.{nameof(PropertyName)}参数不能为空值");

            //获取PropertyName的属性值
            var propertyInfo = validationContext.ObjectType.GetProperty(PropertyName);
            var propertyValue = propertyInfo?.GetValue(validationContext.ObjectInstance, null);
            //如果给定对比的值为null，则返回成功
            if(propertyValue==null) return ValidationResult.Success;

            var thisTypeCode = Convert.GetTypeCode(value);
            var thatTypeCode = Convert.GetTypeCode(propertyValue);
            if(thisTypeCode!=thatTypeCode)
                return new ValidationResult($"{validationContext.ObjectType.Name}.{PropertyName}与{validationContext.ObjectType.Name}.{validationContext.DisplayName}的类型不匹配");

            dynamic thisValue = Convert.ChangeType(value, thisTypeCode);
            dynamic thatValue = Convert.ChangeType(propertyValue, thatTypeCode);
            //如果当前值大于或等于给定值，则返回成功
            if(thisValue>=thatValue) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage.IsNullOrEmpty()? $"{validationContext.ObjectType.Name}.{PropertyName}必须大于或等于{validationContext.ObjectType.Name}.{validationContext.DisplayName}": ErrorMessage);
        }
    }
    



    public class InputDto
    {
        //起始时间
        public DateTime? BeginDate { get; set; } = DateTime.Now;

        //结束时间
        [GreaterOrEqual(nameof(BeginDate))]
        public DateTime? EndDate { get; set; } = DateTime.Now;
    }
}
