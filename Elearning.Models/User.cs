using Common.CustomAttributes;
using Common.Models;

namespace Elearning.Models
{
    [ConfigTable(tableName: "users")]
    public class User: BaseModel
     {
        [PrimaryKey]
        public int UserID { get; set; }

        [Length(500)]
        public string Avatar { get; set; }

        [Length(255), Email]
        public string Email { get; set; }

        [Length(50), Required]
        public string FirebaseID { get; set; }

        [Length(255)]
        public string FullName { get; set; }

        public bool? IsAdmin { get; set; }

        [Length(20), Phone]
        public string PhoneNumber { get; set; }
     }
}
