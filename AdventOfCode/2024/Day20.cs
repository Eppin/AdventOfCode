namespace AdventOfCode._2024;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Utils;
using Coordinate = Coordinate<int>;

public class Day20 : Day
{
    public Day20() : base()
    {
    }

    [Answer("", Example, Data = "###############{nl}#...#...#.....#{nl}#.#.#.#.#.###.#{nl}#S#...#.#.#...#{nl}#######.#.#.###{nl}#######.#.#...#{nl}#######.#.###.#{nl}###..E#...#...#{nl}###.#######.###{nl}#...###...#...#{nl}#.#####.#.###.#{nl}#.#...#.#.#...#{nl}#.#.#.#.#.#.###{nl}#...#...#...###{nl}###############")]
    [Answer("1524", Regular)]
    public override string SolveA()
    {
        var (grid, start, end) = GetGrid();

        var initialShortestPath = GetShortestPath(grid, start, end, null);

        var possibleCheats = PossibleCheats(grid);
        var speeds = new ConcurrentBag<int>();

        Parallel.ForEach(possibleCheats, cheat =>
        {
            speeds.Add(GetShortestPath(grid, start, end, cheat));
        });

        return (from @group in speeds.GroupBy(s => s)
                let save = initialShortestPath - @group.Key
                where save >= 100
                select @group.Count()
                )
            .Sum()
            .ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static int GetShortestPath(Grid<char> grid, Coordinate start, Coordinate end, Coordinate? cheat)
    {
        var dijkstra = new Dijkstra<Coordinate>
        {
            GetNeighbours = reindeer => GetNeighbours(reindeer, grid, cheat),
            EndReached = current => current == end,
            Draw = list =>
            {
                for (var y = 0; y < grid.MaxY; y++)
                {
                    for (var x = 0; x < grid.MaxX; x++)
                    {
                        var c = list.Count(l => l == new Coordinate(x, y));
                        if (c > 0)
                        {
                            Console.Write(c);
                        }
                        else
                            Console.Write(grid[x, y]);
                    }
                    Console.WriteLine();
                }
            }
        };

        return dijkstra.ShortestPath(start);
    }

    private static HashSet<Coordinate> PossibleCheats(Grid<char> grid)
    {
        var cheats = new HashSet<Coordinate>();

        // Skip border/walls around the grid
        for (var y = 1; y < grid.MaxY - 1; y++)
        {
            for (var x = 1; x < grid.MaxX - 1; x++)
            {
                if (grid[x, y] is not '.')
                    continue; // Can only 'walk' on a '.'

                // Get directions of current coordinate
                // Also check if next direction contains a free space, in the same direction
                var directions = grid.Directions(x, y)
                    .Where(d => grid[d.Value] is '#'
                                && grid.Directions(d.Value).TryGetValue(d.Key, out var next) && grid[next] is '.'
                                && !cheats.Any(c => c.X == d.Value.X && c.Y == d.Value.Y)).ToList();

                foreach (var direction in directions)
                    cheats.Add(new Coordinate(direction.Value.X, direction.Value.Y));
            }
        }

        return cheats;
    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate reindeer, Grid<char> grid, Coordinate? cheat = null)
    {
        return grid
            .Neighbours(reindeer.X, reindeer.Y)
            .Where(n => grid[n.X, n.Y] is '.' || (cheat != null && (n.X == cheat.Value.X && n.Y == cheat.Value.Y)))
            .Select(n => new Coordinate(n.X, n.Y));
    }

    private (Grid<char> Grid, Coordinate Start, Coordinate End) GetGrid()
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

        return (grid, start, end);
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s.ToCharArray())
            .ToArray();
    }
}
