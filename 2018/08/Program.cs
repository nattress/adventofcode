using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Node
    {
        public Node[] Children;
        public int[] Metadata;

        public Node(int childCount, int metadataCount)
        {
            Children = new Node[childCount];
            Metadata = new int[metadataCount];
        }

        public int SumMetadata()
        {
            int metadataSum = 0;

            // Current node
            foreach (var m in Metadata)
            {
                metadataSum += m;
            }

            // Children
            foreach (var c in Children)
            {
                metadataSum += c.SumMetadata();
            }

            return metadataSum;
        }

        public int GetNodeValue()
        {
            if (Children.Length == 0)
            {
                return Metadata.Sum();
            }

            int sum = 0;
            foreach (var m in Metadata)
            {
                if (m == 0 || m > Children.Length)
                    continue;

                sum += Children[m - 1].GetNodeValue();
            }

            return sum;
        }
    }

    public class NodeDataProvider
    {
        List<int> _data = new List<int>();
        int _position = 0;

        public NodeDataProvider(string input)
        {
            foreach (var x in input.Split(' '))
            {
                _data.Add(int.Parse(x));
            }
        }

        public int GetNext()
        {
            return _data[_position++];
        }
    }

    public class Day08
    {
        static void Main(string[] args)
        {
            string input = "";

            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    input = line;
                }
            }
            
            int part1 = Part1(new NodeDataProvider(input));
            int part2 = Part2(new NodeDataProvider(input));

            Console.WriteLine($"Part One: {part1}");
            Console.WriteLine($"Part Two: {part2}");
        }

        static Node BuildTree(NodeDataProvider input)
        {
            // Header
            int childCount = input.GetNext();
            int metadataCount = input.GetNext();

            Node newNode = new Node(childCount, metadataCount);
            for (int i = 0; i < childCount; i++)
            {
                newNode.Children[i] = BuildTree(input);
            }

            for (int i = 0; i < metadataCount; i++)
            {
                newNode.Metadata[i] = input.GetNext();
            }

            return newNode;
        }

        static int GetNodeValue(Node node)
        {
            if (node.Children.Length == 0)
            {
                return node.Metadata.Sum();
            }

            int sum = 0;
            foreach (var m in node.Metadata)
            {
                if (m == 0 || m > node.Children.Length)
                    continue;

                sum += GetNodeValue(node.Children[m - 1]);
            }

            return sum;
        }

        public static int Part1(NodeDataProvider input)
        {
            var root = BuildTree(input);
            return root.SumMetadata();
        }

        public static int Part2(NodeDataProvider input)
        {
            var root = BuildTree(input);
            return root.GetNodeValue();
        }
    }
}
