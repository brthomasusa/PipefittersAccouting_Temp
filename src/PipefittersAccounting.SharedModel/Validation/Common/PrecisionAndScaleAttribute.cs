#pragma warning disable CS8620 
#pragma warning disable CS8603 

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.Validation.Common
{
    public class PrecisionAndScaleAttribute : RegularExpressionAttribute
    {
        public PrecisionAndScaleAttribute(int precision, int scale) : base($@"^(0|-?\d{{0,{precision - scale}}}(\.\d{{0,{scale}}})?)$")
        {

        }
    }
}