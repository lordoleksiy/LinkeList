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
        public LinkedListNode<T> Next { get; internal set; } = null;
        public LinkedListNode<T> Previous { get; internal set; } = null;
        public LinkedList.LinkedList<T> List { get; internal set; }

        public LinkedListNode(T value)
        {
            Value = value;
            List = null;
        }

        internal LinkedListNode(T value, LinkedList<T> list)
        {
            Value = value;
            List = list;
        }

        override public String ToString()
        {
            return Value.ToString();
        }
        
    }
}
