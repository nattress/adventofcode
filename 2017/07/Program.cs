using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace _07
{
    class Program
    {
        class Node
        {
            public int Weight;
            public string Name;
            public List<string> Children = new List<string>();

            public Node(string name, int weight, List<string> children)
            {
                Name = name;
                Weight = weight;
                if (children != null)
                    Children = children;
            }
        }

        static Dictionary<string, Node> programs = new Dictionary<string, Node>();
        static void Main(string[] args)
        {
            Dictionary<string, int> inEdges = new Dictionary<string, int>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                while(true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    List<string> children = null;
                    string[] items = null;
                    if (line.Contains("->"))
                    {
                        children = new List<string>();
                        string[] tempItems = line.Split(" -> ");
                        string[] firstItems = tempItems[0].Split(" ");
                        string[] secondItems = tempItems[1].Split(", ");

                        List<string> heh = new List<string>();
                        heh.AddRange(firstItems);
                        heh.AddRange(secondItems);
                        items = heh.ToArray();


                        foreach (var x in secondItems)
                        {
                            children.Add(x);
                            if (inEdges.ContainsKey(x))
                            {
                                inEdges[x] = inEdges[x] + 1;
                            }
                            else
                            {
                                inEdges[x] = 1;
                            }
                        }
                    }
                    else
                    {
                        items = line.Split(" ");
                    }

                    if (!inEdges.ContainsKey(items[0]))
                    {
                        inEdges[items[0]] = 0;
                    }
                    items[1] = items[1].Replace("(", "");
                    items[1] = items[1].Replace(")", "");
                    
                    Node n = new Node(items[0], int.Parse(items[1]), children);
                    programs[items[0]] = n;
                }

                string root = "";
                // Look for an item with no in edges
                foreach (KeyValuePair<string, int> kvp in inEdges)
                {
                    if (kvp.Value == 0)
                    {
                        root = kvp.Key;
                        Console.WriteLine("Root: " + root);
                        break;
                    }
                }

                GetTowerWeight(programs[root]);
            }
        }

        static int GetTowerWeight(Node n)
        {
            int weight = n.Weight;
            Dictionary<string, int> nameToWeight = new Dictionary<string, int>();
            List<int> weights = new List<int>();
            for (int i = 0; i < n.Children.Count; i++)
            {
                int thisWeight = GetTowerWeight(programs[n.Children[i]]);
                weights.Add(thisWeight);
                nameToWeight[programs[n.Children[i]].Name] = thisWeight;
                weight += weights[i];
            }
            weights.Sort();

            if (weights.Count > 1 && weights[weights.Count - 1] != weights[weights.Count - 2])
            {
                Console.WriteLine("Found mismatch");
            }
            return weight;
        }
    }
}
