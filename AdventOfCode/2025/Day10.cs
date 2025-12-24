namespace AdventOfCode._2025;

public partial class Day10 : Day
{
    public Day10() : base()
    {
    }

    [Answer("7", Example, Data = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}{nl}[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}{nl}[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}")]
    [Answer("459", Regular)]
    public override object SolveA()
    {
        return Parse().Sum(Solve);
    }

    [Answer("33", Example, Data = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}{nl}[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}{nl}[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}")]
    public override object SolveB()
    {
        throw new NotImplementedException();
    }

    private static long Solve(Line line)
    {
        var maxLength = long.MaxValue;

        var queue = new Queue<LightState>();

        var start = new char[line.Lights.Length];
        for (var i = 0; i < start.Length; i++)
            start[i] = '.';

        var seen = new HashSet<string>();

        foreach (var buttons in line.Buttons)
        {
            var initial = new LightState(buttons, (char[])start.Clone(), 1);
            queue.Enqueue(initial);
        }

        while (queue.TryDequeue(out var lightState))
        {
            if (lightState.Length >= maxLength)
                continue;

            foreach (var idx in lightState.Buttons)
            {
                lightState.Current[idx] = lightState.Current[idx] == '#' ? '.' : '#';
            }

            var stateKey = new string(lightState.Current);
            if (!seen.Add(stateKey))
            {
                continue;
            }

            if (lightState.Current.SequenceEqual(line.Lights))
            {
                maxLength = Math.Min(maxLength, lightState.Length);
                continue;
            }

            foreach (var buttons in line.Buttons)
            {
                var newX = new LightState(buttons, (char[])lightState.Current.Clone(), lightState.Length + 1);
                queue.Enqueue(newX);
            }
        }

        return maxLength;
    }

    private List<Line> Parse()
    {
        var lines = new List<Line>();

        foreach (var input in SplitInput)
        {
            var lights = LightRegex().Match(input).Value
                .Trim('[', ']')
                .ToCharArray();

            var buttons = ButtonsRegex()
                .Matches(input)
                .Select(m => m.Value
                    .Trim('(', ')')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();

            var joltages = JoltageRegex()
                .Match(input).Value
                .Trim('{', '}')
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            lines.Add(new(lights, buttons, joltages));
        }

        return lines;
    }

    private record Line(char[] Lights, int[][] Buttons, int[] Joltages);

    private class LightState(int[] buttons, char[] current, int length)
    {
        public int[] Buttons { get; } = buttons;
        public char[] Current { get; } = current;

        public int Length { get; } = length;
    }

    [GeneratedRegex(@"\[(.*)\]")]
    private static partial Regex LightRegex();

    [GeneratedRegex(@"\((.*?)\)")]
    private static partial Regex ButtonsRegex();

    [GeneratedRegex(@"\{(.*)\}")]
    private static partial Regex JoltageRegex();
}
