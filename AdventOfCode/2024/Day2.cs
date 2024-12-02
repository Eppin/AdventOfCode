namespace AdventOfCode._2024;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("2", Example, Data = "7 6 4 2 1{nl}1 2 7 8 9{nl}9 7 6 2 1{nl}1 3 2 4 5{nl}8 6 4 4 1{nl}1 3 6 7 9{nl}")]
    public override string SolveA()
    {
        var reports = Parse();

        
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<IEnumerable<long>> Parse()
    {
        return SplitInput
            .Select(s => s
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse));
    }
}
