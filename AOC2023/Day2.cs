using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace AOC2023
{
    internal static class Day2
    {
        public static int Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day2Input.txt";

            //Handle the input
            var games = ParseData(path);

            int sum = 0;
            foreach (var game in games)
            {
                //For part 1
                //sum += game.Possible(12, 13, 14) ? game.ID : 0;

                //For part 2
                sum += game.MinimumRequired().GetPower();
            }
                
            return sum;
        }

        static List<GameModel> ParseData(string path)
        {
            List<GameModel> games = [];
            using StreamReader dataStream = new(File.OpenRead(path));
            string? line;
            while ((line = dataStream.ReadLine()) is not null)
                games.Add(ParseGame(line));

            return games;
        }

        static GameModel ParseGame(string line)
        {
            //Example line
            //Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            var parts = line.Split(':');

            //Read game id from the first part
            int gameID = ReadGameID(parts[0]);

            List<RevealModel> reveals = [];
            foreach(var str in parts[1].Split(';'))
                reveals.Add(ReadReveal(str));

            return new(gameID, reveals);
        }

        static int ReadGameID(string input)
        {
            return int.Parse(input[5..]);
        }

        static RevealModel ReadReveal(string input)
        {
            RevealModel reveal = new();
            foreach (var part in input.Split(","))
            {
                var split = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int count = int.Parse(split[0]);

                switch (split[1])
                {
                    case "red":
                        reveal.Red = count;
                        break;
                    case "green":
                        reveal.Green = count;
                        break;
                    case "blue":
                        reveal.Blue = count;
                        break;
                }
            }
            return reveal;
        }

        record class GameModel(int ID, List<RevealModel> Reveals)
        {
            public bool Possible(int red, int green, int blue)
            {
                foreach (var tuple in Reveals)
                {
                    if (tuple.Red > red || tuple.Green > green || tuple.Blue > blue)
                        return false;
                }
                return true;
            }

            public RevealModel MinimumRequired()
            {
                RevealModel values = new();

                foreach(var (red, green, blue) in Reveals)
                {
                    values.Red = Math.Max(values.Red, red);
                    values.Green = Math.Max(values.Green, green);
                    values.Blue = Math.Max(values.Blue, blue);
                }

                return values;
            }
        }

        record struct RevealModel(int Red, int Green, int Blue)
        {
            public readonly int GetPower()
            {
                return Red * Green * Blue;
            }
        }
    }

}
