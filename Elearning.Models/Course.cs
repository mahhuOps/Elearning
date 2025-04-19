using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "course")]
    public class Course: BaseModel
     {
        [PrimaryKey]
        public int CourseID { get; set; }

        [Length(255), Required]
        public string CourseName { get; set; }

        public string Description { get; set; }

        [Length(500)]
        public string Image { get; set; }

        [NotMap]
        [Detail("SELECT * FROM course_session cs WHERE cs.CourseID = @MasterID;", "CourseSessiones", typeof(List<CourseSession>))]
        public List<CourseSession> CourseSessiones { get; set; }

        [NotMap]
        [Detail("SELECT * FROM course_package cp WHERE cp.CourseID = @MasterID;", "CoursePackages", typeof(List<CoursePackage>))]
        public List<CoursePackage> CoursePackages { get; set; }

        [NotMap]
        [Detail("SELECT * FROM chapter c WHERE c.CourseID = @MasterID;", "Chapteres", typeof(List<Chapter>))]
        public List<Chapter> Chapteres { get; set; }

        public Course()
        {
            var modelDetailConfigs = new List<ModelDetailConfig>();
            modelDetailConfigs.Add(new ModelDetailConfig("course_session", "CourseID", "CourseSessiones"));
            modelDetailConfigs.Add(new ModelDetailConfig("course_packages", "CourseID", "CoursePackages"));
            modelDetailConfigs.Add(new ModelDetailConfig("chapter", "CourseID", "Chapteres"));
            this.ModelDetailConfigs = modelDetailConfigs;
        }
    }
}
