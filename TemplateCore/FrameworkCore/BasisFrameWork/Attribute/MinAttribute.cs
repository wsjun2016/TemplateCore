using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BasisFrameWork.Attribute
{
    /// <summary>
    /// 限定最小值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinAttribute : ValidationAttribute {


        public MinAttribute(double minimum) {
            Minimum = minimum;
        }
        public MinAttribute(int minimum) {
            Minimum = minimum;
        }

        public MinAttribute() {
            Minimum = 0;
        }

        public object Minimum { get; }

        public override bool IsValid(object value) {
            if (value != null && double.TryParse(Minimum.ToString(), out double dMinimum)) {
                if (double.TryParse(value.ToString(), out double dValue))
                    return dValue >= dMinimum;

                if (value.GetType().IsEnum) {
                    dValue = (int)value;
                    return dValue >= dMinimum;
                }
            }

            return false;
        }
    }
}
