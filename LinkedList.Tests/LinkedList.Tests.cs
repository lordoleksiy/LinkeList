using NUnit.Framework;
using LinkedList;
using System;
using FluentAssertions;

namespace LinkedList.Tests
{
    public class Tests
    {
        private LinkedList<int> _list;

        [SetUp]
        public void Setup()
        {
            _list = new LinkedList<int>(1, 2, 3, 4, 5);
        }

        #region Constructor
        [Test]
        public void Constructor_PassNUllMassisve_ArgumentNullException()
        {
            int[] massive = null;
            LinkedList<int> list;

            Action act = () =>  list = new(massive);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("items");
        }
        #endregion

        #region CollectionT
        [Test]
        public void Add_NumToCollection_CountIs1AndListIsNotNullOrEmpty()
        {
            LinkedList<int> list = new();
            int expectedCount = 1;

            list.Add(5);

            list.Should()
                .HaveCount(expectedCount).And
                .NotBeNullOrEmpty();
        }

        [Test]
        public void Clear_CountIs0AndListIsEmpty()
        {
            LinkedList<String> list = new("a", "b", "c");
            int expectedCount = 0;

            list.Clear();

            list.Should()
                .HaveCount(expectedCount).And
                .BeEmpty();
        }

        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 3)]
        [TestCase(new int[] { 3, 1, 5 }, 1)]
        [TestCase(new int[] { 9 }, 9)]
        public void Contains_ContainsNum_ReturnTrue(int[] massive, int num)
        {
            LinkedList<int> list = new(massive);

            bool actual = list.Contains(num);

            actual.Should().BeTrue();
        }

        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 10)]
        [TestCase(new int[] { }, 1)]
        public void Contains_ContainsNum_ReturnFalse(int[] massive, int num)
        {
            LinkedList<int> list = new(massive);

            bool actual = list.Contains(num);

            actual.Should().BeFalse();

        }

        [Test]
        public void CopyTo_CopyToMassive_MassiveEqualsExpexted()
        {
            int[] actual = new int[5];
            int[] expected = { 1, 2, 3, 4, 5 };

            _list.CopyTo(actual, 0);

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase(3)]
        [TestCase(1)]
        [TestCase(5)]
        public void Remove_RemoveNum_ContainsNumFalse(int num)
        {
            LinkedList<int> vs = _list;

            vs.Remove(num);

            vs.Should().NotContain(num);
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