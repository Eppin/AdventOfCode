namespace AdventOfCode._2025;

public class Day5 : Day
{
    public Day5() : base()
    {
    }

    [Answer("3", Example, Data = "3-5{nl}10-14{nl}16-20{nl}12-18{nl}{nl}1{nl}5{nl}8{nl}11{nl}17{nl}32")]
    [Answer("664", Regular)]
    public override object SolveA()
    {
        var (ranges, ids) = Parse();
        return ids.LongCount(id => ranges.Any(r => r.Start <= id && id <= r.End));
    }

    [Answer("14", Example, Data = "3-5{nl}10-14{nl}16-20{nl}12-18{nl}{nl}1{nl}5{nl}8{nl}11{nl}17{nl}32")]
    [Answer("350780324308385", Regular)]
    public override object SolveB()
    {
        var (ranges, _) = Parse();
        var unique = new List<(long Start, long End)>();

        // Ordering is really important, I think
        foreach (var (start, end) in ranges.OrderBy(r => r.Start))
        {
            var count = 0;
            for (var i = 0; i < unique.Count; i++)
            {
                var r = unique[i];

                if (r.Start > end || start > r.End) continue;
                count++;

                if (r.Start > start)
                {
                    // Adjust start
                    unique[i] = (start, r.End);
                }

                if (r.End < end)
                {
                    // Adjust end
                    unique[i] = (r.Start, end);
                }
            }

            // Only add when we don't have an overlap
            if (count == 0) 
                unique.Add((start, end));
        }

        return unique.Sum(r => r.End - r.Start + 1);
    }

    private (List<(long Start, long End)> Ranges, List<long> IDs) Parse()
    {
        var ranges = new List<(long Start, long End)>();
        var ids = new List<long>();

        var emptyLine = false;
        foreach (var line in GetSplitInput(false))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                emptyLine = true;
                continue;
            }

            if (!emptyLine)
            {
                var split = line.Split('-');
                ranges.Add((long.Parse(split[0]), long.Parse(split[1])));
            }
            else
            {
                ids.Add(long.Parse(line));
            }
        }

        return (ranges, ids);
    }
}
