using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LinkedList
{
    public class LinkedList<T> : ICollection<T>
    {
        #region Properties
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;
        private int count;
        private object _syncRoot;
        private readonly Mutex mutex = new();
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
        public event Action EventAdd = delegate { };
        public event Action EventRemove = delegate { };
        public event Action EventClear = delegate { };

        protected virtual void OnAdd()
        {
            EventAdd.Invoke();
        }

        protected virtual void OnRemove()
        {
            EventRemove.Invoke();
        }

        protected virtual void OnClear()
        {
            EventClear.Invoke();
        }

        #endregion

        #region ICollection<T>
        public int Count => count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            mutex.WaitOne();
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
            OnAdd();
            mutex.ReleaseMutex();
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
                if (curNode.Value.Equals(item)) return true;
                curNode = curNode.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (head == null) throw new ArgumentNullException("collection");
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index below zero");
            if (array.Length - index < count) throw new ArgumentOutOfRangeException("index higher than count");

            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                array[index++] = curNode.Value;
                curNode = curNode.Next;
            }
        }

        public bool Remove(T item)
        {
            mutex.WaitOne();
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (curNode.Value.Equals(item))
                {
                    curNode.Previous.Next = curNode.Next;
                    curNode.Next.Previous = curNode.Previous;
                    count--;
                    OnRemove();
                    mutex.ReleaseMutex();
                    return true;
                }
                curNode = curNode.Next;
            }
            mutex.ReleaseMutex();
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            mutex.WaitOne();
            LinkedListNode<T> current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
            mutex.ReleaseMutex();
        }
        #endregion

        #region ICollection
        public bool IsSynchronized => true;

        public object SyncRoot
        {
            get
            {
                mutex.WaitOne();
                if (_syncRoot == null)
                {
                    _syncRoot = new object();
                }
                mutex.ReleaseMutex();
                return _syncRoot;
            }
        }
        public void CopyTo(Array array, int index)
        {
            if (head == null) throw new ArgumentNullException("collection");
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index below zero");
            if (array.Length - index < count) throw new ArgumentOutOfRangeException("index higher than count");

            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                array.SetValue(curNode.Value, index++);
                curNode = curNode.Next;
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
                if (index < 0) throw new IndexOutOfRangeException("index below zero");
                if (index >= count) throw new IndexOutOfRangeException("index higher than count");
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
            mutex.WaitOne();
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
            OnAdd();
            mutex.ReleaseMutex();
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException();
            if (node.List != null) throw new InvalidOperationException();
            mutex.WaitOne();
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
            OnAdd();
            mutex.ReleaseMutex();
        }

        public void AddLast(T item)
        {
            mutex.WaitOne();
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
            OnAdd();
            mutex.WaitOne();
        }

        public void AddLast(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException();
            if (node.List != null) throw new InvalidOperationException();
            mutex.WaitOne();
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
            OnAdd();
            mutex.ReleaseMutex();
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null || newNode == null) throw new ArgumentNullException();
            if (newNode.List != null) throw new InvalidOperationException();
            mutex.WaitOne();
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
                    OnAdd();
                    mutex.ReleaseMutex();
                    return;
                }
                curNode = curNode.Next;
            }
            mutex.ReleaseMutex();
            throw new InvalidOperationException("No item has been found");
        }

        public void AddAfter(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException();
            mutex.WaitOne();
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
                    OnAdd();
                    mutex.ReleaseMutex();
                    return;
                }
                curNode = curNode.Next;
            }
            mutex.ReleaseMutex();
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null || newNode == null) throw new ArgumentNullException();
            if (newNode.List != null) throw new InvalidOperationException();
            mutex.WaitOne();
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
                    OnAdd();
                    mutex.ReleaseMutex();
                    return;
                }
                curNode = curNode.Next;
            }
            mutex.ReleaseMutex();
            throw new InvalidOperationException("No item has been found");
        }

        public void AddBefore(LinkedListNode<T> node, T newItem)
        {
            if (node == null) throw new ArgumentNullException();
            mutex.WaitOne();
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
                    OnAdd();
                    mutex.ReleaseMutex();
                    return;
                }
                curNode = curNode.Next;
            }
            mutex.ReleaseMutex();
            throw new InvalidOperationException("No item has been found");
        }

        public bool RemoveFirst()
        {
            if (head == null) return false;
            mutex.WaitOne();
            head = head.Next;
            head.Previous = null;
            count--;
            OnRemove();
            mutex.ReleaseMutex();
            return true;
        }
        public bool RemoveLast()
        {
            if (tail == null) return false;
            mutex.WaitOne();
            tail = tail.Previous;
            tail.Next = null;
            count--;
            OnRemove();
            mutex.ReleaseMutex();
            return true;
        }

        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> curNode = head;
            while (curNode != null)
            {
                if (curNode.Value.Equals(value))
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
                if (curNode.Value.Equals(value))
                {
                    return curNode;
                }
                curNode = curNode.Previous;
            }
            return null;
        }
        public LinkedListNode<T> Get(int index)
        {
            if (index < 0) throw new IndexOutOfRangeException("index below zero");
            if (index >= count) throw new IndexOutOfRangeException("index higher than count");
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
