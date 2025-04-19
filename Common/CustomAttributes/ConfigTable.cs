using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class ConfigTable : Attribute
    {
        public string TableName { get; set; }

        public bool IsMaster { get; set; }

        public List<string> DetailTables { get; set; }

        public List<string> ColumnSearch { get; set; }

        public ConfigTable(string tableName = "", bool isMaster = false, string detailTables = "", string columnSearch = "")
        {
            TableName = tableName;
            IsMaster = isMaster;

            if (!string.IsNullOrWhiteSpace(detailTables))
            {
                DetailTables = detailTables.Split(";").ToList();
            }

            if (!string.IsNullOrEmpty(columnSearch))
            {
                ColumnSearch = columnSearch.Split(";").ToList();
            }
        }
    }
}
