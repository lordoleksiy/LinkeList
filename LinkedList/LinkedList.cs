using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class LinkedList<T> : ICollection<T>
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
        public LinkedList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            foreach (T item in collection)
            {
                Add(item);
            }
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

        #region ICollection<T>
        public int Count => count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            LinkedListNode<T> node = new(item);
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
        }
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                if (runner.Value.Equals(item)) return true;
                runner = runner.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (head == null) throw new ArgumentNullException();
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (array.Length - index < count) throw new ArgumentException("index");

            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                array[index++] = runner.Value;
                runner = runner.Next;
            }
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                if (runner.Value.Equals(item))
                {
                    runner.Previous.Next = runner.Next;
                    runner.Next.Previous = runner.Previous;
                    count--;
                    return true;
                }
                runner = runner.Next;
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
        #endregion

        #region ICollection
        public bool IsSynchronized => false;

        public object SyncRoot
        {
            get
            {
                System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                return _syncRoot;
            }
        }
        public void CopyTo(Array array, int index)
        {
            if (head == null) throw new ArgumentNullException();
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (array.Length - index < count) throw new ArgumentException("index");

            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                array.SetValue(runner.Value, index++);
                runner = runner.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            set
            {
                if (index < 0 || index >= count) throw new IndexOutOfRangeException("index");
                LinkedListNode<T> pointer = head;
                for (int i = 0; i < index; i++)
                {
                    pointer = pointer.Next;
                }
                pointer.Value = value;
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
            LinkedListNode<T> node = new(item);
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
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("node");
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
        }

        public void AddLast(T item)
        {
            LinkedListNode<T> node = new(item);
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
        }

        public void AddLast(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("node");
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
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (newNode == null) throw new ArgumentNullException("newNode");
            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                if (runner.Equals(node))
                {
                    if (runner.Next != null)
                    {
                        runner.Next.Previous = newNode;
                    }
                    newNode.Previous = runner;
                    newNode.Next = runner.Next;
                    runner.Next = newNode;
                    if (runner == tail) tail = newNode;
                    count++;
                    return;
                }
                runner = runner.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddAfter(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException("node");
            LinkedListNode<T> runner = head;
            LinkedListNode<T> newNode = new(newItem);
            while (runner != null)
            {
                if (runner.Equals(node))
                {
                    if (runner.Next != null)
                    {
                        runner.Next.Previous = newNode;
                    }
                    newNode.Previous = runner;
                    newNode.Next = runner.Next;
                    runner.Next = newNode;
                    if (runner == tail) tail = newNode;
                    count++;
                    return;
                }
                runner = runner.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (newNode == null) throw new ArgumentNullException("newNode");
            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                if (runner.Equals(node))
                {
                    if (runner.Previous != null)
                    {
                        runner.Previous.Next = newNode;
                    }
                    newNode.Previous = runner.Previous;
                    newNode.Next = runner;
                    runner.Previous = newNode;
                    if (runner == head) head = newNode;
                    count++;
                    return;
                }
                runner = runner.Next;
            }
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException("node");
            LinkedListNode<T> runner = head;
            LinkedListNode<T> newNode = new(newItem);
            while (runner != null)
            {
                if (runner.Equals(node))
                {
                    if (runner.Previous != null)
                    {
                        runner.Previous.Next = newNode;
                    }
                    newNode.Previous = runner.Previous;
                    newNode.Next = runner;
                    runner.Previous = newNode;
                    if (runner == head) head = newNode;
                    count++;
                    return;
                }
                runner = runner.Next;
            }
            count++;
        }

        public bool RemoveFirst()
        {
            if (head == null) return false;
            head = head.Next;
            head.Previous = null;
            count--;
            return true;
        }
        public bool RemoveLast()
        {
            if (tail == null) return false;
            tail = tail.Previous;
            tail.Next = null;
            count--;
            return true;
        }

        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> runner = head;
            while (runner != null)
            {
                if (runner.Value.Equals(value))
                {
                    return runner;
                }
                runner = runner.Next;
            }
            return null;
        }
        public LinkedListNode<T> FindLast(T value)
        {
            LinkedListNode<T> runner = tail;
            while (runner != null)
            {
                if (runner.Value.Equals(value))
                {
                    return runner;
                }
                runner = runner.Previous;
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
        #endregion
    }
}
