using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "chapter")]
    public class Chapter: BaseModel
     {
        [PrimaryKey]
        public int ChapterID { get; set; }

        [Required]
        public int? CourseID { get; set; }

        [Length(500)]
        public string CoverImage { get; set; }

        public string Description { get; set; }

        [Length(500), Required]
        public string Title { get; set; }

        public int? SortOrder { get; set; }

        [NotMap]
        [Detail("SELECT * FROM lesson l WHERE l.ChapterID = @MasterID;", "Lessones", typeof(List<Lesson>))]
        public List<Lesson> Lessones { get; set; }

        public Chapter()
        {
            var modelDetailConfigs = new List<ModelDetailConfig>();
            modelDetailConfigs.Add(new ModelDetailConfig("lesson", "ChapterID", "Lessones"));
            this.ModelDetailConfigs = modelDetailConfigs;
        }

    }
}
