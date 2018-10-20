using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BasisFrameWork.Attribute
{
    /// <summary>
    /// 限定string字符串不能为空
    /// </summary>
    public class StringNotEmptyAttribute : RequiredAttribute {
        public override bool IsValid(object value) {
            return !string.IsNullOrWhiteSpace(value?.ToString()?.Trim());
        }
    }
}
