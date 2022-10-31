using NUnit.Framework;
using LinkedList;
using System;
using FluentAssertions;
using System.Collections;
using Moq;

namespace LinkedList.Tests
{
    [TestFixture]
    public class Tests
    {
        private LinkedList<int> _list;

        [OneTimeSetUp]
        public void SetUp()
        {
            _list = new();
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

        #region Events
        [Test]
        public void EventClear_ClearCollectionAndCheckClearEventIsRaised_EventClearIsRaised()
        {
            LinkedList<int> list = new(1, 2, 3);
            using var monitoredEvents = list.Monitor();
            ITestEvent<int> fakeObject = Mock.Of<ITestEvent<int>>();

            list.EventClear += fakeObject.testNoArgumentEvent;
            list.Clear();

            monitoredEvents.Should().Raise("EventClear");
        }

        [Test]
        public void EventAdd_AddNumToCollectionAndMonitorEventIsRaisedAndArgCorrect_EventISRaisedArgCorrect()
        {
            LinkedList<int> list = new();
            using var monitoredEvents = list.Monitor();
            ITestEvent<int> fakeObject = Mock.Of<ITestEvent<int>>();
            int expected = 2;

            list.EventAdd += fakeObject.testOneArgumentEvent;
            list.Add(expected);

            monitoredEvents.Should().Raise("EventAdd")
                .WithArgs<LinkedListNode<int>>(args => args.Value == expected);
        }

        [Test]
        public void EventRemove_RemoveNumFromCollectionAndMonitorEventIsRaisedAndArgCorrect_EventISRaisedArgCorrect()
        {
            LinkedList<int> list = new(6, 2, 4, 6, 1);
            using var monitoredEvents = list.Monitor();
            ITestEvent<int> fakeObject = Mock.Of<ITestEvent<int>>();
            int expected = 2;

            //list.EventRemove += fakeObject.testOneArgumentEvent;
            list.Remove(expected);

            monitoredEvents.Should().Raise("EventRemove")
                .WithArgs<LinkedListNode<int>>(args => args.Value == expected);
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
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expectedCount = 0;

            list.Clear();

            list.Should()
                .HaveCount(expectedCount).And
                .BeEmpty();
        }

        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 3, true)]
        [TestCase(new int[] { 3, 1, 5 }, 1, true)]
        [TestCase(new int[] { 9 }, 9, true)]
        [TestCase(new int[] { 4, 2, 5, 2, 3, 1 }, 10, false)]
        [TestCase(new int[] { }, 1, false)]
        public void Contains_ContainsVariousNum_ReturnEqualsExpected(int[] massive, int num, bool expected)
        {
            LinkedList<int> list = new(massive);

            bool actual = list.Contains(num);

            actual.Should().Be(expected);

        }

        [Test]
        public void CopyTo_CopyToMassiveSameElementsAsInList_MassiveEqualsExpeñted()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int[] actual = new int[5];
            int[] expected = new int[] { 10, 8, 4, 9, 2 };

            list.CopyTo(actual, 0);

