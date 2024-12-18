﻿namespace AdventOfCode._2024;

using System.Linq;
using Coordinate = Coordinate<int>;

public class Day16 : Day
{
    public Day16() : base()
    {
    }

    //[Answer("7036", Example, Data = "###############{nl}#.......#....E#{nl}#.#.###.#.###.#{nl}#.....#.#...#.#{nl}#.###.#####.#.#{nl}#.#.#.......#.#{nl}#.#.#####.###.#{nl}#...........#.#{nl}###.#.#####.#.#{nl}#...#.....#.#.#{nl}#.#.#.###.#.#.#{nl}#.....#...#.#.#{nl}#.###.#.#.#.#.#{nl}#S..#.....#...#{nl}###############")]
    [Answer("11048", Example, Data = "#################{nl}#...#...#...#..E#{nl}#.#.#.#.#.#.#.#.#{nl}#.#.#.#...#...#.#{nl}#.#.#.#.###.#.#.#{nl}#...#.#.#.....#.#{nl}#.#.#.#.#.#####.#{nl}#.#...#.#.#.....#{nl}#.#.#####.#.###.#{nl}#.#.#.......#...#{nl}#.#.###.#####.###{nl}#.#.#...#.....#.#{nl}#.#.#.#####.###.#{nl}#.#.#.........#.#{nl}#.#.#.#########.#{nl}#S#.............#{nl}#################")]
    [Answer("143564", Regular)]
    public override string SolveA()
    {
        var parsed = Parse();

        var grid = new Grid<char>(parsed);

        var start = new Reindeer();
        var end = new Coordinate();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is 'S')
                {
                    start = new Reindeer(new Coordinate(x, y), Direction.East);
                    grid[x, y] = '.';
                }

                if (grid[x, y] is 'E')
                {
                    end = new Coordinate(x, y);
                    grid[x, y] = '.';
                }
            }
        }

        var dijkstra = new Dijkstra<Reindeer>();

        dijkstra.GetDistance = (current, neighbour) => current.Direction == neighbour.Direction ? 1 : 1000;
        dijkstra.GetNeighbours = reindeer => GetNeighbours(reindeer, grid);
        dijkstra.EndReached = current => current.Position == end;
        dijkstra.Draw = list =>
        {
            for (var y = 0; y < grid.MaxY; y++)
            {
                for (var x = 0; x < grid.MaxX; x++)
                {
                    var c = list.Count(l => l.Position == new Coordinate(x, y));
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

        return dijkstra.ShortestPath(start).ToString();
    }

    private static List<Reindeer> GetNeighbours(Reindeer reindeer, Grid<char> grid)
    {
        var directions = new List<Reindeer>();

        var keepDirection = reindeer.Position.Directions[reindeer.Direction];
        if (grid[keepDirection] is '.')
            directions.Add(reindeer with { Position = keepDirection }); // Add reindeer, keep current direction, with a new position

        var rotate = reindeer.Position.Directions
            .Where(d => grid[d.Value] is '.' && Rotate(reindeer.Direction, d.Key))
            .Select(d => reindeer with { Direction = d.Key });

        directions.AddRange(rotate);

        return directions.Distinct().ToList();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static bool Rotate(Direction current, Direction next)
    {
        // Only allow (counter)clockwise
        switch (current)
        {
            case Direction.North when next is Direction.East or Direction.West:
            case Direction.East when next is Direction.North or Direction.South:
            case Direction.South when next is Direction.West or Direction.East:
            case Direction.West when next is Direction.North or Direction.South:
                return true;

            default:
                return false;
        }
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s.ToCharArray())
            .ToArray();
    }

    private record struct Reindeer(Coordinate Position, Direction Direction);
}
