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
                new ConvertData.Column { Length = 3 },
                new ConvertData.Column { Length = 3 },
                new ConvertData.Column { Length = 2 },
                new ConvertData.Column { Length = 4 },
                new ConvertData.Column { Length = 4 }
            };

            var options = new ConvertData.Parameters { Input = "asd123as1234asdf", XMLRootElementName = "", CSVSeparator = null, ColumnLengths = columns, InputHasHeaderRow = false, TrimOuputColumns = false };

            var result = ConvertData.ConvertToXML(options);
            Assert.IsTrue(result.Result.StartsWith("<NewDataSet><Table1><Column1>asd</Column1>"));
        }

        [Test]
        public void TestConvertToXMLUsingSeparatorAndTrim()
        {
            var options = new ConvertData.Parameters { Input = "asd ;as;asdf", XMLRootElementName = "", CSVSeparator = ";", ColumnLengths = null, InputHasHeaderRow = false, TrimOuputColumns = true };

            var result = ConvertData.ConvertToXML(options);
            Assert.IsTrue(result.Result.StartsWith("<NewDataSet><Table1><Column1>asd</Column1>"));
        }

        [Test]
        public void TestConvertToXMLUsingJSON()
        {
            var options = new ConvertData.Parameters { Input = "{\"field1\":\"value1\", \"field2\":\"value2\"}", XMLRootElementName = "test", CSVSeparator = null, ColumnLengths = null, InputHasHeaderRow = false, TrimOuputColumns = false };

            var result = ConvertData.ConvertToXML(options);
            Assert.IsTrue(result.Result.StartsWith("<test><field1>value1</field1>"));
        }
    }
}