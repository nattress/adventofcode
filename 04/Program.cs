using System;
using System.IO;
using System.Collections.Generic;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            int numValid = 0;

            List<string> lines = new List<string>();
            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    if (IsLineValid(line))
                    {
                        numValid++;
                    }
                }
            }

            Console.WriteLine($"Num Valid: {numValid}");
        }

        static bool IsLineValid(string line)
        {
            string[] words = line.Split(" ");
            HashSet<string> usedWords = new HashSet<string>();
            HashSet<string> anagramWords = new HashSet<string>();
            foreach (string word in words)
            {
                if (usedWords.Contains(word))
                    return false;

                List<string> chars = new List<string>();
                for (int i = 0; i < word.Length; i++)
                {
                    chars.Add(word.Substring(i, 1));
                }
                chars.Sort();

                string sortedWord = "";
                foreach (var x in chars)
                {
                    sortedWord += x;
                }

                if (anagramWords.Contains(sortedWord))
                    return false;

                anagramWords.Add(sortedWord);
                usedWords.Add(word);
            }

            return true;
        }
    }
}
