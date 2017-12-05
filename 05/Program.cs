using System;
using System.Collections.Generic;
using System.IO;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> instructions = new List<int>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                while(true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    instructions.Add(int.Parse(line));
                }
            }

            int stepCount = 0;
            int position = 0;
            while (true)
            {
                stepCount++;
                int numToJump = instructions[position];

                if (numToJump >= 3)
                {
                    instructions[position] -= 1;    
                }
                else
                    instructions[position]++;

                position += numToJump;

                if (position >= instructions.Count)
                {
                    Console.WriteLine($"{stepCount} jumps.");
                    return;
                }
            }

        }
    }
}
