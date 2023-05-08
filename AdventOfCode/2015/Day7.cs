namespace AdventOfCode._2015;

using System.Text.RegularExpressions;

public partial class Day7 : Day
{
    private static List<Parsed> _parsedLines = new();

    public Day7() : base()
    {
        _parsedLines = ParseInput().ToList();
    }

    public override string SolveA()
    {
        return Solve("a").ToString();
    }

    public override string SolveB()
    {
        // Find A
        var a = Solve("a");

        // Reset
        _parsedLines = ParseInput().ToList();
        
        // Find B and replace with value from A
        var find = _parsedLines.SingleOrDefault(p => p.Output.Equals("b"));
        if (find == null)
            throw new Exception("Can't be null!");

        find.InputA = a;

        return Solve("a").ToString();
    }

    private static int Solve(object output)
    {
        var find = _parsedLines.SingleOrDefault(p => p.Output.Equals(output));
        if (find == null)
            throw new Exception("Can't be null!");

        if (find.InputA is not int)
            find.InputA = Solve(find.InputA);

        if (find.InputB != null && find.InputB is not int)
            find.InputB = Solve(find.InputB);

        return find.Solve();
    }

    private IEnumerable<Parsed> ParseInput()
    {
        foreach (var input in SplitInput)
        {
            var operatorMatch = OperatorRegex().Match(input);
            if (operatorMatch.Success)
            {
                yield return new Parsed(operatorMatch.Groups[1].Value, operatorMatch.Groups[3].Value, operatorMatch.Groups[4].Value, operatorMatch.Groups[2].Value);
                continue;
            }

            var notMatch = NotRegex().Match(input);
            if (notMatch.Success)
            {
                yield return new Parsed(notMatch.Groups[1].Value, null, notMatch.Groups[2].Value, "NOT");
                continue;
            }

            var simpleMatch = SimpleRegex().Match(input);
            if (simpleMatch.Success)
            {
                yield return new Parsed(simpleMatch.Groups[1].Value, null, simpleMatch.Groups[2].Value, string.Empty);
                continue;
            }
        }
    }

    private class Parsed
    {
        public object InputA { get; set; }
        public object? InputB { get; set; }
        public object Output { get; set; }
        public Operator Operator { get; set; }

        public Parsed(object inputA, object? inputB, object output, string @operator)
        {
            InputA = int.TryParse(inputA.ToString(), out var intInputA) ? intInputA : inputA;
            InputB = int.TryParse(inputB?.ToString(), out var intInputB) ? intInputB : inputB;
            Output = output;
            Operator = @operator switch
            {
                "AND" => Operator.And,
                "OR" => Operator.Or,
                "NOT" => Operator.Not,
                "LSHIFT" => Operator.LeftShift,
                "RSHIFT" => Operator.RightShift,
                _ => Operator.Move
            };
        }

        public int Solve()
        {
            switch (Operator)
            {
                case Operator.And:
                    if (InputA is int aa && InputB is int ba)
                        return aa & ba;
                    break;

                case Operator.Or:
                    if (InputA is int ao && InputB is int bo)
                        return ao | bo;
                    break;

                case Operator.Not:
                    if (InputA is int an)
                        return (ushort)~an;
                    break;

                case Operator.LeftShift:
                    if (InputA is int al && InputB is int bl)
                        return al << bl;
                    break;

                case Operator.RightShift:
                    if (InputA is int ar && InputB is int br)
                        return ar >> br;
                    break;

                case Operator.Move:
                    if (InputA is int am)
                        return am;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new InvalidDataException($"Impossible to reach this point, data is [{InputA}] {Operator} [{InputB}] = [{Output}]");
        }
    }

    private enum Operator
    {
        And,
        Or,
        Not,
        LeftShift,
        RightShift,
        Move
    }

    [GeneratedRegex("^(.*) (AND|RSHIFT|OR|LSHIFT) (.*) -> (.*)$")]
    private static partial Regex OperatorRegex();

    [GeneratedRegex("^NOT (.*) -> (.*)$")]
    private static partial Regex NotRegex();

    [GeneratedRegex("^(\\S*) -> (\\S*)$")]
    private static partial Regex SimpleRegex();
}