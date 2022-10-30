using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList.Tests
{
    public interface ITestEvent<T>
    {
        void testOneArgumentEvent(LinkedListNode<T> element);
        void testNoArgumentEvent();
    }
}
