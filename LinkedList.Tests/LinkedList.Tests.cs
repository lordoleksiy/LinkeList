using NUnit.Framework;
using LinkedList;
using System;
using FluentAssertions;

namespace LinkedList.Tests
{
    public class Tests
    {
        private LinkedList<int> _list;
        private int[] _massive; 

        [SetUp]
        public void Setup()
        {
            _list = new LinkedList<int>(0, 1, 2, 3, 4);
            _massive = new int[5] {0, 1, 2, 3, 4 };
        }

        #region Constructor
        [Test]
        public void Constructor_PassNUllMassisve_ArgumentNullException()
        {
            int[] massive = null;
            LinkedList<int> list;

            Action act = () => list = new(massive);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("items");
        }
        #endregion

        #region ICollectionT
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
        public void CopyTo_CopyToMassive_MassiveEqualsExpeñted()
        {
            int[] actual = new int[5];
            int[] expected = _massive;

            _list.CopyTo(actual, 0);

            actual.Should().BeEquivalentTo(expected);

        }
        [Test]
        public void CopyTo_CopyToEmptyCollection_ArgumentNullException()
        {
            LinkedList<int> vs = new();
            int[] massive = new int[5];

            Action action = () => vs.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("empty collection");
        }
        [Test]
        public void CopyTo_CopyToNullMassive_ArgumentNullException()
        {
            int[] massive = null;

            Action action = () => _list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("array");
        }
        [Test]
        public void CopyTo_CopyToWitnIdexBelowZero_ArgumentOutOfRange()
        {
            int[] massive = new int[5];

            Action action = () => _list.CopyTo(massive, -1);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index");
        }
        [Test]
        public void CopyTo_CopyToMassiveWithSizeLessThanCount_ArgumentException()
        {
            int[] massive = new int[1];

            Action action = () => _list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("array");
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
        [Test]
        public void Remove_RemoveNotContainedNum_ReturnFalse()
        {
            LinkedList<int> vs = _list;

            bool result = vs.Remove(6);

            result.Should().BeFalse();
        }

        [Test]
        public void IsReadOnly_ReturnFalse()
        {
            bool actual = _list.IsReadOnly;

            actual.Should().BeFalse();
        }
        //[Test]
        //public void GetEnumerator()
        //{
        //    int[] expected = { 1, 2, 3, 4, 5 };
        //    int[] actual = new int[5];

        //    IEnumerable weak = _list;

        //    CollectionAssert.AreEqual(sequence,
        //        new[] { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610 });


        //    actual.Should().BeEquivalentTo(expected);
        //}
        #endregion

        #region ICollection
        [TestCase(new int[] {1, 2, 3, 4 })]
        [TestCase(new int[] { })]
        [TestCase(new int[] {1, 2 })]
        public void Count_CountOfCollectionIsEqualsToLengthOfMatrix(int[] massive)
        {
            LinkedList<int> list = new(massive);
            int expected = massive.Length;

            int actual = list.Count;

            actual.Should().Be(expected);
        }
        [Test]
        public void IsSynchronized_ReturnFalse()
        {
            bool actual = _list.IsSynchronized;

            actual.Should().BeFalse();
        }

        [Test]
        public void SyncRoot_SyncRoot_ReturnsSameObject()
        {
            object expected = _list.SyncRoot;

            object actual = _list.SyncRoot;

            actual.Should().Be(expected);
        }

        [Test]
        public void CopyToLegacy_CopyToMassive_MassiveEqualsExpeñted()
        {
            Array actual = new int[5];
            Array expected = _massive;

            _list.CopyTo(actual, 0);

            actual.Should().BeEquivalentTo(expected);

        }
        [Test]
        public void CopyToLegacy_CopyToEmptyCollection_ArgumentNullException()
        {
            LinkedList<int> vs = new();
            Array massive = new int[5];

            Action action = () => vs.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("empty collection");
        }
        [Test]
        public void CopyToLegacy_CopyToNullMassive_ArgumentNullException()
        {
            Array massive = null;

            Action action = () => _list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("array");
        }
        [Test]
        public void CopyToLegacy_CopyToWitnIdexBelowZero_ArgumentOutOfRange()
        {
            Array massive = new int[5];

            Action action = () => _list.CopyTo(massive, -1);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index");
        }
        [Test]
        public void CopyToLegacy_CopyToMassiveWithSizeLessThanCount_ArgumentException()
        {
            Array massive = new int[1];

            Action action = () => _list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("array");
        }

        #endregion

        #region MyMethods
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void This_ThisReturnsCorrectNum(int num)
        {
            LinkedList<int> list = _list;
            int expected = num;

            int actual = list[num];

            actual.Should().Be(expected);
        }
        [TestCase(-1)]
        [TestCase(10)]
        public void This_ThisWithIncorrectIndex_IndexOutOfRange(int index)
        {
            LinkedList<int> list = _list;
            int actual;

            Action action = () => actual = list[index];

            action.Should().Throw<IndexOutOfRangeException>()
                .WithMessage("index");
        }
        #endregion
    }
}