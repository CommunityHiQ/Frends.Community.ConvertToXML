using System;
using NUnit.Framework;

namespace Frends.Community.ConvertToXML.Tests
{
    [TestFixture]
    public class ConvertDataTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Down()
        {
        }

        [Test]
        public void TestConvertToXMLUsingFlatFileInputWithoutSeparator()
        {
            var columns = new[] 
            {
                new Column { Length = 3 },
                new Column { Length = 3 },
                new Column { Length = 2 },
                new Column { Length = 4 },
                new Column { Length = 4 }
            };

            var options = new Parameters {Input = "asd123as1234asdf"};
            var csvOptions = new CsvInputParameters { CSVSeparator = null, ColumnLengths = columns, InputHasHeaderRow = false, TrimOuputColumns = false };
            var result = ConvertData.ConvertToXML(options, csvOptions, null);
            Assert.IsTrue(result.Result.StartsWith("<NewDataSet><Table1><Column1>asd</Column1>"));
        }

        [Test]
        public void TestConvertToXMLUsingSeparatorAndTrim()
        {
            var options = new Parameters { Input = "asd ;as;asdf"};
            var csvOptions = new CsvInputParameters
            {
                CSVSeparator = ";",
                ColumnLengths = null,
                InputHasHeaderRow = false,
                TrimOuputColumns = true
            };
            var result = ConvertData.ConvertToXML(options, csvOptions, null);
            Assert.IsTrue(result.Result.StartsWith("<NewDataSet><Table1><Column1>asd</Column1>"));
        }

        [Test]
        public void TestConvertToXMLUsingJSON()
        {
            var options = new Parameters { Input = "{\"field1\":\"value1\", \"field2\":\"value2\"}"};
            var jsonInputParameters = new JsonInputParameters {XMLRootElementName = "test"};
            var result = ConvertData.ConvertToXML(options, null, jsonInputParameters);
            Assert.IsTrue(result.Result.StartsWith("<test><field1>value1</field1>"));
        }
        [Test]
        public void TestConvertToXMLWithNumericKeys()
        {

            var options = new Parameters { Input = "{\"48\":\"value1\", \"2\":\"value2\"}"};
            var jsonInputParameters = new JsonInputParameters {XMLRootElementName = "test", AppendToFieldName = "foo"};
            var result = ConvertData.ConvertToXML(options, null, jsonInputParameters);
            Assert.IsTrue(result.Result.StartsWith("<test><foo48>value1</foo48>"));
        }

        [Test]
        public void TestConvertToXMLWithNumericKeysWithoutAppend()
        {
            var options = new Parameters { Input = "{\"1\":\"value1\", \"2\":\"value2\"}"};
            var jsonInputParameters = new JsonInputParameters {XMLRootElementName = "test", AppendToFieldName = null};

            var result = ConvertData.ConvertToXML(options, null, jsonInputParameters);
            Assert.IsTrue(result.Result.StartsWith("<test><_x0031_>value1</_x0031_>"));

        }
    }
}