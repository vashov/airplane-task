using NUnit.Framework;

namespace AirplaneTask.Core.Tests
{
    public class TextDecoderTests
    {
        [TestCase("a2[ed]k", "aededk")]
        [TestCase("a2[k3[cd]l]1", "akcdcdcdlkcdcdcdl1")]
        [TestCase("a[3k]", "a3k")]
        [TestCase("k22[q]", "k2qq")]
        [TestCase("2[]", "")]
        [TestCase("4", "4")]
        [TestCase("k22q", "k22q")]
        [TestCase("kq", "kq")]
        [TestCase("2[]3", "3")]
        [TestCase("2[]32[]3", "33")]
        [TestCase("", "")]
        public void TestValidPairs(string inputText, string expectedResult)
        {
            var textDecoder = new TextDecoder();

            var actualResult = textDecoder.Decode(inputText);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("2][3", "2][3")]
        [TestCase("2[3", "2[3")]
        [TestCase("2]3", "2]3")]
        [TestCase("]3", "]3")]
        [TestCase("[3", "[3")]
        [TestCase("3]", "3]")]
        [TestCase("]", "]")]
        [TestCase("[[[", "[[[")]
        [TestCase("]]", "]]")]
        [TestCase("][][][", "][")]
        [TestCase("]2[4]3[][", "]44[")]
        [TestCase("][4]3[][", "]4[")]
        public void TestInvalidPairs(string inputText, string expectedResult)
        {
            var textDecoder = new TextDecoder();

            var actualResult = textDecoder.Decode(inputText);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void TestNullThrowException()
        {
            var textDecoder = new TextDecoder();
            string sourceText = null;

            Assert.Throws<ArgumentNullException>(() => textDecoder.Decode(sourceText));
        }
    }
}