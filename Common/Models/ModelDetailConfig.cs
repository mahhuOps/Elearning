using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ModelDetailConfig
    {
        public string DetailTableName { get; set; }

        public string ForeignKeyName { get; set; }

        public string PropertyOnMasterModel { get; set; }

        public ModelDetailConfig(string detailTableName, string foreignKeyName, string propertyOnMasterModel) { 
            this.DetailTableName = detailTableName;
            this.ForeignKeyName = foreignKeyName;
            this.PropertyOnMasterModel = propertyOnMasterModel;
        }
    }
}
