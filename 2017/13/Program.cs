using System;
using System.Collections.Generic;
using System.IO;

namespace _13
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> layerToRange = new Dictionary<int, int>();
            int maxLayer = 0;
            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                
                    string[] lineData = line.Replace(" ", "").Split(":");
                    int layer = int.Parse(lineData[0]);
                    layerToRange.Add(layer, int.Parse(lineData[1]));
                    maxLayer = Math.Max(layer, maxLayer);

                    
                }
            }

            //T  0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16
            //   0  1  2  3  4  3  2  1  0  1  2  3  4  3  2  1  0
            //   |5| => 0, 8, 16
            //
            //T  0  1  2  3  4  5  6  7  8  9  10 11 12
            //   0  1  2  3  2  1  0  1  2  3  2  1  0
            //   |4| => 0, 6, 12
            //
            //T  0  1  2  3  4  5  6  7  8  9  10 11 12
            //   0  1  2  1  0  1  2  1  0  1  2  1  0
            //   |3| => 0, 4, 8, 12
            //
            //T  0  1  2  3  4  5  6  7  8  9  10 11 12
            //   0  1  0  1  0  1  0
            
            // Cycle length:
            // 2 * R - 2 ?
            int maxLayerSoFar = 0;
            int delay = 0;
            while (true)
            {
                Console.WriteLine($"Trying delay of {delay}");
                int tripSeverity = 0;
                int layer = 0;
                for (int t = delay; t <= maxLayer + delay; t++)
                {
                    // Is there a scanner at this layer?
                    if (layerToRange.ContainsKey(layer))
                    {
                        // Is the scanner at the top of the range according to the formula
                        // 2 * R - 2 == 0 mod t
                        if (t == 0 || (t % (2 * layerToRange[layer] - 2) == 0))
                        {
                            // Caught!
                            maxLayerSoFar = Math.Max(maxLayerSoFar, layer);
                            tripSeverity += t * layerToRange[layer];
                            Console.WriteLine($"Caught at layer {layer}. Max so far {maxLayerSoFar}");

                            // For Part 2 we only consider delays with 0 times getting caught.
                            if (delay > 0)
                                break;
                        }
                    }
                    layer++;
                }

                // Part 1
                if (delay == 0)
                {
                    Console.WriteLine($"Trip severity: {tripSeverity}");
                }
                if (tripSeverity == 0)
                {
                    Console.WriteLine($"A delay of {delay} allows the packet to pass");
                    return;
                }

                delay++;
            }

            
        }
    }
}
