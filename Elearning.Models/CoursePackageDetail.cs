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

        [Email,Required]
        public string ReceiverEmail { get; set; }

        public string History { get; set; }

        [Required]
        public string OrderCode { get; set; }

        [Required]
        public int? CoursePackageID { get; set; }

        [Required]
        public int? NextCoursePackageID { get; set; }

        public decimal? Price { get; set; }
        public decimal? NextPrice { get; set; }
        public decimal? NextPurchasePrice { get; set; }
        public Guid? ReferrerID { get; set; }

        public string Password { get; set; }
    }
}
