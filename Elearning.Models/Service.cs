using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "service")]
    public class Service: BaseModel
     {
        [PrimaryKey]
        public int ServiceID { get; set; }

        [Required]
        public int? CoursePackageID { get; set; }

        public bool IsActive { get; set; }

        [Length(500), Required]
        public string ServiceName { get; set; }

     }
}
