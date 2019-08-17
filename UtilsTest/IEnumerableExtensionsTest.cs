using System.Collections.Generic;
using NUnit.Framework;
using Utils;

namespace UtilsTest
{
    public class IEnumerableExtensionsTest
    {
        [Test]
        public void ContainsAllItems_ShouldReturnTrue()
        {
            List<int> sut = new List<int> {1, 2, 3, 4};
            List<int> list = new List<int>() {1, 2, 3, 4};

            bool result = sut.ContainsAllItemsFrom(list);
            
            Assert.True(result);
        }
        
        [Test]
        public void ContainsAllItems_MultipleEntries_ShouldReturnTrue()
        {
            List<int> sut = new List<int> {1, 2, 2, 3};
            List<int> list = new List<int>() {1, 2, 3};

            bool result = sut.ContainsAllItemsFrom(list);
            
            Assert.True(result);
        }
        
        [Test]
        public void ContainsAllItems_AHasMoreItems_ShouldReturnTrue()
        {
            List<int> sut = new List<int> {1, 2, 3};
            List<int> list = new List<int>() {1, 2};

            bool result = sut.ContainsAllItemsFrom(list);
            
            Assert.True(result);
        }
        
        [Test]
        public void ContainsAllItems_ItemNotInOriginalList_ShouldReturnFalse()
        {
            List<int> sut = new List<int> {1, 2};
            List<int> list = new List<int>() {1, 2, 3};

            bool result = sut.ContainsAllItemsFrom(list);
            
            Assert.False(result);
        }
        
    }
}