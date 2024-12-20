using System.Collections.Concurrent;
using System.Xml.Linq;

namespace AdventOfCode._2024;

using System;
using System.Collections.Generic;
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

        // Null meting...

        var dijkstra = new Dijkstra<Coordinate>();

        dijkstra.GetNeighbours = reindeer => GetNeighbours(reindeer, grid);
        dijkstra.EndReached = current => current == end;
        dijkstra.Draw = list =>
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
        };

        var a = dijkstra.ShortestPath(start);

        //

        var possibleCheats = PossibleCheats(grid); // Draw grid with 'C' as cheat position!
        var speeds = new ConcurrentBag<int>();

        Parallel.ForEach(possibleCheats, cheat =>
        {
            var dijkstra = new Dijkstra<Coordinate>();

            dijkstra.GetNeighbours = reindeer => GetNeighbours(reindeer, grid, cheat);
            dijkstra.EndReached = current => current == end;
            dijkstra.Draw = list =>
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
            };

            var a = dijkstra.ShortestPath(start);
            speeds.Add(a);
        });

        //foreach (var cheat in possibleCheats)
        //{
        //    var dijkstra = new Dijkstra<Coordinate>();

        //    dijkstra.GetNeighbours = reindeer => GetNeighbours(reindeer, grid, cheat);
        //    dijkstra.EndReached = current => current == end;
        //    dijkstra.Draw = list =>
        //    {
        //        for (var y = 0; y < grid.MaxY; y++)
        //        {
        //            for (var x = 0; x < grid.MaxX; x++)
        //            {
        //                var c = list.Count(l => l == new Coordinate(x, y));
        //                if (c > 0)
        //                {
        //                    Console.Write(c);
        //                }
        //                else
        //                    Console.Write(grid[x, y]);
        //            }

        //            Console.WriteLine();
        //        }
        //    };

        //    var a = dijkstra.ShortestPath(start);
        //    speeds.Add(a);
        //    //Console.WriteLine(a);
        //}

        var total = 0;
        foreach (var group in speeds.GroupBy(s => s).OrderBy(g => g.Key))
        {
            var save = a - group.Key;
            if (save >= 100)
            {
                Console.WriteLine($"{group.Key}, {group.Count()} cheats save {a - group.Key} picostuff");// {string.Join(',', group)}");
                total += group.Count();
            }
        }

        //for (int i = 0; i < 10_000; i++)
        //{

        //}

        return total.ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
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
        if (cheat != null)
        {

            var k = grid
                .Neighbours(reindeer.X, reindeer.Y)
                .Where(n => grid[n.X, n.Y] is '.' || (n.X == cheat.Value.X && n.Y == cheat.Value.Y))
                .Select(n => new Coordinate(n.X, n.Y));

            return k;
        }
        else
        {
            var k2 = grid
                .Neighbours(reindeer.X, reindeer.Y)
                .Where(n => grid[n.X, n.Y] is '.')
                .Select(n => new Coordinate(n.X, n.Y));

            return k2;
        }

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
