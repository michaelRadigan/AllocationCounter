using System;
using System.Collections.Generic;

namespace AllocationCounter
{
    static class Foo
    {
        static void Simple(string[] args)
        {
            var eventListener = new SimpleEventListener();


            var count = 0;
            for (var i = 0; i < 10; i++)
            {
                count += Baz().Length;

                for (var j = 0; j < 10; j++)
                {
                    count += Bar().Count;
                }
            }

            Console.WriteLine($"Hello World! count is {count}");
        }


        private static int[] Baz()
        {
            var a = new[] {0, 1, 2, 3, 4, 5};
            return a;
        }


        private static List<int> Bar()
        {
            var b = new List<int> {0, 1, 2};
            var c = new List<int> {3, 4, 5};
            b.AddRange(c);
            return b;
        }
    }
}