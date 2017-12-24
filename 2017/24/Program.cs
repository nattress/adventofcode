using System;
using System.Collections.Generic;
using System.IO;

namespace _24
{
    struct Component
    {
        public int X {get;set;}
        public int Y {get;set;}

        public Component(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Component> components = new List<Component>();
            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    string[] split = line.Split("/");
                    components.Add(new Component(int.Parse(split[0]), int.Parse(split[1])));
                    
                }
            }
            var excludeList = new List<Component>();
            List<int> strengths = new List<int>();
            Dictionary<int, int> lengthsToStrengths = new Dictionary<int, int>();
            BuildBridge(components, 0, excludeList, strengths, lengthsToStrengths);
            int longest = 0;
            foreach (var x in lengthsToStrengths)
            {
                if (longest < x.Key)
                {
                    longest = x.Key;
                }
            }

            Console.WriteLine($"Longest is {longest} with strength {lengthsToStrengths[longest]}");
            strengths.Sort();
            Console.WriteLine($"Strongest: {strengths[0]}. Weakest: {strengths[strengths.Count - 1]}");
        }

        static void BuildBridge(List<Component> components, int nextPort, List<Component> partialBridge, List<int> strengths, Dictionary<int, int> lengthsToStrengths)
        {
            var nextComponents = FindComponentsWithPort(components, nextPort, partialBridge);

            if (nextComponents.Count == 0)
            {
                // Built a bridge we can't extend
                int sum = 0;
                foreach (var x in partialBridge)
                {
                    sum += x.X + x.Y;
                }
                
                if (lengthsToStrengths.ContainsKey(partialBridge.Count))
                {
                    lengthsToStrengths[partialBridge.Count] = Math.Max(lengthsToStrengths[partialBridge.Count], sum);
                }
                else
                {
                    lengthsToStrengths.Add(partialBridge.Count, sum);
                }
                
                strengths.Add(sum);
            }
            foreach (var x in nextComponents)
            {
                partialBridge.Add(x);
                int newNextPort = x.X;
                if (newNextPort == nextPort)
                    newNextPort = x.Y;
                
                BuildBridge(components, newNextPort, partialBridge, strengths, lengthsToStrengths);
                partialBridge.Remove(x);
            }
        }
        static List<Component> FindComponentsWithPort(List<Component> components, int port, List<Component> exclude)
        {
            List<Component> result = new List<Component>();

            foreach (var x in components)
            {
                if ((x.X == port || x.Y == port) && !exclude.Contains(x))
                    result.Add(x);
            }

            return result;
        }
    }
}
