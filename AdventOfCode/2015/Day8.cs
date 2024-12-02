namespace AdventOfCode._2015;

public class Day8 : Day
{
    public Day8() : base()
    {
    }

    public override string SolveA()
    {
        int codeL = 0, charactersL = 0;
        foreach (var line in SplitInput)
        {
            var characters = Regex.Unescape(line);

            if (characters.StartsWith("\""))
                characters = characters[1..];

            if (characters.EndsWith("\""))
                characters = characters[..^1];

            var codeLength = line.Length;
            var charactersLength = characters.Length;

            codeL += codeLength;
            charactersL += charactersLength;
        }

        return $"{codeL - charactersL}";
    }

    public override string SolveB()
    {
        int codeL = 0, charactersL = 0;
        foreach (var line in SplitInput)
        {
            var characters = Regex.Escape(line);
            characters = $"\"{characters.Replace("\"", "\\\"")}\"";

            var codeLength = line.Length;
            var charactersLength = characters.Length;

            codeL += codeLength;
            charactersL += charactersLength;
        }

        return $"{charactersL - codeL}";
    }
}