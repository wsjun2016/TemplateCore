using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BasisFrameWork.Attribute
{
    /// <summary>
    /// 时间不能为空
    /// </summary>
    public class DateTimeNotEmptyAttribute : RequiredAttribute {
        public override bool IsValid(object value) {
            return DateTime.TryParse(value?.ToString() ?? "", out DateTime date) && date != DateTime.MinValue && date != DateTime.MaxValue;
        }
    }

    
}
