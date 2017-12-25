using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _16
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(args[0]);
            // string input = "s1,x3/4,pe/b";
            string[] moves = input.Split(",");
            char[] dancers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            // char[] dancers = new char[] { 'a', 'b', 'c', 'd', 'e' };
            Dictionary<char, int> dancerPosition = new Dictionary<char, int>();
            
            for (int i = 0; i < dancers.Length; i++)
            {
                dancerPosition[dancers[i]] = i;
            }
            Dictionary<char, int> originalDancerPositions = new Dictionary<char, int>(dancerPosition);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            long lastReportedMs = 0;
            
            for (int ctr = 0; ctr < 1000000000; ctr++)
            {
                foreach (var m in moves)
                {
                    string command = m.Substring(0, 1);
                    string data = m.Substring(1);
                    if (command == "s")
                    {
                        int count = int.Parse(data);
                        //
                        // Copy last 'count' to temp buffer
                        //
                        char[] temp = new char[count];
                        for (int i = dancers.Length - count, j = 0; i < dancers.Length; i++, j++)
                        {
                            temp[j] = dancers[i];
                        }
                        //
                        // Move input buffer characters before 'count' to end
                        //
                        for (int i = dancers.Length - count - 1, j = dancers.Length - 1; i >= 0; i--, j--)
                        {
                            dancers[j] = dancers[i];
                            dancerPosition[dancers[j]] = j;
                        }
                        //
                        // Move temp buffer characters
                        //
                        for (int i = 0; i < temp.Length; i++)
                        {
                            dancers[i] = temp[i];
                            dancerPosition[dancers[i]] = i;
                        }
                    }
                    else if (command == "x")
                    {
                        var x = data.Split("/");
                        int pos1 = int.Parse(x[0]);
                        int pos2 = int.Parse(x[1]);
                        char temp = dancers[pos1];
                        dancers[pos1] = dancers[pos2];
                        dancers[pos2] = temp;
                        dancerPosition[dancers[pos1]] = pos1;
                        dancerPosition[dancers[pos2]] = pos2;
                    }
                    else if (command == "p")
                    {
                        var x = data.Split("/");
                        char char1 = char.Parse(x[0]);
                        char char2 = char.Parse(x[1]);
                        int pos1 = dancerPosition[char1];
                        int pos2 = dancerPosition[char2];
                        char temp = dancers[pos1];
                        dancers[pos1] = dancers[pos2];
                        dancers[pos2] = temp;
                        dancerPosition[dancers[pos1]] = pos1;
                        dancerPosition[dancers[pos2]] = pos2;
                    }
                }
                Console.WriteLine($"Part 1: Dancers ended up as {string.Join("", dancers)}");
            }

            Console.WriteLine($"Part 1: Dancers ended up as {string.Join("", dancers)}");

            foreach (var m in moves)
            {
                string command = m.Substring(0, 1);
                string data = m.Substring(1);
                if (command == "s")
                {
                    int count = int.Parse(data);
                    //
                    // Copy last 'count' to temp buffer
                    //
                    char[] temp = new char[count];
                    for (int i = dancers.Length - count, j = 0; i < dancers.Length; i++, j++)
                    {
                        temp[j] = dancers[i];
                    }
                    //
                    // Move input buffer characters before 'count' to end
                    //
                    for (int i = dancers.Length - count - 1, j = dancers.Length - 1; i >= 0; i--, j--)
                    {
                        dancers[j] = dancers[i];
                        dancerPosition[dancers[j]] = j;
                    }
                    //
                    // Move temp buffer characters
                    //
                    for (int i = 0; i < temp.Length; i++)
                    {
                        dancers[i] = temp[i];
                        dancerPosition[dancers[i]] = i;
                    }
                }
                else if (command == "x")
                {
                    var x = data.Split("/");
                    int pos1 = int.Parse(x[0]);
                    int pos2 = int.Parse(x[1]);
                    char temp = dancers[pos1];
                    dancers[pos1] = dancers[pos2];
                    dancers[pos2] = temp;
                    dancerPosition[dancers[pos1]] = pos1;
                    dancerPosition[dancers[pos2]] = pos2;
                }
                else if (command == "p")
                {
                    var x = data.Split("/");
                    char char1 = char.Parse(x[0]);
                    char char2 = char.Parse(x[1]);
                    int pos1 = dancerPosition[char1];
                    int pos2 = dancerPosition[char2];
                    char temp = dancers[pos1];
                    dancers[pos1] = dancers[pos2];
                    dancers[pos2] = temp;
                    dancerPosition[dancers[pos1]] = pos1;
                    dancerPosition[dancers[pos2]] = pos2;
                }
            }
            //
            // Part 2: Let's not run the full logic 1 BILLION times and
            // instead compute a mapping from the input to output for each 
            // character. Then run that 1 billion times, which should be
            // much faster.
            //
            int[] translationArray = new int[dancers.Length];
            for (int i = 0; i < dancers.Length; i++)
            {
                translationArray[i] = originalDancerPositions[dancers[i]];
            }
            
            char[] tempArray = new char[dancers.Length];
            for (int i = 1; i < 50; i++)
            {
                // Back up the current dancers array, then use the backup
                // to perform the translation since the dancers array elements
                // will be overwritten
                for (int j = 0; j < dancers.Length; j++)
                {
                    tempArray[j] = dancers[j];
                }

                for (int j = 0; j < dancers.Length; j++)
                {
                    dancers[j] = tempArray[translationArray[j]];
                }

                if (sw.ElapsedMilliseconds - lastReportedMs > 10000)
                {
                    lastReportedMs = sw.ElapsedMilliseconds;
                    Console.WriteLine($"{i} / 1000000000 after {sw.ElapsedMilliseconds}ms");
                }
                Console.WriteLine($"Part 2: Dancers ended up as {string.Join("", dancers)}");
            }

            Console.WriteLine($"Part 2: Dancers ended up as {string.Join("", dancers)}");
        }
    }
}
