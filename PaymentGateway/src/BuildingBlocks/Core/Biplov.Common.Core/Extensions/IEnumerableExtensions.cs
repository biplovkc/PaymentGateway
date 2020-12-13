using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Biplov.Common.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Select user desired field from source collection based on given source collection.
        /// </summary>
        /// <typeparam name="Tsource"></typeparam>
        /// <param name="source">IEnumerable collection</param>
        /// <param name="fields">fields to be selected from source object</param>
        /// <returns>expando object collection with based on fields</returns>
        public static IEnumerable<ExpandoObject> ShapeData<Tsource>(
            this IEnumerable<Tsource> source, string fields)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(fields));

            // create a list to hold our ExpnadoObjects
            var expandoObjectList = new List<ExpandoObject>();

            // create a list with PropertyInfo objects on TSource.
            // Reflection is expensive, so rather than doing it 
            // for each object on the list, we do it once and reuse results.
            // After all, part of the reflection is on the type of the object (TSource), 
            // not the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(Tsource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                // the fields should be separated by ',', so split it accordingly
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(Tsource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase |
                                BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo is null)
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(Tsource)}");

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (var sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
