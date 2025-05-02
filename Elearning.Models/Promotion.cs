using Common.CustomAttributes;
using Common.Models;
using Elearning.Common.Enum;

namespace Elearning.Models
{
    [ConfigTable(tableName: "promotion")]
    public class Promotion: BaseModel
     {
        [PrimaryKey]
        public int PromotionID { get; set; }

        [Required]
        public PromotionType PromotionType { get; set; }

        [Required]
        public decimal PromotionValue { get; set; }

        [Required]
        public DateTime? ExpireDate { get; set; }

        public int? Amount { get; set; }

        public bool? IsActive { get; set; }

    }
}
