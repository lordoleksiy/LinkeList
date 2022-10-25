using NUnit.Framework;
using LinkedList;
using System;

namespace LinkedList.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Constructor
        [Test]
        public void Constructor_2ElementsAdded_isNotEmpty()
        {
            LinkedList<int> list = new(1, 2);

            Assert.IsNotEmpty(list);
        }

        [Test]
        public void Constructor_NUllMassisve_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(_Constructor_NUllMassisve_ArgumentNullExceptionBody);
        }

        private void _Constructor_NUllMassisve_ArgumentNullExceptionBody()
        {
            int[] massive = null;

            LinkedList<int> list = new(massive);
        }
        #endregion

        #region CollectionT
        [Test]
        public void Add_1ToCollection_CountIs1AndListIsNotEmpty()
        {
            LinkedList<int> list = new();
            int expectedCount = 1;

            list.Add(1);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedCount, list.Count);
                Assert.IsNotEmpty(list);
            });     
        }

        [Test]
        public void Clear_CountIs0AndListIsEmpty()
        {
            LinkedList <String> list = new("a", "b", "c");
            int expectedCount = 0;
            
            list.Clear();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedCount, list.Count);
                Assert.IsEmpty(list);
            });
        }


        #endregion

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(-5)]
        public void Add_NumToCollectionAndTestThis(int num)
        {
            LinkedList<int> list = new();
            int expectedNumber = num;

            list.Add(num);

            Assert.AreEqual(expectedNumber, list[0]);

        }
    }
}