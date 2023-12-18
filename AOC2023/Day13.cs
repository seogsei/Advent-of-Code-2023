namespace AOC2023
{
    internal static class Day13
    {
        const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day13.txt";

        public static int Compute()
        {
            var data = Utilities.ReadFile(path).Split(Environment.NewLine + Environment.NewLine);

            //Part 1 
            //return data.Sum((x) => GetReflectionValue(x, 0));
            
            //Part 2
            return data.Sum((x) => GetReflectionValue(x, 1));
        }

        private static int GetReflectionValue(string data, int numberOfSmudges = 0)
        {
            var lines = data.Split(Environment.NewLine);

            //Check for horizontal mirror
            int reflectionIdx = GetHorizontalReflectionIndex(lines, numberOfSmudges);
            if (reflectionIdx > 0)
                return reflectionIdx * 100;
           
            //Flip the data from vertical to horizontal
            List<string> horizontalLines = [];
            for (int i = 0; i < lines[0].Length; i++)
                horizontalLines.Add(string.Concat(lines.Select((line) => line[i])));

            //Check for vertical mirror using the same method
            reflectionIdx = GetHorizontalReflectionIndex(horizontalLines, numberOfSmudges);
            if (reflectionIdx > 0)
                return reflectionIdx;

            throw new Exception();
        }

        private static int GetHorizontalReflectionIndex(IEnumerable<IEnumerable<char>> data, int numberOfSmudges = 0)
        {
            for (int i = 1; i < data.Count(); i++)
            {
                int smudgeCount = 0;
                foreach (var (left, right) in data.Take(i).Reverse().Zip(data.Skip(i)))
                {
                    if (left is null || right is null)
                        break;

                    smudgeCount += left.Zip(right).Sum((tuple) => tuple.First != tuple.Second ? 1 : 0);

                    if (smudgeCount > numberOfSmudges)
                        break;
                }

                if (smudgeCount == numberOfSmudges)
                    return i;
            }

            return 0;
        }
    }
}
