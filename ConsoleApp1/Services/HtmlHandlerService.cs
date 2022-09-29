using ConsoleApp1.Enums;
using ConsoleApp1.Model;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ConsoleApp1.Services
{
    public static class HtmlHandlerService
    {

        #region Number Convertion
        public static double ToNumber(this string content)
        {
            return ConverToNumber(GetNumberInfo(content).Number);
        }

        private static double ConverToNumber(string content)
        {
            if (string.IsNullOrEmpty(content))
                return 0;

            if (!string.IsNullOrEmpty(content))
            {
                double number = 0;

                Double.TryParse(content, out number);

                return number;
            }

            return 0;
        }

        #endregion

        #region Date Conversion
        public static DateTime ConvertSensorTimestamp(this string content)
        {
            var dateStr = Regex.Match(content, "[0-9]{4}-[0-9]{2}-[0-9]{2}\\s+[0-9]+:[0-9]+:[0-9]{2}").Value;

            var date = new DateTime();

            var isDate = DateTime.TryParse(dateStr, out date);

            return isDate ? date : DateTime.Now;
        }
        #endregion

        #region Get Info
        public static DataType GetNumberType(this string content)
        {
            DataType dataType = DataType.mm;

            Enum.TryParse(GetNumberInfo(content).Type, out dataType);

            return dataType;
        }

        public static NumberTypeModel GetNumberInfo(string content)
        {
            // get only the number + type 2.20cm
            var partialStr = Regex.Match(content, @"[0-9].{1,6}").Value;

            var number = Regex.Match(partialStr, @"[0-9].{1,3}").Value;

            var type = string.Empty;

            // just for data as 1m, 2m, 3m...
            if (number.Length == 2)
            {
                number = Regex.Replace(partialStr, @"[^\d]", "");
                type = Regex.Match(partialStr, @"[a-z]").Value;
            }
            else
            {
                // get cm, mm
                type = Regex.Match(partialStr, @"[a-z][a-z]").Value;

                // get just m 
                if (string.IsNullOrWhiteSpace(type))
                {
                    type = Regex.Match(partialStr, @"[a-z]").Value;
                }
            }

            return new NumberTypeModel
            {
                Number = number,
                Type = type
            };
        }

        public static string GetContent(this string html, string attribute)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            if (!string.IsNullOrEmpty(attribute))
            {
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//div/p").ToList().ToList();

                if (htmlNodes != null && htmlNodes.Count > 0)
                {
                    foreach (var item in htmlNodes)
                    {
                        if (item.Attributes.Count > 0 && item.Attributes[attribute].Name == attribute)
                        {
                            return item.InnerHtml;
                        }
                    }
                }

                return string.Empty;
            }

            return string.Empty;
        }
        #endregion

        #region Not Used
        public static string GetHtmlByAttributeAndValue(string html, string attribute, string value)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(attribute))
            {
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//form/div/input").ToList();

                if (htmlNodes != null && htmlNodes.Count() > 0)
                {

                    foreach (var item in htmlNodes)
                    {
                        var itemValue = item.Attributes[attribute].Value;

                        if (itemValue.Contains(value))
                            return item.Attributes[attribute].Value;
                        else
                            continue;
                    }
                }

                return string.Empty;
            }

            return string.Empty;
        }

        public static void SetHtmlValueByAttribute(string html, string attribute, string value)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            if (!string.IsNullOrEmpty(value))
            {
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//form/div/input").ToList();

                if (htmlNodes != null && htmlNodes.Count() > 0)
                {

                    foreach (var item in htmlNodes)
                    {
                        var itemValue = item.Attributes[attribute].Value;

                        if (itemValue.Contains(value))
                            item.SetAttributeValue(attribute, value);
                        else
                            continue;
                    }
                }
            }
        }
        #endregion
    }
}
