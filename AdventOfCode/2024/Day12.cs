﻿namespace AdventOfCode._2024;

public class Day12 : Day
{
    public Day12() : base()
    {
    }

    [Answer("1930", Example, Data = "RRRRIICCFF{nl}RRRRIICCCF{nl}VVRRRCCFFF{nl}VVRCCCJFFF{nl}VVVVCJJCFE{nl}VVIVCCJJEE{nl}VVIIICJJEE{nl}MIIIIIJJEE{nl}MIIISIJEEE{nl}MMMISSJEEE")]
    [Answer("1461806", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("1206", Example, Data = "RRRRIICCFF{nl}RRRRIICCCF{nl}VVRRRCCFFF{nl}VVRCCCJFFF{nl}VVVVCJJCFE{nl}VVIVCCJJEE{nl}VVIIICJJEE{nl}MIIIIIJJEE{nl}MIIISIJEEE{nl}MMMISSJEEE")]
    [Answer("887932", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private int Solve(bool isPartB)
    {
        var visited = new List<Coordinate>();
        var grid = Parse();

        var total = 0;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var coordinate = new Coordinate(x, y);
                var key = grid[x, y];

                if (visited.Contains(coordinate)) continue;

                var walked = Walk(grid, coordinate, key);
                visited.AddRange(walked);

                var fences = isPartB
                    ? FencesB(grid, walked)
                    : FencesA(grid, walked);

                total += fences * walked.Count;
            }
        }

        return total;
    }

    private static HashSet<Coordinate> Walk(Grid<string> grid, Coordinate coordinate, string key)
    {
        var coordinates = new HashSet<Coordinate>();
        var queue = new Queue<Coordinate>();

        queue.Enqueue(coordinate);
        coordinates.Add(coordinate);

        while (queue.Count > 0)
        {
            coordinate = queue.Dequeue();

            var neighbours = grid.Neighbours(coordinate).Where(p => grid[p] == key);
            foreach (var neighbour in neighbours)
            {
                if (!coordinates.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    coordinates.Add(neighbour);
                }
            }
        }

        return coordinates;
    }

    private static int FencesA(Grid<string> grid, HashSet<Coordinate> coordinates)
    {
        var total = 0;

        foreach (var coordinate in coordinates)
        {
            var neighbours = grid.Neighbours(coordinate).Count(n => coordinates.Any(r => r == n));
            var edges = 4 - neighbours;
            total += edges;
        }

        return total;
    }

    private static int FencesB(Grid<string> grid, HashSet<Coordinate> coordinates)
    {
        var total = 0;

        var fences = coordinates
            .OrderBy(p => p.Y)
            .ThenBy(p => p.X)
            .Select(p => new Fence(p, 0))
            .ToList();

        foreach (var fence in fences)
        {
            var neighbours = grid.Directions(fence.Coordinate, true)
                .Where(n => coordinates.Any(r => r == n.Value))
                .ToDictionary(d => d.Key, d => d.Value);

            if (!neighbours.ContainsKey(Direction.North))
            {
                if (neighbours.TryGetValue(Direction.East, out var east) && !neighbours.ContainsKey(Direction.NorthEast))
                {
                    var eastFence = fences.First(p => p.Coordinate == east);
                    eastFence.Mark += 1;
                }

                if (neighbours.TryGetValue(Direction.West, out var west) && !neighbours.ContainsKey(Direction.NorthWest))
                {
                    var westFence = fences.First(p => p.Coordinate == west);
                    westFence.Mark += 1;
                }
            }

            if (!neighbours.ContainsKey(Direction.East))
            {
                if (neighbours.TryGetValue(Direction.North, out var north) && !neighbours.ContainsKey(Direction.NorthEast))
                {
                    var northFence = fences.First(p => p.Coordinate == north);
                    northFence.Mark += 1;
                }

                if (neighbours.TryGetValue(Direction.South, out var south) && !neighbours.ContainsKey(Direction.SouthEast))
                {
                    var southFence = fences.First(p => p.Coordinate == south);
                    southFence.Mark += 1;
                }
            }

            if (!neighbours.ContainsKey(Direction.South))
            {
                if (neighbours.TryGetValue(Direction.East, out var south) && !neighbours.ContainsKey(Direction.SouthEast))
                {
                    var southFence = fences.First(p => p.Coordinate == south);
                    southFence.Mark += 1;
                }

                if (neighbours.TryGetValue(Direction.West, out var west) && !neighbours.ContainsKey(Direction.SouthWest))
                {
                    var westFence = fences.First(p => p.Coordinate == west);
                    westFence.Mark += 1;
                }
            }

            if (!neighbours.ContainsKey(Direction.West))
            {
                if (neighbours.TryGetValue(Direction.North, out var north) && !neighbours.ContainsKey(Direction.NorthWest))
                {
                    var northFence = fences.First(p => p.Coordinate == north);
                    northFence.Mark += 1;
                }

                if (neighbours.TryGetValue(Direction.South, out var south) && !neighbours.ContainsKey(Direction.SouthWest))
                {
                    var southFence = fences.First(p => p.Coordinate == south);
                    southFence.Mark += 1;
                }
            }

            var count = neighbours.Count(n => n.Key is Direction.North or Direction.East or Direction.South or Direction.West);
            var sum = 4 - fence.Mark - count;

            total += sum;
        }

        return total;
    }

    private Grid<string> Parse()
    {
        var split = GetSplitInput()
            .Select(s => s
                .ToCharArray()
                .Select(c => c.ToString())
                .ToArray())
            .ToArray();

        return new Grid<string>(split);
    }

    private class Fence(Coordinate coordinate, int mark)
    {
        public Coordinate Coordinate { get; set; } = coordinate;
        public int Mark { get; set; } = mark;
    }
}
