namespace AdventOfCode._2015;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    public override string SolveA()
    {
        var result = 0;
        foreach (var character in Input)
        {
            if ('('.Equals(character))
                result++;
            else if (')'.Equals(character))
                result--;
        }

        return $"{result}";
    }

    public override string SolveB()
    {
        var result = 0;
        var count = 0;
        foreach (var character in Input)
        {
            if ('('.Equals(character))
                result++;
            else if (')'.Equals(character))
                result--;

            count++;

            if (result == -1)
                return $"{count}";
        }

        return "-1";
    }
}