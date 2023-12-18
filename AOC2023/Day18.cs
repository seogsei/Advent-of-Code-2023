namespace AOC2023
{
    internal static class Day18
    {
        public const string INPUT_PATH = $@"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day18.txt";

        public static long Compute()
        {
            IEnumerable<(string direction, int distance, string color)> data =
                Utilities.ReadFile(INPUT_PATH).Split(Environment.NewLine).Select((x) => { var split = x.Split(' '); return (split[0], int.Parse(split[1]), split[2].Trim('(', ')')); });

            //Part 1
            //return CalculateArea(data.Select((x) => (x.direction, x.distance)));

            //Part 2
            var a = data.Select((x) => ReadHexCode(x.color));
            return CalculateArea(a);
        }

        private static long CalculateArea(IEnumerable<(string direction, int distance)> digPlan) 
        {
            long area = 0;
            long sidewaysDistance = 0;
            long perimeter = 0;
            foreach (var (direction, distance) in digPlan)
            {
                perimeter += distance;
                switch (direction)
                {
                    case "R":
                        sidewaysDistance += distance;
                        break;
                    case "L":
                        sidewaysDistance -= distance;
                        break;
                    case "D":
                        area += sidewaysDistance * distance;
                        break;
                    case "U":
                        area -= sidewaysDistance * distance;
                        break;
                    default: throw new Exception();
                }
            }

            return area + (perimeter / 2) + 1;
        }

        private static (string direction, int distance) ReadHexCode(string hex) 
        {
            string direction = hex[6] switch
            {
                '0' => "R",
                '1' => "D",
                '2' => "L",
                '3' => "U",
                _ => throw new Exception()
            };

            int distance = int.Parse(hex.Substring(1, 5), System.Globalization.NumberStyles.HexNumber);

            return (direction, distance);
        }
    }
}
