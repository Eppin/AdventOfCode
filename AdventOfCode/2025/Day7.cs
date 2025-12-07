namespace AdventOfCode._2025;

public class Day7 : Day
{
    public Day7() : base()
    {
    }

    [Answer("21", Example, Data = ".......S.......{nl}...............{nl}.......^.......{nl}...............{nl}......^.^......{nl}...............{nl}.....^.^.^.....{nl}...............{nl}....^.^...^....{nl}...............{nl}...^.^...^.^...{nl}...............{nl}..^...^.....^..{nl}...............{nl}.^.^.^.^.^...^.{nl}...............")]
    [Answer("1600", Regular)]
    public override object SolveA()
    {
        var grid = Parse();
        Coordinate? start = null;
        var split = 0;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var cell = grid[x, y];

                if (start == null)
                {
                    if (cell != 'S') continue;

                    start = new Coordinate(x, y);
                    var south = grid.South(start.Value);
                    if (south != null) grid[south.Value] = '|';
                }
                else
                {
                    if (cell != '^') continue;

                    var coordinate = new Coordinate(x, y);

                    var north = grid.North(coordinate);
                    if (north != null && grid[north.Value] == '|')
                    {
                        split++;

                        var east = grid.East(coordinate);
                        var west = grid.West(coordinate);

                        if (east != null) grid[east.Value] = '|';
                        if (west != null) grid[west.Value] = '|';

                        bool stop;
                        do
                        {
                            var southEast = grid.South(east!.Value);
                            var southEastMoved = southEast != null && grid[southEast.Value] == '.';
                            if (southEastMoved)
                            {
                                grid[southEast!.Value] = '|';
                                east = southEast;
                            }

                            var southWest = grid.South(west!.Value);
                            var southWestMoved = southWest != null && grid[southWest.Value] == '.';
                            if (southWestMoved)
                            {
                                grid[southWest!.Value] = '|';
                                west = southWest;
                            }

                            stop = !southEastMoved && !southWestMoved;
                        } while (!stop);
                    }
                }
            }
        }

        return split;
    }

    [Answer("40", Example, Data = ".......S.......{nl}...............{nl}.......^.......{nl}...............{nl}......^.^......{nl}...............{nl}.....^.^.^.....{nl}...............{nl}....^.^...^....{nl}...............{nl}...^.^...^.^...{nl}...............{nl}..^...^.....^..{nl}...............{nl}.^.^.^.^.^...^.{nl}...............")]
    [Answer("8632253783011", Regular)]
    public override object SolveB()
    {
        var (_, b) = Solve();
        return b;
    }

    private (long PartA, long PartB) Solve()
    {
        var grid = Parse();
        Coordinate? start = null;

        var a = 0L;
        var b = new long[grid.MaxX];

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var cell = grid[x, y];
                var coordinate = new Coordinate(x, y);

                if (start == null)
                {
                    if (cell != 'S') continue;
                    start = coordinate;
                    b[x] = 1;
                }
                else
                {
                    if (cell != '^') continue;

                    // Count splits, avoid duplicates
                    a += b[x] > 0 ? 1 : 0;
                    
                    // Add flow to the left and right
                    // Increase value, to count for all possible combinations
                    b[x - 1] += b[x];
                    b[x + 1] += b[x];
                    
                    // Reset column, since the beam won't flow any further
                    b[x] = 0;
                }
            }
        }

        return (a, b.Sum());
    }

    private Grid<char> Parse()
    {
        var grid = SplitInput
            .Select(s => s.ToCharArray())
            .ToArray();

        return new Grid<char>(grid);
    }
}
