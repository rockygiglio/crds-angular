using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace crds_angular.Util
{
    public static class CSV
    {
        public static void Create<T>(List<T> list, string[] headers, Stream stream, string delimiter, bool useQuotes = false) where T : class
        {
            var sw = new StreamWriter(stream, Encoding.UTF8);
            var wroteHeaderRow = false;

            foreach (var row in list)
            {
                if (!wroteHeaderRow)
                {
                    WriteHeaders(sw, headers, delimiter, useQuotes);
                    wroteHeaderRow = true;
                }

                WriteRow(sw, row, headers, delimiter, useQuotes);
            }

            // Reset the stream position to the beginning
            stream.Seek(0, SeekOrigin.Begin);
        }

        private static void WriteHeaders(TextWriter sw, IEnumerable<string> headers, string delimiter, bool useQuotes)
        {
            var initialCell = true;

            foreach (var header in headers)
            {
                if (!initialCell)
                {
                    sw.Write(delimiter);
                }

                if (useQuotes)
                {
                    sw.Write($"\"{header}\"");
                }
                else
                {
                    sw.Write(header);
                }
                initialCell = false;
            }

            sw.Write(Environment.NewLine);
            sw.Flush();
        }

        private static void WriteRow<T>(TextWriter sw, T row, IEnumerable<string> headers, string delimiter, bool useQuotes)
        {
            var initialCell = true;

            foreach (var name in headers)
            {
                if (!initialCell)
                {
                    sw.Write(delimiter);
                }

                var methodName = "get_" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Replace(" ", "");
                var method = row.GetType().GetMethod(methodName);
                var value = method.Invoke(row, null);
                var sValue = (value == null ? "" : value.ToString());

                if (useQuotes)
                {
                    sw.Write($"\"{sValue}\"");
                }
                else
                {
                    sw.Write(sValue);
                }
                initialCell = false;
            }

            sw.Write(Environment.NewLine);
            sw.Flush();
        }
    }
}