using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Touba.WebApis.Api
{
    public static class Extentions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            Guid userid;
            Guid.TryParse( principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userid);
            return userid;
        }

        /// <summary>
        /// Converts an object to a Redis's HashEntry array
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>HashEntry array that represents the object</returns>
        public static HashEntry[] ToHashEntries(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
            .Where(x => x.GetValue(obj) != null) // prevent NullReferenceException
            .Select
            (
                property =>
                {
                    try 
                    {
                        object propertyValue = property.GetValue(obj);
                        string hashValue;

                        // If it has a nested object and the nested object has the same parent object then skip
                        if (propertyValue.GetType().ToString().Equals(obj.GetType().ToString())) hashValue = "";

                        // Detect if given property value is enumerable
                        if (propertyValue is IEnumerable<object> || (propertyValue is object && propertyValue is not string))
                        {
                            // Serialize the property value as JSON
                            hashValue = JsonConvert.SerializeObject(propertyValue);
                        }
                        else
                        {
                            hashValue = propertyValue.ToString();
                        }

                        return new HashEntry(property.Name, hashValue);

                    }
                    catch (Exception Ex)
                    {
                        return new HashEntry(property.Name, "");
                    }
                    
                }
            )
            .ToArray();
        }

        /// <summary>
        /// Converts an array of Redis's HashEntry to a regular object/POCO
        /// </summary>
        /// <param name="hashEntries">Redis Hash Entries</param>
        /// <typeparam name="T">Generic object type T</typeparam>
        /// <returns>The object/POCO</returns>
        public static T ConvertFromRedis<T>(HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry())) continue;

                Type propertyType = property.PropertyType;
                Type nullableType = Nullable.GetUnderlyingType(propertyType);
                var isNullable = nullableType != null;
                var isPrimitive = (isNullable ? nullableType.IsPrimitive : propertyType.IsPrimitive) || (long.TryParse(entry.Value, out long n)) || propertyType == typeof(Decimal);
                var isClassObject = (!isPrimitive && propertyType != typeof(String) && propertyType != typeof(DateTime));

                if (isClassObject) {
                    var jsonParse = typeof(Extentions).GetMethod("TryParseJson").MakeGenericMethod(propertyType);
                    var classObject = jsonParse.Invoke(null, new object[] { entry.Value.ToString() });
                    if (classObject != null) property.SetValue(obj, classObject);
                }
                else property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), nullableType ?? propertyType));
            }
            return (T) obj;
        }

        /// <summary>
        /// Attempts to deserialize json to an object, and return the deserialzed object or null if deserialization fails
        /// </summary>
        /// <param name="json">The object's json</param>
        /// <typeparam name="T">The object's type</typeparam>
        /// <returns>Deserialized object</returns>
        public static object TryParseJson<T>(string json)
        {
            bool success = true;
            var deserializationResult = default(T);

            var settings = new JsonSerializerSettings {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };

            try {
                deserializationResult = JsonConvert.DeserializeObject<T>(json, settings);
            } catch (Exception ex) {
                success = false;
            }

            return (success ? deserializationResult : null);
        }

        /// <summary>
        /// Gets an enum description attribute value
        /// </summary>
        /// <param name="val">The Enum</param>
        /// <returns>Description string</returns>
        public static string Description(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[]) val
            .GetType()
            .GetField(val.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
