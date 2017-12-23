using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _12
{
    class Node
    {
        public int Id {get;set;}
        public List<int> Reachable {get;set;}

        public Node (int id, List<int> reachable)
        {
            Id = id;
            Reachable = reachable;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Node> nodes = new Dictionary<int, Node>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string pattern = "([0-9]+)( <-> )(.+)";
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    var matches = Regex.Matches(line, pattern);
                    foreach (Match m in matches)
                    {
                        int index = int.Parse(m.Groups[1].Value);
                        string reachablelist = m.Groups[3].Value;
                        Console.WriteLine($"Index {index} -> {reachablelist}");
                        List<int> reachable = new List<int>();
                        
                        foreach (var x in reachablelist.Replace(" ", "").Split(","))
                        {
                            reachable.Add(int.Parse(x));
                        }
                        Node n = new Node(index, reachable);
                        nodes.Add(index, n);
                    }
                }
            }

            List<int> remainingNodesToFindGroupsFrom = new List<int>();
            foreach (var x in nodes)
                remainingNodesToFindGroupsFrom.Add(x.Key);
            
            int groupCount = 0;
            while (remainingNodesToFindGroupsFrom.Count > 0)
            {
                var reachableNodes = FindNodesReachableFromIndex(nodes, remainingNodesToFindGroupsFrom[0]);
                groupCount++;
                foreach (var x in reachableNodes)
                {
                    remainingNodesToFindGroupsFrom.Remove(x.Id);
                }
            }

            Console.WriteLine($"Found {groupCount} groups.");
            
        }

        static HashSet<Node> FindNodesReachableFromIndex(Dictionary<int, Node> nodes, int index)
        {
            HashSet<Node> reachableSet = new HashSet<Node>();
            HashSet<int> visited = new HashSet<int>();
            Queue<int> toVisit = new Queue<int>();

            toVisit.Enqueue(index);

            while (toVisit.Count > 0)
            {
                int currentIndex = toVisit.Dequeue();
                reachableSet.Add(nodes[currentIndex]);
                visited.Add(currentIndex);

                foreach (var x in nodes[currentIndex].Reachable)
                {
                    // Don't re-visit nodes to avoid loops
                    if (visited.Contains(x))
                        continue;
                    
                    // Don't queue stuff multiple times to avoid loops
                    if (toVisit.Contains(x))
                        continue;

                    toVisit.Enqueue(x);
                }
            }

            return reachableSet;
        }
    }
}
