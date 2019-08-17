using NUnit.Framework;
using Utils;

namespace UtilsTest
{
    public class IntExtensionsTest
    {
        [Test]
        [TestCase(1,2,ExpectedResult = 1)]
        [TestCase(2,2,ExpectedResult = 0)]
        [TestCase(-1,2,ExpectedResult = 1)]
        [TestCase(21,2,ExpectedResult = 1)]
        public int Mod_ShouldWork(int sut, int modulo)
        {
            return sut.Mod(modulo);
        }
    }
}