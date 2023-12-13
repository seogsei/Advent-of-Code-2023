namespace AOC2023
{

    internal static class Day12
    {
        const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day12.txt";

        public static long Compute()
        {
            string data = ReadData(path);

            IEnumerable<string> split = data.Split(Environment.NewLine);

            IEnumerable<string> records = split.Select((x) => x.Split(' ')[0]);
            IEnumerable<IEnumerable<int>> codes = split.Select((x) => x.Split(' ')[1].Split(',').Select(int.Parse));

            AlternateWay(".???.???...?.???", codes.First().ToArray());

            //Part 1
            return records.Zip(codes).Sum((x) => GetPossibleArrangementCount(x.First, x.Second.ToArray()));

            //Part 2
            records = records.Select((x) => string.Join('?', Enumerable.Repeat(x, 5)));
            codes = codes.Select((x) => Enumerable.Repeat(x, 5).SelectMany(x => x));

            return records.Zip(codes).Sum((x) => GetPossibleArrangementCount(x.First, x.Second.ToArray()));
        }

        private static string ReadData(string path)
        {
            using StreamReader dataStream = new(File.OpenRead(path));
            return dataStream.ReadToEnd();
        }

        private static int GetPossibleArrangementCount(string record, int[] code)
        {
            return Inner(record, 0, code, 0, 0);

            static int Inner(string record, int recordPointer, int[] code, int codePointer, int consecutiveParts)
            {
                //Iterate through the word
                int sum = 0;
                for (; recordPointer < record.Length; recordPointer++)
                {
                    char c = record[recordPointer];

                    if (c == '?')
                    {
                        if (codePointer < code.Length && consecutiveParts < code[codePointer])
                        {
                            sum += Inner(record, recordPointer + 1, code, codePointer, consecutiveParts + 1);
                        }
                        /*
                        if (consecutiveParts == 0 || consecutiveParts == code[codePointer])
                        {
                            sum += Inner(record, recordPointer + 1, code, consecutiveParts > 0 ? codePointer + 1 : codePointer, 0);
                        }

                        return sum;
                        */
                    }

                    if (c == '?' || c == '.')
                    {
                        if (consecutiveParts == 0)
                            continue;

                        if (consecutiveParts != code[codePointer])
                            return sum;

                        codePointer++;
                        consecutiveParts = 0;
                    }
                    else if (c == '#')
                    {
                        if (codePointer >= code.Length)
                            return sum;

                        consecutiveParts++;
                        if (consecutiveParts > code[codePointer])
                            return sum;
                    }
                }

                //Handle the last sequence of '#'s
                if (consecutiveParts > 0)
                {
                    if (consecutiveParts != code[codePointer])
                        return sum;
                    codePointer++;
                }

                //We've reached the end of the word if the record is viable return sum of all the branches + 1 else return the branches
                return codePointer == code.Length ? sum + 1 : sum;
            }
        }

        private static int AlternateWay(ReadOnlySpan<char> record, IEnumerable<int> code)
        {
            record = record.Trim('.'); //Trimming '.' doesnt matter for the result

            if (record[0] == '#')
            {

            }

            for (int i = 0; i < code.First(); i++)
            {

            }

            return 0;
        }
    }
}
