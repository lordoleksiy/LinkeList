using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace LinkedList
{
    public class LinkedList<T> : ICollection<T>, ICollection
    {
        #region Properties
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;
        private int count;
        private object _syncRoot;
        #endregion

        #region Constructors

        public LinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public LinkedList(params T[] items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (var item in items)
            {
                Add(item);
            }
        }
        #endregion

        #region Events
        public event EventHandler<LinkedListNode<T>> EventAdd = delegate { };  // shall i use EventHandler?
        public event EventHandler<LinkedListNode<T>> EventRemove = delegate { };
        public event Action EventClear = delegate { };

        protected virtual void OnAdd(LinkedListNode<T> element)
        {
            EventAdd.Invoke(this, element);
        }

        protected virtual void OnRemove(LinkedListNode<T> element)
        {
            EventRemove.Invoke(this, element);
        }

        protected virtual void OnClear()
        {
            EventClear.Invoke();
        }

        #endregion

        #region ICollection<T>

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            LinkedListNode<T> node = new(item, this);
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                node.Previous = tail;
                tail.Next = node;
                tail = node;
            }
            count++;
            OnAdd(node);
        }
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
            OnClear();
        }

        public bool Contains(T item)
        {
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (Compare(curNode.Value, item)) return true;
                curNode = curNode.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (head == null) throw new ArgumentNullException("empty collection");
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (array.Length - index < count) throw new ArgumentOutOfRangeException("array");

            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                array[index++] = curNode.Value;
                curNode = curNode.Next;
            }
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (Compare(curNode.Value, item))
                {
                    curNode.List = null;
                    OnRemove(curNode);
                    if (curNode.Previous == null)
                    {
                        head = head.Next;
                        head.Previous = null;
                    }
                    else
                    {
                        curNode.Previous.Next = curNode.Next;
                    }
                        
                    if (curNode.Next != null)
                    {
                        curNode.Next.Previous = curNode.Previous;
                    }
                        
                    count--;
                    return true;
                }
                curNode = curNode.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection

        public int Count => count;
        public bool IsSynchronized => false;

        public object SyncRoot
        {
            get
            {
                Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        public void CopyTo(Array array, int index)
        {
            if (head == null) throw new ArgumentNullException("empty collection");
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (array.Length - index < count) throw new ArgumentOutOfRangeException("array");

            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                array.SetValue(curNode.Value, index++);
                curNode = curNode.Next;
            }
        }
        #endregion

        #region MyMethods
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count) throw new IndexOutOfRangeException("index");
                LinkedListNode<T> pointer = head;
                for (int i = 0; i < index; i++)
                {
                    pointer = pointer.Next;
                }
                return pointer.Value;
            }
        }
        public T First
        {
            get
            {
                if (head == null) throw new InvalidOperationException();
                return head.Value;
            }
        }
        public T Last
        {
            get
            {
                if (tail == null) throw new InvalidOperationException();
                return tail.Value;
            }
        }
        public void AddFirst(T item)
        {
            LinkedListNode<T> node = new(item, this);
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                head.Previous = node;
                node.Next = head;
                head = node;
            }
            count++;
            OnAdd(node);
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("newNode");
            if (node.List != null) throw new InvalidOperationException("newNode");
            node.List = this;
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                head.Previous = node;
                node.Next = head;
                head = node;
            }
            count++;
            OnAdd(node);
        }

        public void AddLast(T item)
        {
            LinkedListNode<T> node = new(item, this);
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.Next = node;
                node.Previous = tail;
                tail = node;
            }
            count++;
            OnAdd(node);
        }

        public void AddLast(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("newNode");
            if (node.List != null) throw new InvalidOperationException("newNode");
            node.List = this;
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.Next = node;
                node.Previous = tail;
                tail = node;
            }
            count++;
            OnAdd(node);
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (newNode == null) throw new ArgumentNullException("newNode");
            if (newNode.List != null) throw new InvalidOperationException("newNode");
            LinkedListNode<T> curNode = head;
            newNode.List = this;
            while (curNode != null)
            {
                if (curNode.Equals(node))
                {
                    if (curNode.Next != null)
                    {
                        curNode.Next.Previous = newNode;
                    }
                    newNode.Previous = curNode;
                    newNode.Next = curNode.Next;
                    curNode.Next = newNode;
                    if (curNode == tail) tail = newNode;
                    count++;
                    OnAdd(newNode);
                    return;
                }
                curNode = curNode.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddAfter(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException("node");
            LinkedListNode<T> curNode = head;
            LinkedListNode<T> newNode = new(newItem, this);
            while (curNode != null)
            {
                if (curNode.Equals(node))
                {
                    if (curNode.Next != null)
                    {
                        curNode.Next.Previous = newNode;
                    }
                    newNode.Previous = curNode;
                    newNode.Next = curNode.Next;
                    curNode.Next = newNode;
                    if (curNode == tail) tail = newNode;
                    count++;
                    OnAdd(newNode);
                    return;
                }
                curNode = curNode.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (newNode == null) throw new ArgumentNullException("newNode");
            if (newNode.List != null) throw new InvalidOperationException("newNode");

            newNode.List = this;
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (curNode.Equals(node))
                {
                    if (curNode.Previous != null)
                    {
                        curNode.Previous.Next = newNode;
                    }
                    newNode.Previous = curNode.Previous;
                    newNode.Next = curNode;
                    curNode.Previous = newNode;
                    if (curNode == head) head = newNode;
                    count++;
                    OnAdd(newNode);
                    return;
                }
                curNode = curNode.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException("node");
            LinkedListNode<T> curNode = head;
            LinkedListNode<T> newNode = new(newItem, this);
            while (curNode != null)
            {
                if (curNode.Equals(node))
                {
                    if (curNode.Previous != null)
                    {
                        curNode.Previous.Next = newNode;
                    }
                    newNode.Previous = curNode.Previous;
                    newNode.Next = curNode;
                    curNode.Previous = newNode;
                    if (curNode == head) head = newNode;
                    count++;
                    OnAdd(newNode);
                    return;
                }
                curNode = curNode.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public bool RemoveFirst()
        {
            if (head == null) return false;
            head.List = null;
            OnRemove(head);
            head = head.Next;
            head.Previous = null;
            count--;
            return true;
        }
        public bool RemoveLast()
        {
            if (tail == null) return false;
            tail.List = null;
            OnRemove(tail);
            tail = tail.Previous;
            tail.Next = null;
            count--;
            return true;
        }

        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (Compare(curNode.Value, value))
                {
                    return curNode;
                }
                curNode = curNode.Next;
            }
            return null;
        }
        public LinkedListNode<T> FindLast(T value)
        {
            LinkedListNode<T> curNode = tail;
            while (curNode != null)
            {
                if (Compare(curNode.Value, value))
                {
                    return curNode;
                }
                curNode = curNode.Previous;
            }
            return null;
        }
        public LinkedListNode<T> Get(int index)
        {
            if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("index");
            LinkedListNode<T> pointer = head;
            for (int i = 0; i < index; i++)
            {
                pointer = pointer.Next;
            }
            return pointer;
        }
        private static bool Compare(T item1, T item2)
        {
            return EqualityComparer<T>.Default.Equals(item1, item2);
        }
        #endregion
    }
}