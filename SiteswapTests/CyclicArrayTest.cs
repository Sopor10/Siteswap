using System.Linq;
using NUnit.Framework;
using Siteswaps;

namespace SiteswapTests
{
    public class CyclicArrayTest
    {
        [SetUp]
        public void Setup()
        {
        }

        
        
        [Test]
        [TestCase(0, ExpectedResult =  0)]
        [TestCase(1, ExpectedResult =  2)]
        [TestCase(2, ExpectedResult =  4)]
        [TestCase(3, ExpectedResult =  6)]
        [TestCase(4, ExpectedResult =  8)]
        [TestCase(5, ExpectedResult =  0)]
        [TestCase(6, ExpectedResult =  2)]
        [TestCase(7, ExpectedResult =  4)]
        [TestCase(-1, ExpectedResult =  8)]
        public int AccessShouldWork(int i)
        {
            var sut = new CyclicArray<int>(new []{0,2,4,6,8});

            var result = sut[i];

            return result;
        }

        [Test]
        public void Enumerate_ShouldWork()
        {
            var sut = new CyclicArray<int>(new []{1,2,3});
            
            var result = sut.Enumerate(2).ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(6));
                Assert.That(result[0],Is.EqualTo((0,1)));
                Assert.That(result[1],Is.EqualTo((1,2)));
                Assert.That(result[2],Is.EqualTo((2,3)));
                Assert.That(result[3],Is.EqualTo((3,1)));
                Assert.That(result[4],Is.EqualTo((4,2)));
                Assert.That(result[5],Is.EqualTo((5,3)));
            });
        }
        
        [Test]
        public void Enumerate_0_ShouldReturnEmptyIEnumerable()
        {
            var sut = new CyclicArray<int>(new []{1,2,3});
            
            var result = sut.Enumerate(0).ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(0));
            });
        }
        
        [Test]
        public void Sequence_ShouldWork()
        {
            var sut = new CyclicArray<int>(new []{1,2,3});
            
            var result = sut.Sequence().Take(10).ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(10));
                Assert.That(result, Is.EqualTo(new int[]{1,2,3,1,2,3,1,2,3,1}));

            });
        }
        

    }
}