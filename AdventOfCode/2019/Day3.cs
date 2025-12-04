namespace AdventOfCode._2019;

public class Day3 : Day
{
    public Day3() : base()
    {
    }

    [Answer("6", Example, Data = "R8,U5,L5,D3{nl}U7,R6,D4,L4")]
    [Answer("159", Example, Data = "R75,D30,R83,U83,L12,D49,R71,U7,L72{nl}U62,R66,U55,R34,D71,R55,D58,R83")]
    [Answer("135", Example, Data = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51{nl}U98,R91,D20,R16,D67,R40,U7,R15,U6,R7")]
    [Answer("260", Regular)]
    public override object SolveA()
    {
        var parse = Parse();

        var wire1 = parse[0];
        var wire2 = parse[1];

        var (path1, _) = Loop(wire1);
        var (path2, _) = Loop(wire2);

        return path1
            .Intersect(path2)
            .Select(intersection => Math.Abs(intersection.X) + Math.Abs(intersection.Y))
            .Prepend(int.MaxValue)
            .Min();
    }

    [Answer("30", Example, Data = "R8,U5,L5,D3{nl}U7,R6,D4,L4")]
    [Answer("610", Example, Data = "R75,D30,R83,U83,L12,D49,R71,U7,L72{nl}U62,R66,U55,R34,D71,R55,D58,R83")]
    [Answer("410", Example, Data = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51{nl}U98,R91,D20,R16,D67,R40,U7,R15,U6,R7")]
    [Answer("15612", Regular)]
    public override object SolveB()
    {
        var parse = Parse();

        var wire1 = parse[0];
        var wire2 = parse[1];

        var (path1, steps1) = Loop(wire1);
        var (path2, steps2) = Loop(wire2);

        return path1
            .Intersect(path2)
            .Select(y => steps1[y] + steps2[y])
            .Prepend(int.MaxValue)
            .Min();
    }

    private static (HashSet<Coordinate> Path, Dictionary<Coordinate, int> Steps) Loop((char, int)[] input)
    {
        var path = new HashSet<Coordinate>();
        var pathSteps = new Dictionary<Coordinate, int>();

        var current = new Coordinate(0, 0);
        var steps = 0;

        foreach (var (direction, distance) in input)
        {
            for (var i = 0; i < distance; i++)
            {
                current = direction switch
                {
                    'R' => current.Right,
                    'L' => current.Left,
                    'U' => current.Up,
                    'D' => current.Down,
                    _ => throw new InvalidDataException($"Unknown direction {direction}")
                };
                path.Add(current);
                pathSteps.TryAdd(current, ++steps);
            }
        }

        return (path, pathSteps);
    }

    private (char, int)[][] Parse()
    {
        return SplitInput
            .Select(l => l
                .Split(',')
                .Select(s => (s[0], int.Parse(s[1..])))
                .ToArray())
            .ToArray();
    }
}
