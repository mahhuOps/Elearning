using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "course_session")]
    public class CourseSession: BaseModel
     {
        [PrimaryKey]
        public int CourseSessionID { get; set; }

        public string Content { get; set; }

        [Required]
        public int? CourseID { get; set; }

        [Length(500)]
        public string Image { get; set; }

        public int? SortOrder { get; set; }

        [Length(500)]
        public string Title { get; set; }

     }
}
