using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using GenericParsing;
using Newtonsoft.Json.Linq;

namespace Frends.Community.ConvertToXML
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
    /// Json input parameters
    /// </summary>
    public class JsonInputParameters
    {
        /// <summary>
        /// XML root element name
        /// </summary>
        public string XMLRootElementName { get; set; }
        /// <summary>
        /// Json change numeric keys
        /// </summary>
        public string AppendToFieldName { get; set; }
    }
    /// <summary>
    /// Csv input parameters
    /// </summary>
    public class CsvInputParameters
    {
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
    /// Parameters for file appearing
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Input data. Supported formats JSON, CSV and fixed length
        /// </summary>
        public string Input { get; set; }
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
    /// SubmitForm
    /// </summary>
    public static class ConvertData
    {
        /// <summary>
        /// Parse input to xml
        /// </summary>
        /// <returns>Object {string Result }  </returns>
        public static Output ConvertToXML(Parameters parameters, [PropertyTab] CsvInputParameters csvInputParameters, [PropertyTab] JsonInputParameters jsonInputParameters)
        {
            if (parameters.Input.GetType() != typeof(string))
                throw new InvalidDataException("The input data string was not in correct format. Supported formats are JSON, CSV and fixed length.");

            if (parameters.Input.StartsWith("{") || parameters.Input.StartsWith("["))
            {
                if (string.IsNullOrEmpty(jsonInputParameters.XMLRootElementName))
                    throw new MissingFieldException("Root element name missing. Required with JSON input");

                if (jsonInputParameters.AppendToFieldName == null)
                    return new Output { Result = JsonConvert.DeserializeXmlNode(parameters.Input, jsonInputParameters.XMLRootElementName).OuterXml };

                var jsonObject = (JObject)JsonConvert.DeserializeObject(parameters.Input);
                var newObject = ChangeNumericKeys(jsonObject, jsonInputParameters.AppendToFieldName);
                return new Output { Result = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(newObject), jsonInputParameters.XMLRootElementName).OuterXml };
            }

            if (!string.IsNullOrEmpty(csvInputParameters.CSVSeparator) && parameters.Input.Contains(csvInputParameters.CSVSeparator))
            {
                using (var parser = new GenericParserAdapter())
                {
                    char? separator = Convert.ToChar(csvInputParameters.CSVSeparator);
                    parser.SetDataSource(new StringReader(parameters.Input));
                    parser.ColumnDelimiter = separator;
                    parser.FirstRowHasHeader = csvInputParameters.InputHasHeaderRow;
                    parser.MaxBufferSize = 4096;
                    parser.TrimResults = csvInputParameters.TrimOuputColumns;
                    return new Output { Result = parser.GetXml().OuterXml };
                }
            }

            if (csvInputParameters.ColumnLengths == null)
                throw new InvalidDataException("The input was recognized as fixed length file, but no column lengths were supplied.");

            using (var parser = new GenericParserAdapter())
            {
                var headerList = new List<int>();
                foreach (var column in csvInputParameters.ColumnLengths)
                {
                    headerList.Add(column.Length);
                }
                var headerArray = headerList.ToArray();

                parser.SetDataSource(new StringReader(parameters.Input));
                parser.ColumnWidths = headerArray;
                parser.FirstRowHasHeader = csvInputParameters.InputHasHeaderRow;
                parser.MaxBufferSize = 4096;
                parser.TrimResults = csvInputParameters.TrimOuputColumns;
                return new Output { Result = parser.GetXml().OuterXml };
            }
        }               
        private static JObject ChangeNumericKeys(JObject o, string appendWith)
        {
            var newO = new JObject();

            foreach (var node in o)
            {
                switch (node.Value.Type)
                {
                    case JTokenType.Array:
                        var newArray = new JArray();
                        foreach (var item in node.Value)
                        {
                            newArray.Add(ChangeNumericKeys(JObject.FromObject(item), appendWith));
                        }
                        newO[node.Key] = newArray;
                        break;
                    case JTokenType.Object:
                        newO[node.Key] = ChangeNumericKeys(JObject.FromObject(node.Value), appendWith);
                        break;
                    default:
                        if (char.IsNumber(node.Key[0]))
                        {
                            var newName = appendWith + node.Key;
                            newO[newName] = node.Value;
                        }
                        else
                        {
                            newO[node.Key] = node.Value;
                        }
                        break;
                }
            }

            return newO;
        }
    }
}
