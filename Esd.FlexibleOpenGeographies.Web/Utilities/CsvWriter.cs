using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;

namespace Esd.FlexibleOpenGeographies.Web.Utilities
{
    public class CsvExport<T> where T : class
    {
        public IEnumerable<T> Objects;

        public CsvExport(IEnumerable<T> objects)
        {
            Objects = objects;
        }

        public string Export()
        {
            return Export(true);
        }

        public string Export(bool includeHeaderLine)
        {
            var sb = new StringBuilder();
            //Get properties using reflection.
            IList<PropertyInfo> propertyInfos = typeof(T).GetProperties();

            if (includeHeaderLine)
            {
                //add header line.
                foreach (var propertyInfo in propertyInfos)
                {
                    sb.Append(propertyInfo.Name).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            //add value for each property.
            foreach (var obj in Objects)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }

        //export to a file.
        public void ExportToFile(string path)
        {
            File.WriteAllText(path, Export());
        }

        //export as binary data.
        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes(Export());
        }

        //get the csv value for field.
        private string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";
            if (value is INullable && ((INullable)value).IsNull) return "";

            if (value is DateTime)
            {
                return ((DateTime)value).ToString(((DateTime)value).TimeOfDay.TotalSeconds < 1 ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss");
            }
            var output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;

        }
    }
}