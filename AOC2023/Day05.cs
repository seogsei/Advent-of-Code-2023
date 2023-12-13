namespace AOC2023
{
    internal static class Day05
    {
        public static uint Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day05.txt";

            using StreamReader dataStream = new(File.OpenRead(path));

            (var seedNumbers, var maps) = ReadData(dataStream);
            //Part 1
            /*
            IEnumerable<uint> enumerable = seedNumbers;

            foreach(var map in maps)
                enumerable = map.Translate(enumerable);

            return enumerable.Min();
            */
            //Part 2

            List<Range> seedRanges = [];
            for (int i = 0; i < seedNumbers.Count; i += 2)
                seedRanges.Add(new(seedNumbers[i], seedNumbers[i + 1]));

            IEnumerable<Range> enumerable = seedRanges;
            foreach (var map in maps)
                enumerable = map.Translate(enumerable);

            return enumerable.Min((x) => x.Start);
        }

        private static (List<uint> seedNumbers, List<Map> maps) ReadData(StreamReader dataStream)
        {
            string data = dataStream.ReadToEnd();
            string[] parts = data.Split("\r\n\r\n");

            var seedNumbers = ReadNumbers(parts[0].Split(':')[1]);

            var maps = ReadMaps(parts.Skip(1));

            return (seedNumbers, maps);
        }

        private static List<Map> ReadMaps(IEnumerable<string> inputs)
        {
            List<Map> result = [];

            foreach (string input in inputs)
            {
                result.Add(ReadMap(input));
            }

            return result;
        }

        private static Map ReadMap(string input)
        {
            var data = input.Split("\n").Skip(1);

            List<MapEntry> entries = [];
            foreach (var entry in data)
            {
                entries.Add(ReadEntry(entry));
            }

            return new(entries);
        }

        private static List<uint> ReadNumbers(string input)
        {
            List<uint> result = [];

            foreach (var item in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                result.Add(uint.Parse(item));

            return result;
        }

        private static MapEntry ReadEntry(string input)
        {
            var a = input.Split();

            return new(uint.Parse(a[0]), uint.Parse(a[1]), uint.Parse(a[2]));
        }

        private record class Map(List<MapEntry> Entries)
        {
            public uint Translate(uint value)
            {
                foreach (var entry in Entries)
                    if (entry.Contains(value))
                        return entry.Translate(value);

                return value;
            }

            public IEnumerable<uint> Translate(IEnumerable<uint> values)
            {
                foreach (var value in values)
                    yield return Translate(value);
            }

            public IEnumerable<Range> Translate(IEnumerable<Range> ranges)
            {
                foreach (var part in ranges)
                    foreach (var translation in Translate(part))
                        yield return translation;
            }

            public IEnumerable<Range> Translate(Range range)
            {
                foreach (var entry in Entries)
                {
                    if (Range.Intersect(range, entry.Source).IsEmpty()) continue;

                    yield return entry.Translate(range);

                    foreach (var remains in Translate(range - entry.Source))
                        yield return remains;

                    yield break;
                }

                yield return range;
            }
        }

        private class MapEntry(uint DestinationStart, uint SourceStart, uint Length)
        {
            public Range Source { get; } = new(SourceStart, Length);
            public Range Destination { get; } = new(DestinationStart, Length);

            public bool Contains(uint value)
            {
                return value >= SourceStart && value < SourceStart + Length;
            }

            public uint Translate(uint value)
            {
                return value - SourceStart + DestinationStart;
            }

            public Range Translate(Range range)
            {
                return Range.Intersect(range, Source) - Source.Start + Destination.Start;
            }
        }

        private readonly record struct Range(uint Start, uint Length)
        {
            public static readonly Range Empty = default;

            public readonly uint End => Start + Length;

            public bool IsEmpty()
            {
                return Length == 0;
            }

            public static Range Intersect(Range lhs, Range rhs)
            {
                uint start = Math.Max(lhs.Start, rhs.Start);
                uint end = Math.Min(lhs.End, rhs.End);

                if (end <= start)
                    return Empty;

                return Between(start, end);
            }

            public static Range Between(uint start, uint end) => new(start, end - start);

            public static Range operator +(Range range, uint shift) => new(range.Start + shift, range.Length);

            public static Range operator -(Range range, uint shift) => new(range.Start - shift, range.Length);

            public static IEnumerable<Range> operator -(Range lhs, Range rhs)
            {
                if (lhs.Start < rhs.Start)
                    yield return Between(lhs.Start, Math.Min(lhs.End, rhs.Start));

                if (rhs.End < lhs.End)
                    yield return Between(rhs.End, lhs.End);
            }
        }
    }
}
