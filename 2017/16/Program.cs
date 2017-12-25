using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _16
{
    class Program
    {
        enum CommandType
        {
            Spin,
            Exchange,
            Partner
        }

        class Command{
            public CommandType CommandType;
            public int Arg0;
            public int Arg1;
            public char CharArg0;
            public char CharArg1;

            public Command(string command, string data)
            {
                if (command == "s")
                {
                    CommandType = CommandType.Spin;
                    Arg0 = int.Parse(data);
                }
                else if (command == "x")
                {
                    CommandType = CommandType.Exchange;
                    var x = data.Split("/");
                    Arg0 = int.Parse(x[0]);
                    Arg1 = int.Parse(x[1]);
                }
                else if (command == "p")
                {
                    CommandType = CommandType.Partner;
                    var x = data.Split("/");
                    CharArg0 = char.Parse(x[0]);
                    CharArg1 = char.Parse(x[1]);
                }
            }
        }
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
            List<Command> commands = new List<Command>();
            foreach (var m in moves)
            {
                string command = m.Substring(0, 1);
                string data = m.Substring(1);
                commands.Add(new Command(command, data));
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            long lastReportedMs = 0;
            char[] temp = new char[dancers.Length];
            List<string> previousPositions = new List<string>();
            for (int ctr = 0; ctr < 1000000000; ctr++)
            {
                foreach (var c in commands)
                {
                    switch (c.CommandType)
                    {
                        case CommandType.Spin:
                        {
                            //
                            // Copy last 'count' to temp buffer
                            //
                            int count = c.Arg0;
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
                            for (int i = 0; i < count; i++)
                            {
                                dancers[i] = temp[i];
                                dancerPosition[dancers[i]] = i;
                            }
                            break;
                        }
                        case CommandType.Exchange:
                        {
                            int pos1 = c.Arg0;
                            int pos2 = c.Arg1;
                            char temp2 = dancers[pos1];
                            dancers[pos1] = dancers[pos2];
                            dancers[pos2] = temp2;
                            dancerPosition[dancers[pos1]] = pos1;
                            dancerPosition[dancers[pos2]] = pos2;
                            break;
                        }
                        case CommandType.Partner:
                        {
                            char char1 = c.CharArg0;
                            char char2 = c.CharArg1;
                            int pos1 = dancerPosition[char1];
                            int pos2 = dancerPosition[char2];
                            char temp3 = dancers[pos1];
                            dancers[pos1] = dancers[pos2];
                            dancers[pos2] = temp3;
                            dancerPosition[dancers[pos1]] = pos1;
                            dancerPosition[dancers[pos2]] = pos2;
                            break;
                        }
                    }
                }
                
                if (previousPositions.Contains(string.Join("", dancers)))
                {
                    Console.WriteLine($"Found a repetition after {ctr} repetitions");
                    break;
                }
                else
                {
                    previousPositions.Add(string.Join("", dancers));
                }
                
                if (ctr == 0)
                {
                    Console.WriteLine($"Part 1: Dancers ended up as {string.Join("", dancers)}");
                }

                if (sw.ElapsedMilliseconds - lastReportedMs > 10000)
                {
                    Console.WriteLine($"{(double)((double)ctr / 1000000000.0f) * 100.0}% - {ctr} / 1000000000");
                    lastReportedMs = sw.ElapsedMilliseconds;
                }
            }

            Console.WriteLine($"Part 2: Dancers ended up as {previousPositions[(1000000000 % 60) - 1]}");
        }
    }
}
