using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day10
{
    internal static class Solution
    {
        public static long Compute()
        {
            const string path = @"C:\Users\rapha\source\repos\AOC2023\AOC2023\Day10\Input.txt";
            using StreamReader dataStream = new(File.OpenRead(path));

            var map = ReadData(dataStream);

            //Part 1
            //return RatPath(Find('S', map), map).Count() / 2;

            //Part 2
            //We create a empty map that we can manipulate the squares of
            var emptyMap = new char[map.Length][];

            for (int j = 0; j < emptyMap.Length; j++)
                Array.Fill(emptyMap[j] = new char[map[0].Length], ' ');

            //Find the 'S' that indicates the start
            Vector2Int startSquare = Find('S', map);

            //Only draw the pipes that the rat went through
            foreach (var position in RatPath(startSquare, map))
                emptyMap[position.Y][position.X] = map[position.Y][position.X];

            //Correct the starting square
            emptyMap[startSquare.Y][startSquare.X] = FindCorrectPipe(startSquare, map);

            return emptyMap.Sum((line) =>
            {
                bool inside = false;

                return line.Count((c) => 
                {
                    if (c == '|' || c == 'L' || c == 'J')
                        inside = !inside;

                    return c == ' ' && inside;
                });
            });
        }

        private static char FindCorrectPipe(Vector2Int startSquare, string[] map)
        {
            //Which directions this pipe can go towards
            bool up = CanEnterPipe(Vector2Int.Up, map[startSquare.Y + Vector2Int.Up.Y][startSquare.X + Vector2Int.Up.X]);
            bool right = CanEnterPipe(Vector2Int.Right, map[startSquare.Y + Vector2Int.Right.Y][startSquare.X + Vector2Int.Right.X]);
            bool down = CanEnterPipe(Vector2Int.Down, map[startSquare.Y + Vector2Int.Down.Y][startSquare.X + Vector2Int.Down.X]);

            if(up)
            {
                if (right)
                    return 'L';
                if (down)
                    return '|';
                return 'J'; //Left
            }
            if (right)
            {
                if (down)
                    return 'F';
                return '_'; //Left
            }
            return '7'; //Down, Left
        }

        private static IEnumerable<Vector2Int> RatPath(Vector2Int startPosition, string[] map)
        {
            Vector2Int position = startPosition;
            Vector2Int direction = Vector2Int.CardinalDirections().First((dir) => CanEnterPipe(dir, map[startPosition.Y + dir.Y][startPosition.X + dir.X]));
            do
            {
                position += direction;
                yield return position;

                if (position == startPosition)
                    break;

                direction = GetNextDirection(map[position.Y][position.X], direction);
            } while (true);
        }

        private static (int, int) Find(char c, string[] arr) 
        {
            for (int j = 0; j< arr.Length; j++)
            {
                int i = arr[j].IndexOf(c);
                if (i != -1)
                    return (i, j);
            }

            return default;
        }

        private static string[] ReadData(StreamReader stream)
        {
            return stream.ReadToEnd().Split("\r\n");
        }

        private static bool CanEnterPipe(Vector2Int direction, char pipe) => pipe switch
        {
            '|' => direction == Vector2Int.Up || direction == Vector2Int.Down,
            '-' => direction == Vector2Int.Left|| direction == Vector2Int.Right,
            'F' => direction == Vector2Int.Up || direction == Vector2Int.Left,
            'J' => direction == Vector2Int.Down|| direction == Vector2Int.Right,
            '7' => direction == Vector2Int.Up || direction == Vector2Int.Right,
            'L' => direction == Vector2Int.Down || direction == Vector2Int.Left,
        };

        private static Vector2Int GetNextDirection(char pipe, Vector2Int direction)
        {
            return (pipe) switch
            {
                '|' or '-' => direction,
                'F'  or 'J'=> new(-direction.Y, -direction.X),
                '7' or 'L' => new(direction.Y, direction.X)
            };
        }

        private record struct Vector2Int(int X, int Y) :
            IAdditionOperators<Vector2Int, Vector2Int, Vector2Int>
        {
            public static Vector2Int Right { get; } = (1, 0);
            public static Vector2Int Left { get; } = (-1, 0);
            public static Vector2Int Up { get; } = (0, -1);
            public static Vector2Int Down { get; } = (0, 1);

            public static IEnumerable<Vector2Int> CardinalDirections()
            {
                yield return Right;
                yield return Left;
                yield return Up;
                yield return Down;
            }

            public static Vector2Int operator +(Vector2Int left, Vector2Int right)
            {
                return new(left.X + right.X, left.Y + right.Y);
            }

            public static implicit operator (int, int)(Vector2Int value)
            {
                return (value.X, value.Y);
            }

            public static implicit operator Vector2Int((int, int) value)
            {
                return new Vector2Int(value.Item1, value.Item2);
            }
        }
    }
}
