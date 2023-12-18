namespace AOC2023
{
    internal static class Utilities
    {
        public static string ReadFile(string path)
        {
            using StreamReader dataStream = new(File.OpenRead(path));
            return dataStream.ReadToEnd();
        }
    }
}