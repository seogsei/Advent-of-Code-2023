using System;
using System.Diagnostics;
using System.Numerics;

namespace AOC2023
{
    internal static class Day3
    {
        public static int Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day3Input.txt";
            using StreamReader dataStream = new(File.OpenRead(path));

            return Part2(dataStream);
        }

        private static int Part1(StreamReader dataStream)
        {
            int sum = 0;

            string? previousLine = null;
            string? curLine = dataStream.ReadLine();
            string? nextLine = dataStream.ReadLine();

            int digitLength = 0;
            while (curLine is not null)
            {
                for (int i = 0; i < curLine.Length; i++)
                {
                    if (char.IsDigit(curLine[i]))
                    {
                        digitLength++;
                        continue;
                    }

                    if (digitLength == 0)
                        continue;

                    //We have found a number we need to read the value and check if its around a special character
                    int digitStart = i - digitLength;
                    sum += HandleDigit(curLine, digitStart, digitLength, previousLine, nextLine);
                    digitLength = 0;
                }

                if (digitLength > 0)
                {
                    int digitStart = curLine.Length - digitLength;
                    sum += HandleDigit(curLine, digitStart, digitLength, previousLine, nextLine);
                    digitLength = 0;
                }

                previousLine = curLine;
                curLine = nextLine;
                nextLine = dataStream.ReadLine();
            }

            return sum;
        }

        private static int Part2(StreamReader dataStream)
        {
            var sum = 0;

            string? previousLine = null;
            string? curLine = dataStream.ReadLine();
            string? nextLine = dataStream.ReadLine();

            while (curLine is not null)
            {
                Console.WriteLine(curLine);
                for (int i = 0; i < curLine.Length; i++)
                {
                    if (curLine[i] != '*')
                        continue;

                    var numbers = GetAdjacentNumbers(curLine, i, previousLine, nextLine);

                    if (numbers.Count == 2)
                    {
                        int gearRatio = numbers[0] * numbers[1];
                        sum += gearRatio;
                    }
                        
                }

                previousLine = curLine;
                curLine = nextLine;
                nextLine = dataStream.ReadLine();
            }

            return sum;
        }

        private static int HandleDigit(string curLine, int startIdx, int length, string? previousLine, string? nextLine)
        {
            if (CheckAroundForSpecials(previousLine, curLine, nextLine, startIdx, length))
                return int.Parse(curLine.Substring(startIdx, length));

            return 0;
        }

        private static bool CheckAroundForSpecials(string? previousString, string currentString, string? nextString, int startIdx, int lenght) 
        {
            //Check on the current string first
            if (startIdx > 0 && IsSpecialCharacter(currentString[startIdx - 1]))
                return true;

            if (startIdx + lenght < currentString.Length && IsSpecialCharacter(currentString[startIdx + lenght]))
                return true;

            int checkBoxStart = startIdx - 1;
            if (checkBoxStart < 0)
                checkBoxStart = 0;

            int checkBoxEnd = startIdx + lenght + 1;
            if (checkBoxEnd > currentString.Length)
                checkBoxEnd = currentString.Length;

            if (previousString is not null && ContainsSpecialCharacter(previousString[checkBoxStart..checkBoxEnd]))
                return true;

            if (nextString is not null && ContainsSpecialCharacter(nextString[checkBoxStart..checkBoxEnd]))
                return true;

            return false;
        }

        private static bool ContainsSpecialCharacter(string str)
        {
            foreach (var c in str)
                if (IsSpecialCharacter(c))
                    return true;

            return false;
        }

        private static bool IsSpecialCharacter(char c)
        {
            return c != '.' && !char.IsDigit(c);
        }

        private static List<int> GetAdjacentNumbers(string curLine, int idx, string? prevLine, string? nextLine)
        {
            List<int> numbers = [];

            if (prevLine is not null)
                GetNumbersOnLineAdjacent(prevLine, idx, ref numbers);

            GetNumbersOnLineAdjacent(curLine, idx, ref numbers);

            if (nextLine is not null)
                GetNumbersOnLineAdjacent(nextLine, idx, ref numbers);

            return numbers;
        }

        private static List<int> GetNumbersOnLineAdjacent(string line, int idx, ref List<int> result)
        {
            int i = idx;
            for (; i - 1 >= 0 && char.IsDigit(line[i - 1]); i--) { }

            int end = Math.Min(idx + 2, line.Length);
            for (; i < end; i++)
            {
                if (!char.IsDigit(line[i]))
                    continue;

                int number = 0;
                do
                {
                    number *= 10;
                    number += line[i] - '0';

                } while (++i < line.Length && char.IsDigit(line[i]));

                result.Add(number);
            }
            return result;
        }
    }
}
