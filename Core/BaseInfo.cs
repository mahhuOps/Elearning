using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class BaseInfo
    {
        public static string UserName { get; set; }
        public static int UserID { get; set; }

        /// <summary>
        /// email từ firebase
        /// </summary>
        public static string email { get; set; }
        /// <summary>
        /// name từ firebase
        /// </summary>
        public static string name{ get; set; }
        public static string firebaseid { get; set; }
        public static string token { get; set; }
        public static bool? IsAdmin { get; set; }
    }
}
