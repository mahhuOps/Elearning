using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ConvertUtility
    {
        public static readonly DateFormatHandling JSONDateFormatHandling = DateFormatHandling.IsoDateFormat;
        public static readonly DateTimeZoneHandling JSONDateTimeZoneHandling = DateTimeZoneHandling.Local;
        public static readonly string JSONDateFormatString = "yyyy'-'MM'-'dd'T'HH':''mm':'ss.fffK'";
        public static readonly NullValueHandling JSONNullValueHandling = NullValueHandling.Include;
        public static readonly ReferenceLoopHandling JSONReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        private static Dictionary<string, JsonSerializerSettings> DicJsonSerializerSettings = new Dictionary<string, JsonSerializerSettings>();

        public static JsonSerializerSettings GetJsonSerializerSetting(bool ignoreNullValue = false, bool excludeBaseProperties = false, bool ignoreJsonIgnoreAttribute = false)
        {
            var key = $"{ignoreNullValue}_{excludeBaseProperties}_{ignoreJsonIgnoreAttribute}";
            if (!DicJsonSerializerSettings.ContainsKey(key))
            {
                var settings = new JsonSerializerSettings()
                {
                    DateFormatHandling = JSONDateFormatHandling,
                    DateTimeZoneHandling = JSONDateTimeZoneHandling,
                    DateFormatString = JSONDateFormatString,
                    NullValueHandling = JSONNullValueHandling,
                    ReferenceLoopHandling = JSONReferenceLoopHandling
                };

                if (ignoreNullValue)
                {
                    settings.NullValueHandling = NullValueHandling.Ignore;
                }

                if (excludeBaseProperties || ignoreJsonIgnoreAttribute)
                {
                    settings.ContractResolver = CustomContractResolver.Instance(excludeBaseProperties, ignoreJsonIgnoreAttribute);
                }

                DicJsonSerializerSettings.TryAdd(key, settings);
            }

            return DicJsonSerializerSettings[key];
        }

        public static string Serialize(object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings ?? GetJsonSerializerSetting());
        }

        public static string Serialize(object obj,bool ignoreNullValue = false, bool excludeBaseProperties = false, bool ignoreJsonIgnoreAttribute = false)
        {
            return JsonConvert.SerializeObject(obj, GetJsonSerializerSetting(ignoreNullValue, excludeBaseProperties, ignoreJsonIgnoreAttribute));
        }

        public static T Deserialize<T>(string json)
        {
            return Deserialize<T>(json, false);
        }

        public static T Deserialize<T>(string json, bool ignoreJsonIgnoreAttribute)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSetting(ignoreJsonIgnoreAttribute: ignoreJsonIgnoreAttribute));
            }
            catch
            {
                if(typeof(T) == typeof(string))
                {
                    return (T)((object)json);
                }
                throw;
            }
        }

        public static object DeserializeObject(string json, Type type)
        {
            return DeserializeObject(json, type, false);
        }

        public static object DeserializeObject(string json, Type type, bool ignoreJsonIgnoreAttribute)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, type,GetJsonSerializerSetting(ignoreJsonIgnoreAttribute: ignoreJsonIgnoreAttribute));
            }
            catch
            {
                if (type == typeof(string))
                {
                    return json;
                }
                throw;
            }
        }

        private class CustomContractResolver : DefaultContractResolver
        {
            public static CustomContractResolver Instance(bool excludeBaseProperties, bool ignoreJsonIgnoreAttribute)
            {
                return new CustomContractResolver()
                {
                    ExcludeBaseProperties = excludeBaseProperties,
                    IgnoreJsonIgnoreAttribute = ignoreJsonIgnoreAttribute
                };
            }

            public bool ExcludeBaseProperties { get; set; }
            public bool IgnoreJsonIgnoreAttribute { get; set; }

            private string[] excludeProperties = { "CreatedDate", "CreatedBy", "ModifiedDate", "ModifiedBy" };

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);
                if(this.ExcludeBaseProperties && excludeProperties.Any(c => c.Equals(property.PropertyName, StringComparison.OrdinalIgnoreCase)))
                {
                    property.ShouldSerialize = c => { return false; };
                }

                if (this.IgnoreJsonIgnoreAttribute)
                {
                    property.Ignored = false;
                }

                return property;
            }
        }
    }
}
