using NUnit.Framework;
using LinkedList;
using System;
using FluentAssertions;

namespace LinkedList.Tests
{
    [TestFixture]
    public class Tests
    {
        private LinkedList<int> _list;
        private int[] _massive;

        [SetUp]
        public void Setup()
        {
            _list = new LinkedList<int>(10, 8, 4, 9, 2);
            _massive = new int[5] { 10, 8, 4, 9, 2 };
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
            int expectedCount = 0;

            _list.Clear();

            _list.Should()
                .HaveCount(expectedCount).And
                .BeEmpty();
        }

        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 3)]
        [TestCase(new int[] { 3, 1, 5 }, 1)]
        [TestCase(new int[] { 9 }, 9)]
        public void Contains_ContainsNum_ReturnTrue(int[] massive, int num)
        {
            _list = new(massive);

            bool actual = _list.Contains(num);

            actual.Should().BeTrue();
        }

        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 10)]
        [TestCase(new int[] { }, 1)]
        public void Contains_ContainsNum_ReturnFalse(int[] massive, int num)
        {
            _list = new(massive);

            bool actual = _list.Contains(num);

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
            _list = new();
            int[] massive = new int[5];

            Action action = () => _list.CopyTo(massive, 0);

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
            _list.Remove(num);

            _list.Should().NotContain(num);
        }
        [Test]
        public void Remove_RemoveNotContainedNum_ReturnFalse()
        {
            bool result = _list.Remove(6);

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
        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 1, 2 })]
        public void Count_CountOfCollectionIsEqualsToLengthOfMatrix(int[] massive)
        {
            _list = new(massive);
            int expected = massive.Length;

            int actual = _list.Count;

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
            _list = new();
            Array massive = new int[5];

            Action action = () => _list.CopyTo(massive, 0);

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
        [TestCase(10, 0)]
        [TestCase(4, 2)]
        [TestCase(2, 4)]
        public void This_ThisReturnsCorrectNum(int num, int index)
        {
            int expected = num;

            int actual = _list[index];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void Get_GetReturnsCorrectNode(int index)
        {
            int expectedVal = _list[index];


            LinkedListNode<int> actual = _list.Get(index);

            actual.List.Should().BeEquivalentTo(_list);
            actual.Value.Should().Be(expectedVal);
        }
        [TestCase(-1)]
        [TestCase(100)]
        public void Get_GetElementOnInCorrectIndex_ArgumentOutOfRange(int index)
        {
            Action action = () => _list.Get(index);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
        [TestCase(-1)]
        [TestCase(10)]
        public void This_ThisWithIncorrectIndex_IndexOutOfRange(int index)
        {
            int actual;

            Action action = () => actual = _list[index];

            action.Should().Throw<IndexOutOfRangeException>()
                .WithMessage("index");
        }
        [Test]
        public void First_ReturnsFirstElemOfCollection_FirstElemEqualsNum()
        {
            int expected = this._list[0];

            int actual = _list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void First_FirstElementButCollectionIsEmpty_InvalidOperationException()
        {
            _list = new();
            int actual;

            Action action = () => actual = _list.First;

            action.Should().Throw<InvalidOperationException>();
        }
        [Test]
        public void Last_ReturnsLastElemOfCollection_LastElemEqualsNum()
        {
            int expected = _list[_list.Count - 1];

            int actual = _list.Last;

            actual.Should().Be(expected);
        }

        [Test]
        public void Last_LastElementButCollectionIsEmpty_InvalidOperationException()
        {
            _list = new();
            int actual;

            Action action = () => actual = _list.Last;

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void AddFirst_AddFirstItemToEmptyCollection_FirstEqualsToAdded()
        {
            _list = new();
            int expected = 99;

            _list.AddFirst(99);
            int actual = _list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstItemToStart_FirstEqualsToAdded()
        {
            int expected = 99;

            _list.AddFirst(99);
            int actual = _list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFLast_AddLastItemToEmptyCollection_LastEqualsToAdded()
        {
            _list = new();
            int expected = 99;

            _list.AddLast(99);
            int actual = _list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastTItemToEnd_LastEqualsToAdded()
        {
            int expected = 99;

            _list.AddLast(99);
            int actual = _list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstNodeToEmptyCollection_FirstEqualsToAdded()
        {
            _list = new();
            int expected = 99;
            LinkedListNode<int> added = new(99);

            _list.AddFirst(added);
            int actual = _list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstNodeToStart_FirstEqualsToAdded()
        {
            int expected = 99;
            LinkedListNode<int> added = new(99);


            _list.AddFirst(added);
            int actual = _list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastNodeToEmptyCollection_LastEqualsToAdded()
        {
            _list = new();
            int expected = 99;
            LinkedListNode<int> added = new(99);

            _list.AddLast(added);
            int actual = _list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastNodeToEnd_LastEqualsToAdded()
        {
            int expected = 99;
            LinkedListNode<int> added = new(99);


            _list.AddLast(added);
            int actual = _list.Last;

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddAfter_AddAfterTItem_ElemOnNextIndexEqualsExpected(int index)
        {
            int expected = 99;

            _list.AddAfter(_list.Get(index), 99);
            int actual = _list[index + 1];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddAfter_AddAfterNode_ElemOnNextIndexEqualsExpected(int index)
        {
            LinkedListNode<int> node = new(99);
            int expected = 99;

            _list.AddAfter(_list.Get(index), node);
            int actual = _list[index + 1];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddBefore_AddBeforeTItem_ElemOnPrevIndexEqualsExpected(int index)
        {
            int expected = 99;

            _list.AddBefore(_list.Get(index), 99);
            int actual = _list[index];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddBefore_AddBeforeNode_ElemOnIndexEqualsBefore(int index)
        {
            LinkedListNode<int> node = new(99);
            int expected = 99;

            _list.AddBefore(_list.Get(index), node);
            int actual = _list[index];

            actual.Should().Be(expected);
        }
        [Test]
        public void AddBeforeAddAfter_AddWhenNodeInCollectionNull_ArgumentNullException()
        {
            LinkedListNode<int> newNode = new(10);
            int newItem = 10;

            Action action1 = () => _list.AddBefore(null, newNode);
            Action action2 = () => _list.AddAfter(null, newNode);
            Action action3 = () => _list.AddBefore(null, newItem);
            Action action4 = () => _list.AddAfter(null, newItem);

            action1.Should().Throw<ArgumentNullException>()
                .WithParameterName("node");
            action2.Should().Throw<ArgumentNullException>()
                .WithParameterName("node");
            action3.Should().Throw<ArgumentNullException>()
                .WithParameterName("node");
            action4.Should().Throw<ArgumentNullException>()
                .WithParameterName("node");
        }
        [Test]
        public void AddMethods_AddNullNode_ArgumentNullException()
        {
            LinkedListNode<int> node = _list.Get(1);
            Action action1 = () => _list.AddFirst(null);
            Action action2 = () => _list.AddLast(null);
            Action action3 = () => _list.AddBefore(node, null);
            Action action4 = () => _list.AddAfter(node, null);

            action1.Should().Throw<ArgumentNullException>()
                .WithParameterName("newNode");
            action2.Should().Throw<ArgumentNullException>()
                .WithParameterName("newNode");
            action3.Should().Throw<ArgumentNullException>()
                .WithParameterName("newNode");
            action4.Should().Throw<ArgumentNullException>()
                .WithParameterName("newNode");
        }
        [Test]
        public void AddMethods_AddNodeWithNotNullList_InvalidOperationException()
        {
            LinkedListNode<int> newNode = new(10);
            LinkedListNode<int> node = _list.Get(1);
            _list.AddFirst(newNode);

            Action action1 = () => _list.AddFirst(newNode);
            Action action2 = () => _list.AddLast(newNode);
            Action action3 = () => _list.AddAfter(node, newNode);
            Action action4 = () => _list.AddBefore(node, newNode);

            action1.Should().Throw<InvalidOperationException>()
                .WithMessage("newNode");
            action2.Should().Throw<InvalidOperationException>()
                .WithMessage("newNode");
            action3.Should().Throw<InvalidOperationException>()
                .WithMessage("newNode");
            action4.Should().Throw<InvalidOperationException>()
                .WithMessage("newNode");
        }
        [Test]
        public void AddMethods_AddNodeAfterOrBeforeNodeNotInList_InvalidOperationException()
        {
            LinkedListNode<int> node = new(99);
            LinkedListNode<int> testNode1 = new(100);
            LinkedListNode<int> testNode2 = new(100);

            Action action1 = () => _list.AddBefore(node, testNode1);
            Action action2 = () => _list.AddBefore(node, 100);
            Action action3 = () => _list.AddAfter(node, testNode2);
            Action action4 = () => _list.AddAfter(node, 100);

            action1.Should().Throw<InvalidOperationException>()
                .WithMessage("No item has been found");
            action2.Should().Throw<InvalidOperationException>()
                .WithMessage("No item has been found");
            action3.Should().Throw<InvalidOperationException>()
                .WithMessage("No item has been found");
            action4.Should().Throw<InvalidOperationException>()
                .WithMessage("No item has been found");
        }
        [Test]
        public void RemoveFirst_RemoveFirstNodeFromCollection_NodeIsRemoved()
        {
            int removed = _list.First;

            _list.RemoveFirst();
            int actual = _list.First;

            actual.Should().NotBe(removed);
        }

        [Test]
        public void Removelast_RemoveLastNodeFromCollection_NodeIsRemoved()
        {
            int removed = _list.Last;

            _list.RemoveLast();
            int actual = _list.Last;


            actual.Should().NotBe(removed);
        }
        [Test]
        public void Find_FindNumber_FindNumberEqualsExcpected()
        {
            int expected = 4;

            LinkedListNode<int> node =  _list.Find(expected);
            int actual = node.Value;

            actual.Should().Be(expected);
        }
        [Test]
        public void FindLast_FindLastNumber_FindNumberEqualsExcpected()
        {
            int expected = 4;

            LinkedListNode<int> node = _list.FindLast(expected);
            int actual = node.Value;

            actual.Should().Be(expected);
        }
        [Test]
        public void RemoveMethods_RemoveItemsFromEmptyCollection_ReturnFalse()
        {
            _list = new();

            bool actual1 = _list.RemoveFirst();
            bool actual2 = _list.RemoveLast();

            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }
        [Test]
        public void FindMethods_FindElementThatNotInCollection_ReturnNull()
        {
            LinkedListNode<int> actual1 = _list.Find(99);
            LinkedListNode<int> actual2 = _list.FindLast(99);

            actual1.Should().BeNull();
            actual2.Should().BeNull();
        }

        #endregion
    }
}