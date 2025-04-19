using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "course_package")]
    public class CoursePackage: BaseModel
     {
        [PrimaryKey]
        public int CoursePackageID { get; set; }

        [Required, Length(500)]
        public string Title { get; set; }

        [Required]
        public int? CourseID { get; set; }

        [Required]
        public decimal Price { get; set; }

        [NotMap]
        [Detail("SELECT * FROM service s WHERE s.CoursePackageID = @MasterID;", "Services", typeof(List<Service>))]
        public List<Service> Services { get; set; }

        public CoursePackage()
        {
            var modelDetailConfigs = new List<ModelDetailConfig>();
            modelDetailConfigs.Add(new ModelDetailConfig("service", "CoursePackageID", "Services"));
            this.ModelDetailConfigs = modelDetailConfigs;
        }

    }
}
