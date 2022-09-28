using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{

    public sealed class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; set; } = null;
        public LinkedListNode<T> Previous { get; set; } = null;

        override public String ToString()
        {
            return Value.ToString();
        }
        public LinkedListNode(T value)
        {
            Value = value;
        }
    }
}
