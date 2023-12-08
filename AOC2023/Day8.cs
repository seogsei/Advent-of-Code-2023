namespace AOC2023
{
    internal static class Day8
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day8Input.txt";
            using StreamReader dataStream = new(File.OpenRead(path));

            var (instruction, map) = ReadData(dataStream);

            //Part 1
            //return Iterate(map["AAA"], (x) => x.Name == "ZZZ", instruction, map);

            //Part2
            var startNodes = map.Values.Where((x)=>x.Name.EndsWith('A'));
            var periods = startNodes.Select((x) => Iterate(x, (y) => y.Name.EndsWith('Z'), instruction, map));       
            
            return periods.Aggregate(LeastCommonMultiple);
        }

        private static (string instruction, Dictionary<string, Node> map) ReadData(StreamReader dataStream)
        {
            string data = dataStream.ReadToEnd();

            var parts = data.Split(Environment.NewLine + Environment.NewLine);


            Dictionary<string, Node> map = [];
            var nodes = parts[1].Split(Environment.NewLine);

            foreach (var node in nodes)
            {
                string name = node[..3];
                string left = node[7..10];
                string right = node[12..15];

                map.Add(name, new(name, right, left));
            }

            return (parts[0], map);
        }

        private static long Iterate(Node startingNode, Func<Node, bool> predicate, string instruction, Dictionary<string, Node> map)
        {
            Node currentNode = startingNode;
            long stepsTaken = 0;

            while (true)
                foreach (var dir in instruction)
                {
                    stepsTaken++;

                    currentNode = dir == 'L' ? map[currentNode.Left] : map[currentNode.Right];

                    if (predicate(currentNode))
                        return stepsTaken;
                }
        }

        private static long GreatestCommonDivisor(long a, long b) 
        {
            while(b != 0) 
            {
                long t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        private static long LeastCommonMultiple(long a, long b) 
        {
            return a * (b / GreatestCommonDivisor(a, b));
        }

        private record class Node(string Name, string Right, string Left);
    }
}
