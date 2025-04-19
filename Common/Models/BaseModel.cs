using Common.CustomAttributes;
using DatabaseService.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Models
{
    public class BaseModel
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMap]
        public ModelStateEnum State { get; set; }
        [NotMap]
        public List<ModelDetailConfig>? ModelDetailConfigs { get; set; }
    }
}
