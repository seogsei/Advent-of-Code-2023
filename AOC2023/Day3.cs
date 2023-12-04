namespace AOC2023
{
    internal static class Day3
    {
        public static int Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day2Input.txt";

            int sum = 0;

            using StreamReader dataStream = new(File.OpenRead(path));

            string? previousLine = null;
            string? curLine = null;
            string? nextLine = null;
            

            return sum;
        }

        private static bool IsAOCSymboll(this char c)
        {
            return !char.IsDigit(c) && c != '.';
        }
    }
}
