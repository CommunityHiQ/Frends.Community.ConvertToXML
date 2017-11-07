using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GenericParsing;

namespace Frends.Community.ConvertToXML
{    /// <summary>
     /// SubmitForm
     /// </summary>
    public static class ConvertData
    {
        /// <summary>
        /// Column
        /// </summary>
        public class Column
        {
            /// <summary>
            /// Column length
            /// </summary>
            public int Length { set; get; }
        }
        /// <summary>
        /// Parameters for file appearing
        /// </summary>
        public class Parameters
        {
            /// <summary>
            /// Input data. Supported formats JSON, CSV and fixed length
            /// </summary>
            public string Input { get; set; }
            /// <summary>
            /// XML root element name
            /// </summary>
            public string XMLRootElementName { get; set; }
            /// <summary>
            /// CSV separator
            /// </summary>
            public string CSVSeparator { get; set; }
            /// <summary>
            /// Output column lengths
            /// </summary>
            public Column[] ColumnLengths { get; set; }
            /// <summary>
            /// Input has header row
            /// </summary>
            public bool InputHasHeaderRow { get; set; }
            /// <summary>
            /// Trim ouput columns
            /// </summary>
            public bool TrimOuputColumns { get; set; }
        }

        /// <summary>
        /// Return object
        /// </summary>
        public class Output
        {
            /// <summary>
            /// Result string
            /// </summary>
            public string Result { get; set; }
        }

        /// <summary>
        /// Parse input to xml
        /// </summary>
        /// <returns>Object {string Result }  </returns>
        public static Output ConvertToXML(Parameters parameters)
        {
            if (parameters.Input.GetType() != typeof(string))
                throw new InvalidDataException("The input data string was not in correct format. Supported formats are JSON, CSV and fixed length.");

            if (parameters.Input.StartsWith("{") || parameters.Input.StartsWith("["))
            {
                return new Output { Result = JsonConvert.DeserializeXmlNode(parameters.Input, parameters.XMLRootElementName).OuterXml };
            }

            if (!string.IsNullOrEmpty(parameters.CSVSeparator) && parameters.Input.Contains(parameters.CSVSeparator))
            {
                using (var parser = new GenericParserAdapter())
                {
                    char? separator = Convert.ToChar(parameters.CSVSeparator);
                    parser.SetDataSource(new StringReader(parameters.Input));
                    parser.ColumnDelimiter = separator;
                    parser.FirstRowHasHeader = parameters.InputHasHeaderRow;
                    parser.MaxBufferSize = 4096;
                    parser.TrimResults = parameters.TrimOuputColumns;
                    return new Output { Result = parser.GetXml().OuterXml };
                }
            }

            if (parameters.ColumnLengths == null)
                throw new InvalidDataException("The input was recognized as fixed length file, but no column lengths were supplied.");

            using (var parser = new GenericParserAdapter())
            {
                var headerList = new List<int>();
                foreach (var column in parameters.ColumnLengths)
                {
                    headerList.Add(column.Length);
                }
                var headerArray = headerList.ToArray();

                parser.SetDataSource(new StringReader(parameters.Input));
                parser.ColumnWidths = headerArray;
                parser.FirstRowHasHeader = parameters.InputHasHeaderRow;
                parser.MaxBufferSize = 4096;
                parser.TrimResults = parameters.TrimOuputColumns;
                return new Output { Result = parser.GetXml().OuterXml };
            }
        }
    }
}
