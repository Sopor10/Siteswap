using NUnit.Framework;
using Siteswaps;

namespace SiteswapTests
{
    public class SiteswapTest
    {
        [Test]
        [TestCase(531)]
        [TestCase(441)]
        [TestCase(756)]
        [TestCase(744)]
        [TestCase(97531)]
        [TestCase(3)]
        public void IsValid_ValidSiteswap_ShouldWork(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsValid();
            
            Assert.True(result);
        }
        
        [Test]
        [TestCase(21)]
        [TestCase(97541)]
        [TestCase(76)]
        public void IsValid_InvalidSiteswap_ShouldWork(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsValid();
            
            Assert.False(result);
        }

        [Test]
        [TestCase(744,5)]
        [TestCase(5,5)]
        [TestCase(97531,5)]
        [TestCase(633,4)]
        public void NumberOfObjects_ShouldWork(int siteswap, int numberOfBalls)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.NumberOfObjects();
            Assert.That(result,Is.EqualTo((decimal)numberOfBalls));
        }
        
        [Test]
        [TestCase(744)]
        [TestCase(5)]
        [TestCase(97531)]
        [TestCase(633)]
        [TestCase(31)]
        public void IsGroundState_GroundStateSiteswap_ShouldReturnTrue(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsGroundState();
            Assert.True(result);
        }
        
        [Test]
        [TestCase(51)]
        [TestCase(71)]
        [TestCase(336)]
        [TestCase(363)]
        [TestCase(75319)]
        public void IsGroundState_NotGroundStateSiteswap_ShouldReturnFalse(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsGroundState();
            Assert.False(result);
        }
        
        [Test]
        [TestCase(744)]
        [TestCase(5)]
        [TestCase(97531)]
        [TestCase(633)]
        [TestCase(31)]
        public void IsExcitedState_NotExitedStateSiteswap_ShouldReturnFalse(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsExcitedState();
            Assert.False(result);
        }
        
        [Test]
        [TestCase(51)]
        [TestCase(71)]
        [TestCase(336)]
        [TestCase(363)]
        [TestCase(75319)]
        [TestCase(53197)]
        public void IsExcitedState_ExitedStateSiteswap_ShouldReturnTrue(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.IsExcitedState();
            Assert.True(result);
        }
        
        [Test]
        [TestCase(51,ExpectedResult = 5)]
        [TestCase(71,ExpectedResult = 7)]
        [TestCase(336,ExpectedResult = 6)]
        [TestCase(363,ExpectedResult = 6)]
        [TestCase(75319,ExpectedResult = 9)]
        [TestCase(53197,ExpectedResult = 9)]
        public int MaxThrow_ShouldWork(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.MaxThrow();
            return result;
        }
        
        [Test]
        [TestCase(51)]
        public void GenerateHandStatus_ShouldWork(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.GenerateHandStatus();
            
            var expectedValues = new CyclicArray<HandStatus>(new []
            {
                HandStatus.Empty,
                HandStatus.Empty,
                HandStatus.Full,
                HandStatus.Empty,
                HandStatus.Full
            });
            Assert.Multiple(() =>
            {
                for (int i = 0; i < sut.MaxThrow(); i++)
                {
                    Assert.That(result[i], Is.EqualTo(expectedValues[i]), $"Position {i} ist falsch");
                }
            });
        }
        
        [Test]
        [TestCase(51)]
        
        public void CalculateGetIn_ExitedStateSiteswap_ShouldReturnGetin(int siteswap)
        {
            var sut = new Siteswap(siteswap);

            var result = sut.CalculateGetIn();
            
//            Assert.True(result);
        }
    }
}