            actual.Should().BeEquivalentTo(expected);

        }
        [Test]
        public void CopyTo_CopyToEmptyCollection_ArgumentNullException()
        {
            LinkedList<int> list = new();
            int[] massive = new int[5];

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("empty collection");
        }
        [Test]
        public void CopyTo_CopyToNullMassive_ArgumentNullException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int[] massive = null;

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("array");
        }
        [Test]
        public void CopyTo_CopyToWitnIdexBelowZero_ArgumentOutOfRange()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int[] massive = new int[5];

            Action action = () => list.CopyTo(massive, -1);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index");
        }
        [Test]
        public void CopyTo_CopyToMassiveWithSizeLessThanCount_ArgumentException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int[] massive = new int[1];

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("array");
        }


        [TestCase(4)]
        [TestCase(10)]
        [TestCase(2)]
        public void Remove_RemoveNumInList_ContainsNumFalseAndResOfRemoveIsTrue(int num)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);

            bool actual = list.Remove(num);

            list.Should().NotContain(num);
            actual.Should().BeTrue();
        }
        [Test]
        public void Remove_RemoveNotContainedNum_ReturnFalse()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);

            bool result = list.Remove(6);

            result.Should().BeFalse();
        }

        [Test]
        public void IsReadOnly_ReturnFalse()
        {
            bool actual = _list.IsReadOnly;

            actual.Should().BeFalse();
        }
        [Test]
        public void GetEnumerator()
        {
            LinkedList<int> list = new(4, 2, 5, 3, 5, 1);

            foreach (int i in list)
            {

            }
        }

        [Test]
        public void IEnumerableGetEnumerator()
        {
            LinkedList<int> list = new(4, 2, 5, 3, 5, 1);
            IEnumerable test = list;
            
            foreach (int i in test) { }

            
        }
        #endregion

        #region ICollection
        [TestCase(new int[] { 1, 2, 3, 4 }, 4)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 1, 2 }, 2)]
        public void Count_CountOfCollectionIsEqualsToLengthOfMatrix(int[] massive, int expected)
        {
            LinkedList<int> list = new(massive);

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
        public void CopyToLegacy_CopyToMassiveSameElementsAsInList_MassiveEqualsExpeñted()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            Array actual = new int[5];
            Array expected = new int[] { 10, 8, 4, 9, 2 };

            list.CopyTo(actual, 0);

            actual.Should().BeEquivalentTo(expected);

        }
        [Test]
        public void CopyToLegacy_CopyToEmptyCollection_ArgumentNullException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            list = new();
            Array massive = new int[5];

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("empty collection");
        }
        [Test]
        public void CopyToLegacy_CopyToNullMassive_ArgumentNullException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            Array massive = null;

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("array");
        }
        [Test]
        public void CopyToLegacy_CopyToWitnIdexBelowZero_ArgumentOutOfRange()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            Array massive = new int[5];

            Action action = () => list.CopyTo(massive, -1);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index");
        }
        [Test]
        public void CopyToLegacy_CopyToMassiveWithSizeLessThanCount_ArgumentException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            Array massive = new int[1];

            Action action = () => list.CopyTo(massive, 0);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("array");
        }

        #endregion

        #region MyMethods
        [TestCase(0, 10)]
        [TestCase(2, 4)]
        [TestCase(4, 2)]
        public void This_ThisReturnsCorrectNum(int index, int expected)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);

            int actual = list[index];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void Get_GetReturnsCorrectNode(int index)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expectedVal = list[index];


            LinkedListNode<int> actual = list.Get(index);

            actual.List.Should().BeEquivalentTo(list);
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
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = list[0];

            int actual = list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void First_FirstElementButCollectionIsEmpty_InvalidOperationException()
        {
            int actual;

            Action action = () => actual = _list.First;

            action.Should().Throw<InvalidOperationException>();
        }
        [Test]
        public void Last_ReturnsLastElemOfCollection_LastElemEqualsNum()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = list[list.Count - 1];

            int actual = list.Last;

            actual.Should().Be(expected);
        }

        [Test]
        public void Last_LastElementButCollectionIsEmpty_InvalidOperationException()
        {
            int actual;

            Action action = () => actual = _list.Last;

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void AddFirst_AddFirstItemToEmptyCollection_FirstEqualsToAdded()
        {
            LinkedList<int> list = new();
            int expected = 99;

            list.AddFirst(99);
            int actual = list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstItemToStart_FirstEqualsToAdded()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = 99;

            list.AddFirst(99);
            int actual = list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFLast_AddLastItemToEmptyCollection_LastEqualsToAdded()
        {
            LinkedList<int> list = new();
            int expected = 99;

            list.AddLast(99);
            int actual = list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastTItemToEnd_LastEqualsToAdded()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = 99;

            list.AddLast(99);
            int actual = list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstNodeToEmptyCollection_FirstEqualsToAdded()
        {
            LinkedList<int> list = new();
            LinkedListNode<int> added = new(99);
            int expected = 99;

            list.AddFirst(added);
            int actual = list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddFirst_AddFirstNodeToStart_FirstEqualsToAdded()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> added = new(99);
            int expected = 99;


            list.AddFirst(added);
            int actual = list.First;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastNodeToEmptyCollection_LastEqualsToAdded()
        {
            LinkedList<int> list = new();
            LinkedListNode<int> added = new(99);
            int expected = 99;

            list.AddLast(added);
            int actual = list.Last;

            actual.Should().Be(expected);
        }
        [Test]
        public void AddLast_AddLastNodeToEnd_LastEqualsToAdded()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> added = new(99);
            int expected = 99;


            list.AddLast(added);
            int actual = list.Last;

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddAfter_AddAfterTItem_ElemOnNextIndexEqualsExpected(int index)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = list.Get(index);
            int expected = 99;

            list.AddAfter(node, 99);
            int actual = list[index + 1];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddAfter_AddAfterNode_ElemOnNextIndexEqualsExpected(int index)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = list.Get(index);
            LinkedListNode<int> newNode = new(99);
            int expected = 99;

            list.AddAfter(node, newNode);
            int actual = list[index + 1];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddBefore_AddBeforeTItem_ElemOnPrevIndexEqualsExpected(int index)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = list.Get(index);
            int expected = 99;

            list.AddBefore(node, 99);
            int actual = list[index];

            actual.Should().Be(expected);
        }
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(4)]
        public void AddBefore_AddBeforeNode_ElemOnIndexEqualsBefore(int index)
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = list.Get(index);
            LinkedListNode<int> newNode = new(99);
            int expected = 99;

            list.AddBefore(node, newNode);
            int actual = list[index];

            actual.Should().Be(expected);
        }
        [Test]
        public void AddBeforeAddAfter_AddWhenNodeInCollectionNull_ArgumentNullException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> newNode = new(10);
            int newItem = 10;

            Action action1 = () => list.AddBefore(null, newNode);
            Action action2 = () => list.AddAfter(null, newNode);
            Action action3 = () => list.AddBefore(null, newItem);
            Action action4 = () => list.AddAfter(null, newItem);

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
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = list.Get(1);

            Action action1 = () => list.AddFirst(null);
            Action action2 = () => list.AddLast(null);
            Action action3 = () => list.AddBefore(node, null);
            Action action4 = () => list.AddAfter(node, null);

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
        public void AddMethods_AddNodeWithFieldListNotNull_InvalidOperationException()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> newNode = new(10);
            LinkedListNode<int> node = list.Get(1);
            list.AddFirst(newNode);

            Action action1 = () => list.AddFirst(newNode);
            Action action2 = () => list.AddLast(newNode);
            Action action3 = () => list.AddAfter(node, newNode);
            Action action4 = () => list.AddBefore(node, newNode);

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
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            LinkedListNode<int> node = new(99);
            LinkedListNode<int> testNode1 = new(100);
            LinkedListNode<int> testNode2 = new(100);

            Action action1 = () => list.AddBefore(node, testNode1);
            Action action2 = () => list.AddBefore(node, 100);
            Action action3 = () => list.AddAfter(node, testNode2);
            Action action4 = () => list.AddAfter(node, 100);

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
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int removed = list.First;

            list.RemoveFirst();
            int actual = list.First;

            actual.Should().NotBe(removed);
        }

        [Test]
        public void Removelast_RemoveLastNodeFromCollection_NodeIsRemoved()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int removed = list.Last;

            list.RemoveLast();
            int actual = list.Last;


            actual.Should().NotBe(removed);
        }
        [Test]
        public void Find_FindNumber_FindNumberEqualsExcpected()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = 4;

            LinkedListNode<int> node =  list.Find(expected);
            int actual = node.Value;

            actual.Should().Be(expected);
        }
        [Test]
        public void FindLast_FindLastNumber_FindNumberEqualsExcpected()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);
            int expected = 4;

            LinkedListNode<int> node = list.FindLast(expected);
            int actual = node.Value;

            actual.Should().Be(expected);
        }
        [Test]
        public void RemoveMethods_RemoveItemsFromEmptyCollection_ReturnFalse()
        {
            LinkedList<int> list = new();

            bool actual1 = list.RemoveFirst();
            bool actual2 = list.RemoveLast();

            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }
        [Test]
        public void FindMethods_FindElementThatNotInCollection_ReturnNull()
        {
            LinkedList<int> list = new(10, 8, 4, 9, 2);

            LinkedListNode<int> actual1 = list.Find(99);
            LinkedListNode<int> actual2 = list.FindLast(99);

            actual1.Should().BeNull();
            actual2.Should().BeNull();
        }

        #endregion
    }
}