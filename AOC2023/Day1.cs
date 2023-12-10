namespace AOC2023
{
    internal static class Day1
    {
        static readonly string[] arr = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

        public static int Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day1Input.txt";

            int sum = 0;

            using StreamReader dataStream = new(File.OpenRead(path));

            string? line;
            while ((line = dataStream.ReadLine()) is not null)
                sum += GetNumberFromLine(line);

            return sum;
        }

        static int GetNumberFromLine(string line)
        {
            int first = ReadDigitFromLine(line);
            int second = ReadDigitFromLine(line, true);

            Console.WriteLine($"{line} : {first}{second}");

            return first * 10 + second;
        }

        static int ReadDigitFromLine(string line, bool reverse = false)
        {
            int i = reverse ? line.Length - 1 : 0;
            int step = reverse ? -1 : 1;
            Func<int, int, bool> predicate = reverse ? (x, y) => { return x >= 0; }
            :
                                                       (x, y) => { return x < y; };

            for (; predicate(i, line.Length); i += step)
            {
                if (char.IsDigit(line[i]))
                    return line[i] - '0';

                string subStr = line[i..];
                for (int j = 1; j < arr.Length; j++)
                    if (subStr.StartsWith(arr[j]))
                        return j;
            }

            throw new Exception("Invalid Line");
        }
    }
}