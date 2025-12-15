namespace AdventOfCode._2025;

public partial class Day10 : Day
{
    public Day10() : base()
    {
    }

    [Answer("7", Example, Data = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}{nl}[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}{nl}[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}")]
    [Answer("", Regular)]
    public override object SolveA()
    {
        var result = 0L;
        foreach (var line in Parse())
        {
            result += Solve(line);
            // break;
        }


        return result;
    }

    public override object SolveB()
    {
        throw new NotImplementedException();
    }

    private long Solve(Line line)
    {
        // Console.WriteLine($"Solve: {string.Join("", line.Lights)}");

        var maxLength = long.MaxValue;
        var queue = new Queue<X>();

        var start = new char[line.Lights.Length];
        for (var i = 0; i < start.Length; i++)
            start[i] = '.';

        foreach (var buttons in line.Buttons)
        {
            queue.Enqueue(new X(buttons, [], (char[])start.Clone(), [buttons]));
        }

        while (queue.TryDequeue(out var x))
        {
            // Console.WriteLine($"S: {string.Join(" - ", x.Path.Select(y => string.Join(',', y)))}");

            if (x.Path.Count >= maxLength)
                continue;

            foreach (var buttons in x.Buttons)
            {
                if (buttons >= x.Current.Length)
                    Console.WriteLine();

                // Console.WriteLine($"B: {string.Join("", x.Current)} => {string.Join(',', buttons)}");
                x.Current[buttons] = x.Current[buttons] == '#' ? '.' : '#';
                // Console.WriteLine($"A: {string.Join("", x.Current)}");
                // Console.WriteLine();
            }

            if (x.Current.SequenceEqual(line.Lights))
            {
                // Console.WriteLine("FOUND!! ->");

                if (maxLength > x.Path.Count)
                {
                    maxLength = x.Path.Count;
                    Console.WriteLine("(new) Max length: " + maxLength);
                }

                // foreach (var path in x.Path)
                // {
                //     Console.WriteLine($"\t{string.Join(',', path)}");
                // }

                continue;
            }

            foreach (var buttons in line.Buttons)
            {
                if (x.LastPress.Length == 1 && x.LastPress.SequenceEqual(buttons))
                    continue; // Skip pressing the same single button, to avoid looping

                // TODO Last press not needed? Just use a priority queue? New at the end

                var newX = new X(buttons, x.Buttons, (char[])x.Current.Clone(), [..x.Path, buttons]);
                // Console.WriteLine($"Next: {string.Join(',', newX.Buttons)}, {string.Join("", newX.Current)}, {string.Join(" - ", newX.Path.Select(y => string.Join(',', y)))}");

                queue.Enqueue(newX);
            }
        }

        return maxLength;
    }

    public class X(int[] buttons, int[] lastPress, char[] current, List<int[]> path)
    {
        public int[] Buttons { get; set; } = buttons;
        public int[] LastPress { get; set; } = lastPress;
        public char[] Current { get; set; } = current;

        public List<int[]> Path { get; set; } = path;
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

    [GeneratedRegex(@"\[(.*)\]")]
    private static partial Regex LightRegex();

    [GeneratedRegex(@"\((.*?)\)")]
    private static partial Regex ButtonsRegex();

    [GeneratedRegex(@"\{(.*)\}")]
    private static partial Regex JoltageRegex();
}
