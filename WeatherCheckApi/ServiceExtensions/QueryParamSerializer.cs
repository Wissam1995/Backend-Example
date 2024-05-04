using RestEase;
using System.Collections;
using System.Reflection;

namespace WeatherCheckApi.Extensions
{
    public class QueryParamSerializer : RequestQueryParamSerializer
    {
        public override IEnumerable<KeyValuePair<string, string>> SerializeQueryParam<T>(string name, T value, RequestQueryParamSerializerInfo info)
        {
            return Serialize(name, value, info);
        }

        public override IEnumerable<KeyValuePair<string, string>> SerializeQueryCollectionParam<T>(string name, IEnumerable<T> values, RequestQueryParamSerializerInfo info)
        {
            return Serialize(name, values, info);
        }

        private IEnumerable<KeyValuePair<string, string>> Serialize<T>(string name, T value, RequestQueryParamSerializerInfo info)
        {
            if (value == null)
            {
                yield break;
            }

            foreach (KeyValuePair<string, object> item in GetPropertiesDeepRecursive(value, name))
            {
                if (item.Value == null)
                {
                    yield return new KeyValuePair<string, string>(item.Key, string.Empty);
                    continue;
                }

                object value2 = item.Value;
                if (value2 is DateTime)
                {
                    yield return new KeyValuePair<string, string>(value: ((DateTime)value2).ToString(info.Format ?? "o"), key: item.Key);
                }
                else
                {
                    yield return new KeyValuePair<string, string>(item.Key, item.Value.ToString());
                }
            }
        }

        private Dictionary<string, object> GetPropertiesDeepRecursive(object obj, string name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (obj == null)
            {
                dictionary.Add(name, null);
                return dictionary;
            }

            if (obj.GetType().IsValueType || obj is string)
            {
                dictionary.Add(name, obj);
                return dictionary;
            }

            IEnumerable enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                int num = 0;
                {
                    foreach (object item in enumerable)
                    {
                        dictionary = dictionary.Concat(GetPropertiesDeepRecursive(item, $"{name}[{num++}]")).ToDictionary((KeyValuePair<string, object> e) => e.Key, (KeyValuePair<string, object> e) => e.Value);
                    }

                    return dictionary;
                }
            }

            PropertyInfo[] properties = obj.GetType().GetProperties();
            string empty = string.Empty;
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dictionary = dictionary.Concat(GetPropertiesDeepRecursive(propertyInfo.GetValue(obj, null), empty + propertyInfo.Name)).ToDictionary((KeyValuePair<string, object> e) => e.Key, (KeyValuePair<string, object> e) => e.Value);
            }

            return dictionary;
        }
    }
}