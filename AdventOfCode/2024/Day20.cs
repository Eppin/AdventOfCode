namespace AdventOfCode._2024;

using System;
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
        return Solve(false).ToString();
    }

    [Answer("", Example, Data = "###############{nl}#...#...#.....#{nl}#.#.#.#.#.###.#{nl}#S#...#.#.#...#{nl}#######.#.#.###{nl}#######.#.#...#{nl}#######.#.###.#{nl}###..E#...#...#{nl}###.#######.###{nl}#...###...#...#{nl}#.#####.#.###.#{nl}#.#...#.#.#...#{nl}#.#.#.#.#.#.###{nl}#...#...#...###{nl}###############")]
    [Answer("1033746", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var cheatLimit = isPartB ? 20 : 2;

        var (grid, start, end) = GetGrid();

        var racetrack = new List<Racetrack>();
        var visited = new HashSet<Coordinate>();
        var distance = 0;

        var current = start;
        while (true)
        {
            racetrack.Add(new Racetrack(current, distance));

            if (current == end)
                break;

            var neighbour = current.Neighbours.Single(n => grid[n] is '.' && !visited.Contains(n));

            visited.Add(current);
            current = neighbour;
            distance++;
        }

        var total = 0;
        foreach (var rt1 in racetrack)
        {
            foreach (var rt2 in racetrack)
            {
                if (rt1 == rt2) continue;

                distance = rt2.Distance - rt1.Distance;
                var manhattan = Math.Abs(rt1.Position.X - rt2.Position.X) + Math.Abs(rt1.Position.Y - rt2.Position.Y); // |X1-X2| + |Y1-Y2|
                if (manhattan > cheatLimit) continue;

                var saved = distance - manhattan;
                if (saved < 100) continue;

                total++;
            }
        }

        return total;
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

    private record struct Racetrack(Coordinate Position, int Distance);
}
