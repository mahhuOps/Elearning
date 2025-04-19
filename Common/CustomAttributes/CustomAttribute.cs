using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMap : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Email : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Phone : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Length : Attribute
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; }

        public Length(int maxLength = 255, int minLength = 0)
        {
            MaxLength = maxLength;
            MinLength = minLength;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Detail : Attribute
    {
        public string CommandGetDetail { get; set; }
        public string PropertyInMaster { get; set; }

        public Type Type { get; set; }

        public Detail(string commandGetDetail, string propertyInMaster, Type type)
        {
            CommandGetDetail = commandGetDetail;
            PropertyInMaster = propertyInMaster;
            Type = type;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AllowUpdateSingle : Attribute
    {

    }
}
