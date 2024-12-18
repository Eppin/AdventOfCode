namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day18 : Day
{
    public Day18() : base()
    {
    }

    [Answer("22", Example, Data = "5,4{nl}4,2{nl}4,5{nl}3,0{nl}2,1{nl}6,3{nl}2,4{nl}1,5{nl}0,6{nl}3,3{nl}2,6{nl}5,1{nl}1,2{nl}5,5{nl}2,5{nl}6,5{nl}1,4{nl}0,4{nl}6,4{nl}1,1{nl}6,1{nl}1,0{nl}0,5{nl}1,6{nl}2,0")]
    [Answer("296", Regular)]
    public override string SolveA()
    {
        var grid = GetGrid();

        var dijkstra = new Dijkstra<Coordinate>();

        var end = new Coordinate(70, 70); // Example uses 6,6

        dijkstra.GetNeighbours = reindeer => grid.Neighbours(reindeer.X, reindeer.Y).Where(n => grid[n.X, n.Y] is '.').Select(n => new Coordinate(n.X, n.Y));
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

        return dijkstra.ShortestPath(new Coordinate(0, 0)).ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private Grid<char> GetGrid()
    {
        const int length = 71; // Example uses 7

        var parsed = Parse().Take(1024); // Example uses 12

        var array = new char[length][];
        for (var y = 0; y < length; y++)
        {
            array[y] = new char[length];
            for (var x = 0; x < length; x++)
                array[y][x] = '.';
        }

        var grid = new Grid<char>(array);

        foreach (var coordinate in parsed)
            grid[coordinate] = '#';

        return grid;
    }

    private List<Coordinate> Parse()
    {
        return GetSplitInput()
            .Select(s =>
            {
                var split = s.Split(',');
                return new Coordinate(int.Parse(split[0]), int.Parse(split[1]));
            }).ToList();
    }
}
