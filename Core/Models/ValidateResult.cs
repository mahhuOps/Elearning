using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ValidateResult
    {
        public ErrorCodeEnum ErrorCode { get; set; }

        public string FieldError { get; set; }

        public object FieldValue { get; set; }
    }
}
