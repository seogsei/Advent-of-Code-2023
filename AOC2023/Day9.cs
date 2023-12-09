namespace AOC2023
{
    internal static class Day9
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day9Input.txt";
            using StreamReader dataStream = new(File.OpenRead(path));

            IEnumerable<IEnumerable<int>> data = ReadData(dataStream);

            //Part 1
            //long sum = data.Sum(FindNextNumber);

            //Part 2
            long sum = data.Sum(FindPreviousNumber);
           
            return sum;
        }

        private static int FindNextNumber(IEnumerable<int> input)
        {
            if (input.All((x) => x == 0))
                return 0;

            return input.Last() + FindNextNumber(CollapseSequence(input));
        }

        private static int FindPreviousNumber(IEnumerable<int> input)
        {
            if (input.All((x) => x == 0))
                return 0;

            return input.First() - FindPreviousNumber(CollapseSequence(input));
        }

        private static IEnumerable<int> CollapseSequence(IEnumerable<int> input)
        {
            return input.Zip(input.Skip(1), (x, y) => y - x);
        }

        private static IEnumerable<IEnumerable<int>> ReadData(StreamReader dataStream)
        {
            string? line;
            while((line = dataStream.ReadLine()) != null)
            {
                yield return line.Split(' ').Select(int.Parse);
            }
        }
    }
}
