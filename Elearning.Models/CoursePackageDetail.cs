using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "course_package_detail")]
    public class CoursePackageDetail: BaseModel
     {
        [PrimaryKey]
        public int CoursePackageDetailID { get; set; }

        [Required]
        public int? CourseID { get; set; }

        public decimal PurchasePrice { get; set; }

        [Required]
        public int? UserID { get; set; }

     }
}
