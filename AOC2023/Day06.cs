namespace AOC2023
{
    internal static class Day06
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day06.txt";

            using StreamReader dataStream = new(File.OpenRead(path));

            //Part 1
            /*
            IEnumerable<RaceData> races = ReadDataPart1(dataStream);

            long result = 1;

            foreach (RaceData raceData in races)
            {
                var poly = new SecondDegreePolynomial(-1, raceData.RecordTime, -raceData.Distance);
                var range = Range.Between((long)Math.Ceiling(poly.FirstRoot()), (long)Math.Ceiling(poly.SecondRoot()));

                result *= range.Length;
            }

            return result;
            */

            //Part 2

            RaceData race = ReadDataPart2(dataStream);

            var polynomial = new SecondDegreePolynomial(-1, race.RecordTime, -race.Distance);

            var range = Range.Between((long)Math.Ceiling(polynomial.FirstRoot()), (long)Math.Ceiling(polynomial.SecondRoot()));

            return range.Length;

        }

        private static IEnumerable<RaceData> ReadDataPart1(StreamReader dataStream)
        {
            string data = dataStream.ReadToEnd();
            var lines = data.Split("\n");
            var timeLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var distanceLine = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < timeLine.Length; i++)
            {
                yield return new(long.Parse(timeLine[i]), long.Parse(distanceLine[i]));
            }
        }
        private static RaceData ReadDataPart2(StreamReader dataStream)
        {
            string data = dataStream.ReadToEnd();
            var lines = data.Split("\n");
            var timeLine = string.Join("", lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));
            var distanceLine = string.Join("", lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));

            return new(long.Parse(timeLine), long.Parse(distanceLine));
        }

        public record struct RaceData(long RecordTime, long Distance);


        //A class used to solve second degree polynomials which this problem is all about
        private record class SecondDegreePolynomial(double A = 1f, double B = 1f, double C = 1f)
        {
            public double Discriminant()
            {
                return (B * B) - (4 * A * C);
            }

            public double FirstRoot()
            {
                var disc = Discriminant();

                if (disc < 0)
                    return float.NaN;

                return (-B + Math.Sqrt(disc)) * 0.5f / A;
            }

            public double SecondRoot()
            {
                var disc = Discriminant();

                if (disc < 0)
                    return float.NaN;

                return (-B - Math.Sqrt(disc)) * 0.5f / A;
            }
        }


        //Copied from day 5 as an long based class instead of uint
        private readonly record struct Range(long Start, long Length)
        {
            public static readonly Range Empty = default;

            public readonly long End => Start + Length;

            public bool IsEmpty()
            {
                return Length == 0;
            }

            public static Range Intersect(Range lhs, Range rhs)
            {
                var start = Math.Max(lhs.Start, rhs.Start);
                var end = Math.Min(lhs.End, rhs.End);

                if (end <= start)
                    return Empty;

                return Between(start, end);
            }

            public static Range Between(long start, long end) => new(start, end - start);

            public static Range operator +(Range range, long shift) => new(range.Start + shift, range.Length);

            public static Range operator -(Range range, long shift) => new(range.Start - shift, range.Length);

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
