using System;
using System.Threading;
using LinkedList;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //  Тест ввода:
            LinkedList<int> vs = new(4, 2, 8, 11, 28, 11, 3);
            vs.Add(19);  // 4 2 8 11 28 11 3 19
            vs.AddFirst(10);    // 10 4 2 8 11 28 11 3 19
            vs.AddLast(9);  //  10 4 2 8 11 28 11 3 19 9

            LinkedListNode<int> node = vs.Find(28);
            vs.AddAfter(node, 16);  // 10 4 2 8 11 28 16 11 3 19 9
            vs.AddBefore(node, 17);     // 10 4 2 8 11 17 28 16 11 3 19 9
            
            LinkedListNode<int> node1 = new(vs[0]);
            vs.AddBefore(vs.FindLast(11), node1);   // 10 4 2 8 11 17 28 16 10 11 3 19 9
            LinkedListNode<int> node2 = new(33);
            vs.AddAfter(vs.Get(2), node2);  // 10 4 33 2 8 11 17 28 16 10 11 3 19 9

            foreach (int i in vs)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine("\nCount: " + vs.Count);
            Console.WriteLine("First: " + vs.First);
            Console.WriteLine("Last: " + vs.Last);

            // Деякі тести:
            vs.RemoveLast();    // 10 4 33 2 8 11 17 28 16 10 11 3 19
            vs.RemoveFirst();   // 4 33 2 8 11 17 28 16 10 11 3 19
            vs.Remove(28);  // 4 33 2 8 11 17 16 10 11 3 19
            foreach (int i in vs)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine("\ncontains 19: " + vs.Contains(19));
            Console.WriteLine("contains 88: " + vs.Contains(88));

            int[] list = new int[12];
            vs.CopyTo(list, 1);
            Console.Write("test CopyTo: ");
            foreach (int i in list)
            {
                Console.Write($"{i} ");
            }

            vs.Clear();
            Console.WriteLine("\nCount: " + vs.Count);

            vs = new(4, 2, 43, 12, 43, 1, 2, 43);

            // Тест синхронізації потоків:
            int x = 0;
            Console.WriteLine("\nSync Root:");
            object locker = new();  // объект-заглушка
                                    // запускаем пять потоков
            for (int i = 1; i < 6; i++)
            {
                Thread myThread = new(Print);
                myThread.Name = $"Thread {i}";
                myThread.Start();
            }


            void Print()
            {
                lock (vs.SyncRoot)
                {
                    foreach (int i in vs)
                    {
                        Console.Write($"{i * x} ");
                    }
                    x++;
                    Console.WriteLine($" - thread {x}");
                }
            }


            //Тест Events:
            //Console.WriteLine("\nEvents: ");
            //void Changed()
            //{
            //    Console.WriteLine("The collection is changed!");
            //}
            //vs.EventAdd += Changed;
            //vs.Add(43);
            //foreach (int i in vs)
            //{
            //    Console.Write($"{i} ");
            //}
        }
    }
}
