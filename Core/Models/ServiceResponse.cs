using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ServiceResponse
    {
        public ErrorCodeEnum ErrorCode { get; set; }

        public bool Success { get; set; }

        public object Data { get; set; }

        public List<ValidateResult> ValidateInfo { get; set; }

        public ServiceResponse()
        {
            Success = true;
            ValidateInfo = new List<ValidateResult>();
        }
    }
}
