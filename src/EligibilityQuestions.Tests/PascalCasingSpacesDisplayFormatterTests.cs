using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class PascalCasingSpacesDisplayFormatterTests
    {
        private PascalCasingSpacesDisplayFormatter _theFormatter;

        [SetUp]
        public void Setup()
        {
            _theFormatter = new PascalCasingSpacesDisplayFormatter();
        }

        [Test]
        public void handles_regular_case()
        {
            var result = _theFormatter.FormatValue("ThisIsATest");
            result.ShouldEqual("This Is A Test");
        }

        [Test]
        public void handles_acryonym()
        {
            var result = _theFormatter.FormatValue("PDP");
            result.ShouldEqual("PDP");
        }

        [Test]
        public void handles_multiple_acronyms()
        {
            var result = _theFormatter.FormatValue("MAOrMAPD");
            result.ShouldEqual("MA Or MAPD");
        }
    }
}