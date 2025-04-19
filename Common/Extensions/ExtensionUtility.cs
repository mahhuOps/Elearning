using Common.CustomAttributes;
using Common.Extensions;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Common.Extension
{
    public static class ExtensionUtility
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(obj));
        }

        public static object Get(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return null;
            }
        }

        public static T Get<T>(this Dictionary<string, object> dictionary, string key)
        {
            T value = default(T);
            if (dictionary.ContainsKey(key) && dictionary[key] != null)
            {
                value = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(dictionary[key]));
            }

            return value;
        }

        public static object GetValue(this object obj, string key)
        {
            if (obj == null || string.IsNullOrEmpty(key)) return null;

            PropertyInfo propertyInfo = obj.GetType().GetProperty(key);

            if (propertyInfo != null) return propertyInfo.GetValue(obj);

            return obj.ToDictionary().Get(key);
        }

        public static T GetValue<T>(this object obj, string key)
        {
            T value = default(T);

            if (obj == null || string.IsNullOrEmpty(key)) return value;

            PropertyInfo propertyInfo = obj.GetType().GetProperty(key);

            if (propertyInfo != null)
            {
                object valueInfo = propertyInfo.GetValue(obj);

                if(valueInfo != null)
                {
                    value = (T)valueInfo;
                }
            }
            else
            {
                value = obj.ToDictionary().Get<T>(key);
            }

            return value;
        }

        public static void SetValue(this object obj, string key, object value)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(key,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty);
        
            if(propertyInfo != null)
            {
                Type type = propertyInfo.PropertyType;
                if(!object.Equals(value,DBNull.Value) && propertyInfo.CanWrite)
                {
                    if(value != null)
                    {
                        propertyInfo.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type),null);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, null, null);
                    }
                }
            }
        }

        public static Attribute GetCustomAttribute(this Type type,Type typeAttribute)
        {
            var attribute = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeAttribute)).FirstOrDefault().GetCustomAttribute(typeAttribute, true);

            if (attribute != null) return attribute;
            else return null;
        }
        public static List<Attribute> GetCustomAttributes(this Type type, Type typeAttribute)
        {
            var list = new List<Attribute>();
            var propertyInfos = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeAttribute));

            if(propertyInfos != null && propertyInfos.Count() > 0) {
                foreach(var propertyInfo in propertyInfos)
                {
                    var attribute = propertyInfo.GetCustomAttribute(typeAttribute);
                    list.Add(attribute);
                }
            }

            return list;
        }

        public static List<PropertyInfo> GetProperties(this Type type, Type typeAttribute)
        {
            var list = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeAttribute)).ToList();
            return list;
        }

        public static PropertyInfo GetProperty(this Type type,Type typeAttribute)
        {
            var list = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeAttribute)).FirstOrDefault();
            return list;
        }

        public static List<string> GetAllFieldName(this Type type)
        {
            var list = type.GetProperties().Select(property => property.Name).ToList();
            return list;
        }

        public static List<T> ToList<T>(this DataTable dataTable) where T : class
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = row.ToItem<T>();
                data.Add(item);
            }
            return data;
        }

        public static T ToItem<T>(this DataRow dataRow) where T : class
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        obj.SetValue(pro.Name, dataRow[column.ColumnName]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        public static bool ValidateEmail(this string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePhone(this string phone)
        {
            return Regex.Match(phone, @"^(\+[0-9]{9})$").Success;
        }

        public static int GetPrimaryKey<Entity>(this Entity entity)
        {
            var primaryKeyProp = entity?.GetType().GetProperty(typeof(PrimaryKey));
            var primaryKeyField = primaryKeyProp?.Name ?? "";

            return entity?.GetValue<int>(primaryKeyField) ?? 0;
        }

        public static int GetPrimaryKey<Entity>(this Entity entity, Type type)
        {
            var primaryKeyProp = type.GetProperty(typeof(PrimaryKey));
            var primaryKeyField = primaryKeyProp?.Name ?? "";

            return entity?.GetValue<int>(primaryKeyField) ?? 0;
        }

        public static void SetPrimaryKey<Entity>(this Entity entity, int id)
        {
            var primaryKeyProp = entity?.GetType().GetProperty(typeof(PrimaryKey));
            primaryKeyProp?.SetValue(entity, id);
        }
    }
}
