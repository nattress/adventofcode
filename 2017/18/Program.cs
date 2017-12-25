using System;
using System.Collections.Generic;
using System.IO;

namespace _18
{
    class Registers
    {
        public long LastSound = 0;
        public Dictionary<string, long> _Registers = new Dictionary<string, long>();
        public int _pc = 0;
        public Queue<long> _ReceiveQueue = new Queue<long>();
        public int _Id = 0;
        public bool Blocked = false;
        public Registers(int programId)
        {
            _Id = programId;
            string all = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < 26; i++)
            {
                _Registers[all.Substring(i, 1)] = 0;
            }
            _Registers["p"] = programId;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> instructions = new List<string>();
            Registers[] programs = new Registers[2];
            programs[0] = new Registers(0);
            programs[1] = new Registers(1);

            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    instructions.Add(line);
                }
            }
            int program1SendCount = 0;
            int instructionCounter = 0;
            int currentProgramId = 0;
            while (true)
            {
                if (programs[0].Blocked && programs[1].Blocked && programs[0]._ReceiveQueue.Count == 0 && programs[1]._ReceiveQueue.Count == 0)
                {
                    break;
                }
                Registers r = programs[currentProgramId];
                string[] op = instructions[r._pc].Split(" ");
                instructionCounter++;
                switch (op[0])
                {
                    case "snd":
                    {
                        programs[1 - currentProgramId]._ReceiveQueue.Enqueue(GetValue(op[1], r));
                        if (currentProgramId == 1)
                            program1SendCount++;
                        break;
                    }
                    case "set":
                    {
                        long y = GetValue(op[2], r);
                        r._Registers[op[1]] = y;
                        break;
                    }
                    case "add":
                    {
                        long y = GetValue(op[2], r);
                        r._Registers[op[1]] = r._Registers[op[1]] + y;
                        break;
                    }
                    case "mul":
                    {
                        long y = GetValue(op[2], r);
                        r._Registers[op[1]] = r._Registers[op[1]] * y;
                        break;
                    }
                    case "mod":
                    {
                        long y = GetValue(op[2], r);
                        r._Registers[op[1]] = r._Registers[op[1]] % y;
                        break;
                    }
                    case "rcv":
                    {
                        if (r._ReceiveQueue.Count > 0)
                        {
                            r.Blocked = false;
                            r._Registers[op[1]] = r._ReceiveQueue.Dequeue();
                        }
                        else
                        {
                            //Block on receive - switch context
                            r.Blocked = true;
                            currentProgramId = 1 - currentProgramId;
                            continue;
                        }
                        
                        break;
                    }
                    case "jgz":
                    {
                        long x = GetValue(op[1], r);
                        long y = GetValue(op[2], r);
                        if (x > 0)
                        {
                            // jump y offset
                            r._pc += (int)y;
                            continue;
                        }
                        break;
                    }
                }

                r._pc++;
            }

            Console.WriteLine($"Program 1 sent a value {program1SendCount} times.");
        }

        public static long GetValue(string registerOrValue, Registers r)
        {
            if (int.TryParse(registerOrValue, out int result))
            {
                return result;
            }
            else
            {
                return r._Registers[registerOrValue];
            }
        }
    }
}
