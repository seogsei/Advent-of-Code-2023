namespace AOC2023
{
    internal static class Day11
    {
        public static long Compute()
        {
            string data = ReadData();
            var split = data.Split(Environment.NewLine);

            List<int> rows = [], columns = [];
            List<Coordinate> galaxies = [];
            List<(Coordinate, Coordinate)> galaxyPairs = [];

            //Find the galaxy pairs
            for (int j = 0; j < split.Length; j++)
                for (int i = 0; i < split[j].Length; i++)
                    if (split[j][i] == '#')
                    {
                        var galaxy = (j, i);
                        galaxies.ForEach((other) => galaxyPairs.Add((galaxy, other)));
                        galaxies.Add(galaxy);
                    }


            for (int j = 0; j < split.Length; j++)
                if (!split[j].Contains('#'))
                    rows.Add(j);

            for (int i = 0; i < split[0].Length; i++)
            {
                bool add = true;
                for (int j = 0; j < split.Length; j++)
                    if (split[j][i] == '#')
                    {
                        add = false;
                        break;
                    }
                if (add)
                    columns.Add(i);
            }

            //Part 1
            /*
            return galaxyPairs.Sum((pair) => Distance(pair.Item1.X, pair.Item2.X, (i) => rows.Contains(i) ? 2 : 1)
                                            + Distance(pair.Item1.Y, pair.Item2.Y, (i) => columns.Contains(i) ? 2 : 1));
            */

            //Part 2

            return galaxyPairs.Sum((pair) => Distance(pair.Item1.X, pair.Item2.X, (i) => rows.Contains(i) ? 1_000_000 : 1)
                                           + Distance(pair.Item1.Y, pair.Item2.Y, (i) => columns.Contains(i) ? 1_000_000 : 1));
        }

        private static long Distance(int p1, int p2, Func<int, int> costFunction)
        {
            int a = Math.Min(p1, p2);
            int b = Math.Max(p1, p2);

            long sum = 0;
            for (int i = a; i < b; i++)
                sum += costFunction(i);

            return sum;
        }

        private static string ReadData()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day11.txt";

            using StreamReader dataStream = new(File.OpenRead(path));
            return dataStream.ReadToEnd();
        }
    }

    internal record struct Coordinate(int X, int Y)
    {
        public static implicit operator (int, int)(Coordinate value)
        {
            return (value.X, value.Y);
        }

        public static implicit operator Coordinate((int, int) value)
        {
            return new Coordinate(value.Item1, value.Item2);
        }
    }
}
