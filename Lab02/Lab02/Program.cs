using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace App
{
    class Program
    {
        private static List<string> words = new List<string>();

        public static void Main(string[] args)
        {
            int choice;

            while (true)
            {
                choice = UILoader.LoadMenu();

                if (choice != 0 && choice != 9)
                {
                    if(words.Count <= 0)
                    {
                        Console.WriteLine("Please load words first!");
                        Console.WriteLine("\n\nPress any key to continue...");
                        Console.ReadKey(true);
                        continue;
                    }
                }

                switch (choice)
                {
                    case 0:
                        // exe file locates in /bin/Debug, so the path should direct ../../
                        words = WordLoader.ReadFile("../../Words.txt");
                        Console.WriteLine($"The number of words is {words.Count}");

                        break;
                    case 1:
                        Stopwatch stopWatch_bubble = Stopwatch.StartNew();
                        Functions.BubbleSort(words);
                        stopWatch_bubble.Stop();

                        Console.WriteLine($"Execution Time: {stopWatch_bubble.ElapsedMilliseconds} ms");
                        break;
                    case 2:
                        Stopwatch stopWatch_linq = Stopwatch.StartNew();
                        Functions.LINQ_LambdaSort(words);
                        stopWatch_linq.Stop();

                        Console.WriteLine($"Execution Time: {stopWatch_linq.ElapsedMilliseconds} ms");
                        break;
                    case 3:
                        int countDistinct = Functions.DistinctWordCount(words);
                        Console.WriteLine($"Distinct count in {countDistinct}");
                        break;
                    case 4:
                        Console.WriteLine("The first 10 words are:");
                        Functions.Get10Words(words).ForEach(Console.WriteLine);
                        break;
                    case 5:
                        Console.WriteLine("The words printed from end to beginning are:");
                        Functions.Reverse(words).ForEach(Console.WriteLine);
                        break;
                    case 6:
                        var lastAList = Functions.FindLastWithA(words);
                        Console.WriteLine($"The {lastAList.Count} words that end with 'a' are:");
                        lastAList.ForEach(Console.WriteLine);
                        break;
                    case 7:
                        var firstMList = Functions.FindFirstWithM(words);
                        Console.WriteLine($"The {firstMList.Count} words that start with 'm' are:");
                        firstMList.ForEach(Console.WriteLine);
                        break;
                    case 8:
                        var findS = Functions.FindS(words);
                        Console.WriteLine($"The {findS.Count} words that have more than 5 characters and contain the letter 's' are:");
                        findS.ForEach(Console.WriteLine);
                        break;
                    case 9:
                        Console.WriteLine("\n\nPress any key to exit...");
                        Console.ReadKey(true);
                        return;
                }

                Console.WriteLine("\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
    }

    class Functions
    {
        public static List<string> FindS(List<string> words)
        {
            return words.Where(w => w.Length > 5 && w.Contains('s')).ToList();
        }
        public static List<string> FindFirstWithM(List<string> words)
        {
            return words.Where(w => w[0] == 'm').ToList();
        }
        public static List<string> FindLastWithA(List<string> words)
        {
            return words.Where(w => w[w.Length - 1] == 'a').ToList();
        }
        public static List<string> Reverse(List<string> words)
        {
            return words.Select(w => new string(w.Reverse().ToArray())).ToList();
        }
        public static List<string> Get10Words(List<string> words)
        {
            return words.Take(10).ToList();       
        }
        public static int DistinctWordCount(List<string> words)
        {
            List<string> distinctWords = new List<string>(words);
            return distinctWords.Distinct().Count();
        }
        public static List<string> LINQ_LambdaSort(List<string> words)
        {
            var sorted = words.OrderBy(w => w).ToList<string>();

            return sorted;
        }

        public static List<string> BubbleSort(List<string> words)
        {
            List<string> sorted = new List<string>(words);

            for(int i=0; i<sorted.Count-1; ++i)
            {
                for(int j=0;j<sorted.Count-i-1; ++j)
                {
                    if (Compare(sorted[j], sorted[j+1]) > 0)
                    {
                        string temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                      
                }
            }

            return sorted;
        }

        private static int Compare(string a, string b)
        {
            int i = 0;
            while(i < a.Length && i < b.Length)
            {
                if (a[i] != b[i]) return a[i] - b[i];
                i++;
            }
            return a.Length - b.Length;
        }
    }

    class UILoader
    {
        private static string title = "Choose an option (Move: ↑ ↓, Select: Enter)";
        private static List<string> menus = new List<string>
        {
            "Import words from file",
            "Bubble sort words",
            "LINQ/Lambda sort words",
            "Count the distinct words",
            "Take the first 10 words",
            "Reverse each word and print the list",
            "Get add display words that end with 'a' and display the count",
            "Get and display words that start with 'm' and display the count",
            "Get and display words that are more than 5 characters long and contain the letter 'f', and display the count",
            "Exit"
        };

        public static int LoadMenu()
        {
            Console.CursorVisible = false;

            int choice = SelectMenu();

            return choice;
        }

        private static int SelectMenu()
        {
            int index = 0;

            while (true)
            {
                Console.Clear();

                Console.WriteLine(title);
                
                for(int i=0; i<menus.Count; ++i)
                {
                    if(i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {menus[i]}");
                        Console.ResetColor();
                    } else
                    {
                        Console.WriteLine($"  {menus[i]}");
                    }
                }
                Console.WriteLine();

                var key = Console.ReadKey(true).Key;

                if(key == ConsoleKey.UpArrow)
                {
                    index = (index - 1 + menus.Count) % menus.Count;
                } 
                else if(key == ConsoleKey.DownArrow)
                {
                    index = (index + 1 + menus.Count) % menus.Count;
                }
                else if(key == ConsoleKey.Enter)
                {
                    return index;
                }
            }


        }
    }


    class WordLoader
    {
        public static List<string> ReadFile(string fileName)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length > 0)
                    {
                        words.Add(line);
                    }
                }
            }

            return words;
        }
    }

}