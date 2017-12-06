using System;
using System.Collections.Generic;
using System.IO;

namespace day6
{
    class MemoryLayout
    {
        List<int> _bank = new List<int>();

        public MemoryLayout(int[] memory)
        {
            foreach (int x in memory)
            {
                _bank.Add(x);
            }
        }
        public override bool Equals(object obj)
        {
            MemoryLayout other = obj as MemoryLayout;

            if (other == null)
                return false;

            if (_bank.Count != other._bank.Count)
                return false;

            for (int i = 0; i < _bank.Count; i++)
            {
                if (_bank[i] != other._bank[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < _bank.Count; i++)
            {
                hash += _bank[i] ^ 13;
            }

            return hash;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string input = "10	3	15	10	5	15	5	15	9	2	5	8	5	2	3	6";
            //string input = "0\t2\t7\t0";
            string[] stringBlobs = input.Split("\t");

            List<int> bank = new List<int>();
            foreach (var x in stringBlobs)
            {
                bank.Add(int.Parse(x));
            }
            Dictionary<MemoryLayout, int> previousLayouts = new Dictionary<MemoryLayout, int>();
            int numRedistributions = 0;
            while(true)
            {
                int lowestTieBreaker = -1;
                int lowest = int.MinValue;

                for (int i = 0; i < bank.Count; i++)
                {
                    if (bank[i] > lowest)
                    {
                        lowestTieBreaker = i;
                        lowest = bank[i];
                    }
                }

                int restributionIndex = GetNextIndex(lowestTieBreaker, bank.Count - 1);
                int blocksLeft = lowest;
                bank[lowestTieBreaker] = 0;
                while (blocksLeft > 0)
                {
                    bank[restributionIndex]++;
                    blocksLeft --;

                    restributionIndex = GetNextIndex(restributionIndex, bank.Count - 1);
                }

                numRedistributions++;
                // Save this layout
                MemoryLayout newLayout = new MemoryLayout(bank.ToArray());
                if (previousLayouts.ContainsKey(newLayout))
                {
                    Console.WriteLine($"Number of redistributions: {numRedistributions}");
                    Console.WriteLine($"Dict of redistributions: {numRedistributions - previousLayouts[newLayout]}");
                    return;
                }

                previousLayouts.Add(new MemoryLayout(bank.ToArray()), numRedistributions);
                
            }
        }

        static int GetNextIndex(int current, int max)
        {
            int next = current + 1;
            if (next > max)
                next = 0;

            return next;
        }
    }
}
