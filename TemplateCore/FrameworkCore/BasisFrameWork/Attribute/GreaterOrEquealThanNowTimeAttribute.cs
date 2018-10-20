using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BasisFrameWork.Attribute
{
    /// <summary>
    /// 时间必须大于或等于当前时间
    /// </summary>
    public class GreaterOrEquealThanNowTimeAttribute : RequiredAttribute {
        public override bool IsValid(object value) {
            return DateTime.TryParse(value?.ToString() ?? "", out DateTime date) && date != DateTime.MinValue && date != DateTime.MaxValue && date >= DateTime.Now;
        }
    }
}
