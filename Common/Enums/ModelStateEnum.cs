using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseService.Enums
{
    public enum ModelStateEnum
    {
        /// <summary>
        /// No Action
        /// </summary>
        None = 0,
        /// <summary>
        /// Thêm
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Sửa
        /// </summary>
        Update = 2,
        /// <summary>
        /// Xóa
        /// </summary>
        Delete = 3,
        /// <summary>
        /// Nhân bản
        /// </summary>
        Duplicate = 4
    }
}
