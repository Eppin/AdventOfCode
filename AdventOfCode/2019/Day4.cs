namespace AdventOfCode._2019;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    [Answer("1", Example, Data = "111111-111111")]
    [Answer("0", Example, Data = "223450-223450")]
    [Answer("0", Example, Data = "123789-123789")]
    [Answer("1890", Regular, Data = "138241-674034")]
    public override object SolveA()
    {
        var (start, end) = Parse();

        var valid = 0;
        for (var i = start; i <= end; i++)
        {
            if(IsValid(i))
                valid++;
        }

        return valid;
    }

    public override object SolveB()
    {
        throw new NotImplementedException();
    }
    
    private static bool IsValid(int password)
    {
        var hasAdjacent = false;
        
        var start = password;
        var previous = 0;
        for (var i = 0; i < 6; i++)
        {
            var next = start / 10;
            var current = start % 10;

            if (i > 0)
            {
                if (current > previous)
                    return false;
                
                if(current == previous)
                    hasAdjacent = true;
            }

            previous = current;
            start = next;
        }

        return hasAdjacent;
    }

    private (int Start, int End) Parse()
    {
        var parts = Input.Split('-');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
