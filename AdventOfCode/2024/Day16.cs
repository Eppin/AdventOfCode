namespace AdventOfCode._2024;

using System.Linq;
using Coordinate = Coordinate<int>;

public class Day16 : Day
{
    public Day16() : base()
    {
    }

    //[Answer("7036", Example, Data = "###############{nl}#.......#....E#{nl}#.#.###.#.###.#{nl}#.....#.#...#.#{nl}#.###.#####.#.#{nl}#.#.#.......#.#{nl}#.#.#####.###.#{nl}#...........#.#{nl}###.#.#####.#.#{nl}#...#.....#.#.#{nl}#.#.#.###.#.#.#{nl}#.....#...#.#.#{nl}#.###.#.#.#.#.#{nl}#S..#.....#...#{nl}###############")]
    [Answer("11048", Example, Data = "#################{nl}#...#...#...#..E#{nl}#.#.#.#.#.#.#.#.#{nl}#.#.#.#...#...#.#{nl}#.#.#.#.###.#.#.#{nl}#...#.#.#.....#.#{nl}#.#.#.#.#.#####.#{nl}#.#...#.#.#.....#{nl}#.#.#####.#.###.#{nl}#.#.#.......#...#{nl}#.#.###.#####.###{nl}#.#.#...#.....#.#{nl}#.#.#.#####.###.#{nl}#.#.#.........#.#{nl}#.#.#.#########.#{nl}#S#.............#{nl}#################")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        var parsed = Parse();

        var grid = new Grid<char>(parsed);

        var start = new Coordinate();
        var end = new Coordinate();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is 'S')
                {
                    start = new Coordinate(x, y);
                    grid[x, y] = '.';
                }

                if (grid[x, y] is 'E')
                {
                    end = new Coordinate(x, y);
                    grid[x, y] = '.';
                }
            }
        }

        throw new NotImplementedException();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static Direction[] Rotate(Direction direction)
    {
        // Only allow current direction and (counter)clockwise
        return direction switch
        {
            Direction.North => [Direction.North, Direction.East, Direction.West],
            Direction.East => [Direction.East, Direction.North, Direction.South],
            Direction.South => [Direction.South, Direction.West, Direction.East],
            Direction.West => [Direction.West, Direction.North, Direction.South],
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s.ToCharArray())
            .ToArray();
    }
}
