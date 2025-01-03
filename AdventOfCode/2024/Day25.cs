namespace AdventOfCode._2024;

public class Day25 : Day
{
    public Day25() : base()
    {
    }

    [Answer("3", Example, Data = "#####{nl}.####{nl}.####{nl}.####{nl}.#.#.{nl}.#...{nl}.....{nl}{nl}#####{nl}##.##{nl}.#.##{nl}...##{nl}...#.{nl}...#.{nl}.....{nl}{nl}.....{nl}#....{nl}#....{nl}#...#{nl}#.#.#{nl}#.###{nl}#####{nl}{nl}.....{nl}.....{nl}#.#..{nl}###..{nl}###.#{nl}###.#{nl}#####{nl}{nl}.....{nl}.....{nl}.....{nl}#....{nl}#.#..{nl}#.#.#{nl}#####")]
    [Answer("3116", Regular)]
    public override string SolveA()
    {
        var (locks, keys) = Parse();
        var count = 0;

        foreach (var @lock in locks)
        {
            foreach (var key in keys)
            {
                var fail = false;
                for (var i = 0; i < 5; i++)
                {
                    if (@lock[i] + key[i] > 5)
                    {
                        fail = true;
                        break;
                    }
                }

                if (!fail) count++;
            }
        }

        return count.ToString();
    }

    [Answer("christmas \ud83c\udf84", Regular)]
    public override string SolveB()
    {
        return "christmas \ud83c\udf84";
    }

    private (List<List<int>> Locks, List<List<int>> Keys) Parse()
    {
        var locks = new List<List<int>>();
        var keys = new List<List<int>>();
        var tmp = new List<string>();

        string? first;
        string? last;

        foreach (var line in GetSplitInput(false))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                first = tmp.First();
                last = tmp.Last();
                if (first.ToCharArray().All(a => a is '#') && last.ToCharArray().All(a => a is '.'))
                    locks.Add(AddToList(tmp));
                else
                    keys.Add(AddToList(tmp));

                tmp.Clear();
                continue;
            }

            tmp.Add(line);
        }

        // Process last one...
        first = tmp.First();
        last = tmp.Last();
        if (first.ToCharArray().All(a => a is '#') && last.ToCharArray().All(a => a is '.'))
            locks.Add(AddToList(tmp));
        else
            keys.Add(AddToList(tmp));

        tmp.Clear();

        return (locks, keys);
    }

    private static List<int> AddToList(List<string> tmp)
    {
        var array = tmp.Select(s => s.ToCharArray()).ToArray();
        var grid = new Grid<char>(array);

        var list = new List<int>();
        for (var x = 0; x < grid.MaxX; x++)
        {
            var count = -1;
            for (var y = 0; y < grid.MaxY; y++)
            {
                if (grid[x, y] is '#')
                    count++;
            }

            list.Add(count);
        }

        return list;
    }
}
