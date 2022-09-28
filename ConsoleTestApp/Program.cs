using System;
using LinkedList;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LinkedList<int> vs = new(2, 4, 5, 2);
            foreach (int i in vs)
            {
                Console.WriteLine(i);
            }
        }
    }
}
