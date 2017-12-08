using System;
using System.IO;
using System.Collections.Generic;

namespace _08
{
    class Program
    {
        static Dictionary<string, int> _reg = new Dictionary<string, int>();
        static int largestEver = 0;
        static int GetReg(string register)
        {
            if (_reg.ContainsKey(register))
            {
                return _reg[register];
            }
            return 0;
        }
        static void Main(string[] args)
        {
            using (TextReader tr = File.OpenText(args[0]))
            {
                while(true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    string[] items = line.Split(" ");
                    string reg = items[0];
                    bool inc = false;
                    if (items[1] == "inc")
                        inc = true;

                    int val = int.Parse( items[2]);

                    string opReg = items[4];
                    string op = items[5];
                    int opVal = int.Parse(items[6]);

                    bool isConTrue = false;
                    if (op == ">=")
                    {
                        isConTrue = GetReg(opReg) >= opVal;
                    }
                    else if (op == "!=")
                    {
                        isConTrue = GetReg(opReg) != opVal;
                    }
                    else if (op == ">")
                    {
                        isConTrue = GetReg(opReg) > opVal;
                    }
                    else if (op == "<")
                    {
                        isConTrue = GetReg(opReg) < opVal;
                    }
                    else if (op == "<=")
                    {
                        isConTrue = GetReg(opReg) <= opVal;
                    }
                    else if (op == "==")
                    {
                        isConTrue = GetReg(opReg) == opVal;
                    }
                    else
                        System.Diagnostics.Debug.Assert(false);

                    if (isConTrue)
                    {
                        int curRegVal = GetReg(reg);
                        if (inc)
                        {
                            curRegVal += val;
                        }
                        else
                        {
                            curRegVal -= val;
                        }

                        _reg[reg] = curRegVal;
                        largestEver = Math.Max(largestEver, curRegVal);
                    }
                }
            }

            int largest = 0;
            foreach (var x in _reg.Values)
            {
                largest = Math.Max(largest, x);
            }

            Console.WriteLine("Largest val is " + largest);
            Console.WriteLine("Largest ever val is " + largestEver);
        }
    }
}
