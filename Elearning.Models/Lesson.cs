using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "lesson")]
    public class Lesson: BaseModel
     {
        [PrimaryKey]
        public int LessonID { get; set; }

        [Required]
        public int? ChapterID { get; set; }

        public object Description { get; set; }

        [Length(500)]
        public string LinkSource { get; set; }

        [Length(500)]
        public string LinkVideo { get; set; }

        public int? SortOrder { get; set; }

        [Length(500), Required]
        public string Title { get; set; }

     }
}
