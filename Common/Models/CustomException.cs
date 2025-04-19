using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CustomException : Exception
    {
        #region contructor
        Dictionary<string, object> FSData = new Dictionary<string, object>();
        string CustomMessage = string.Empty;
        public CustomException(Dictionary<string, object> data)
        {
            FSData = data;
        }

        public CustomException(string customMessage)
        {
            CustomMessage = customMessage;

        }
        #endregion

        public override IDictionary Data
        {
            get
            {
                return FSData;
            }
        }

        public override string Message
        {
            get
            {
                return CustomMessage;
            }
        }
    }
}
