using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace AOC2023
{
    internal static class Day4
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day4Input.txt";
            using StreamReader dataStream = new(File.OpenRead(path));

            List<ScratchCard> cards = ReadData(dataStream);

            //Part 1
            //return (long)cards.Sum((card) => card.CalculateScore());

            //Part2
            for (int i = 0; i < cards.Count; i++)
                for (int j = 1; j <= cards[i].NumberOfMatches(); j++)
                    cards[i + j].AddCopy(cards[i].Copies);

            return cards.Sum((card) => card.Copies);
        }

        private static List<ScratchCard> ReadData(StreamReader dataStream)
        {
            List<ScratchCard> cards = [];

            string? line;
            while ((line = dataStream.ReadLine()) is not null)
            {
                cards.Add(ReadCard(line));
            }

            return cards;
        }

        private static ScratchCard ReadCard(string input)
        {
            var numbers = input.Split(':')[1].Split('|');

            return new(ReadNumbers(numbers[0]), ReadNumbers(numbers[1]));
        }

        private static List<int> ReadNumbers(string input)
        {
            List<int> list = [];

            string[] numbers = input.Split(' ',StringSplitOptions.RemoveEmptyEntries);

            foreach (var number in numbers)
                list.Add(int.Parse(number));

            return list;
        }

        private record class ScratchCard(List<int> WinningNumbers, List<int> CandidateNumbers)
        {
            public int Copies { get; private set; } = 1;

            public void AddCopy(int count) => Copies += count;

            public int NumberOfMatches()
            {
                return CandidateNumbers.Where(WinningNumbers.Contains).Count();
            }

            public int CalculateScore()
            {
                int matches = NumberOfMatches();  

                return matches == 0 ? 0 : 1 << (matches - 1);
            }
        }
    }
}
