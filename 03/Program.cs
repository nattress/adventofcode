using System;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            PositionalMemoryCellBank memoryBank = new PositionalMemoryCellBank();

            for (int i = 1; i < 265150; i++)
            {
                var cell = memoryBank.GetCellAtIndex(i);
                Console.WriteLine($"{i} => ({cell.Point.X}, {cell.Point.Y}) Distance {Math.Abs(cell.Point.X) + Math.Abs(cell.Point.Y)} Value {cell.Value}");
                if (cell.Value > 265149)
                {
                    Console.WriteLine("Solved the puzzle for the input");
                    break;
                }
            }
        }
    }
}
