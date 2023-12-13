namespace AOC2023
{
    internal static class Day07
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Input\Day06.txt";

            using StreamReader dataStream = new(File.OpenRead(path));

            var data = ReadData(dataStream).ToList();
            //Part 1
            /*
            data.Sort((x, y) => 
            {
                const string pokerStrength = "23456789TJQKA";

                static PokerType GetType(string hand)
                {
                    PokerType type = PokerType.HighCard;

                    foreach (char strength in pokerStrength)
                    {
                        int count = 0;
                        foreach (var c in hand)
                            if (strength == c)
                                count++;

                        if (count < 2)
                            continue;

                        type += count switch
                        {
                            5 => (int)PokerType.FiveOfAKind,
                            4 => (int)PokerType.FourOfAKind,
                            3 => (int)PokerType.ThreeOfAKind,
                            2 => (int)PokerType.OnePair,
                            _ => throw new NotImplementedException(),
                        };
                    }

                    return type;
                }

                int typeComparison = GetType(x.Item1) - GetType(y.Item1);

                if (typeComparison != 0)
                    return typeComparison;

                for (int i = 0; i < 5; i++)
                {
                    int cardComparison = pokerStrength.IndexOf(x.Item1[i]) - pokerStrength.IndexOf(y.Item1[i]);
                    if (cardComparison != 0)
                        return cardComparison;
                }

                return 0;
            });
            */

            //Part2

            data.Sort((x, y) =>
            {
                const string pokerStrength = "J23456789TQKA";

                static PokerType GetType(string hand)
                {
                    int numberOfJokers = hand.Count((c) => c == 'J');

                    if (numberOfJokers > 3)
                        return PokerType.FiveOfAKind;

                    int numberOfGroups = 0;
                    int longestChain = 1;

                    foreach (char str in pokerStrength)
                    {
                        if (str == 'J')
                            continue;

                        int count = hand.Count((c) => c == str);

                        longestChain = Math.Max(longestChain, count);

                        if (count > 1)
                            numberOfGroups++;
                    }

                    longestChain += numberOfJokers;

                    return longestChain switch
                    {
                        1 => PokerType.HighCard,
                        2 => numberOfGroups < 2 ? PokerType.OnePair : PokerType.TwoPair,
                        3 => numberOfGroups < 2 ? PokerType.ThreeOfAKind : PokerType.FullHouse,
                        4 => PokerType.FourOfAKind,
                        5 => PokerType.FiveOfAKind,
                    };
                }

                int typeComparison = GetType(x.Item1) - GetType(y.Item1);

                if (typeComparison != 0)
                    return typeComparison;

                for (int i = 0; i < 5; i++)
                {
                    int cardComparison = pokerStrength.IndexOf(x.Item1[i]) - pokerStrength.IndexOf(y.Item1[i]);
                    if (cardComparison != 0)
                        return cardComparison;
                }

                return 0;
            });

            long result = 0;
            for (int i = 0; i < data.Count; i++)
                result += data[i].Item2 * (i + 1);

            return result;
        }

        private static IEnumerable<(string, int)> ReadData(StreamReader dataStream)
        {
            string? line;
            while ((line = dataStream.ReadLine()) is not null)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                yield return (parts[0], int.Parse(parts[1]));
            }
        }

        private enum PokerType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }
    }
}
