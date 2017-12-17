using System;
using System.Collections.Generic;
using System.IO;

namespace _17
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> items = new List<int>();
            items.Add(0);
            int index = 0;
            int iterationCount = 328;
            for (int i = 1; i <= 2017; i++)
            {
                index += iterationCount;
                index = index % i;
                // if (index >= items.Count)
                //     index -= iterationCount;
                items.Insert((index + 1 % i), i);
                index++;
            }
            Console.WriteLine("2017 inserted at index " + index);
            Console.WriteLine(items[index+1]);

            Main2();
        }

        static void Main2()
        {
            //
            // Don't insert 50 million times...
            // 0 will always remain at index 0 because the circular buffer won't insert before the 0th index
            // Just keep track of what gets inserted at index 1
            //
            int index = 0;
            int iterationCount = 328;
            int itemAfter0 = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                index += iterationCount;
                index = index % i;

                if (((index + 1) % i) == 1)
                {
                    itemAfter0 = i;
                }
                index++;
            }
            
            Console.WriteLine($"Item after 0: {itemAfter0}");
        }
    }
}
