namespace AdventOfCode._2015;

public partial class Day14 : Day
{
    public Day14() : base()
    {
    }

    [Answer("2696", Regular)]
    public override string SolveA()
    {
        return Solve().ToString();
    }

    [Answer("1084", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool partB = false)
    {
        var reindeers = Parse().ToList();

        var distance = reindeers.ToDictionary(r => r.Deer, _ => 0);
        var lead = reindeers.ToDictionary(r => r.Deer, _ => 0);

        // True -> running, false -> resting
        var status = reindeers.ToDictionary(r => r.Deer, r => (true, r.Seconds));

        for (var i = 0; i < 2503; i++)
        {
            foreach (var reindeer in reindeers)
            {
                var isRunning = status[reindeer.Deer].Item1;
                var secondsLeft = status[reindeer.Deer].Seconds;

                // Running
                if (isRunning && secondsLeft > 0)
                {
                    distance[reindeer.Deer] += reindeer.Distance;
                    status[reindeer.Deer] = (true, secondsLeft - 1);
                }
                // Resting
                else if (!isRunning && secondsLeft > 0)
                {
                    status[reindeer.Deer] = (false, secondsLeft - 1);
                }

                // Reset to resting
                if (status[reindeer.Deer].Seconds == 0 && isRunning)
                    status[reindeer.Deer] = (false, reindeer.Rest);

                // Reset to running
                if (status[reindeer.Deer].Seconds == 0 && !isRunning)
                    status[reindeer.Deer] = (true, reindeer.Seconds);
            }

            if (!partB)
                continue;

            var leads = distance
                .GroupBy(x => x.Value)
                .MaxBy(x => x.Key);

            if (leads == null)
                continue;

            foreach (var a in leads)
                lead[a.Key] += 1;
        }

        return !partB
            ? distance.Max(x => x.Value)
            : lead.Max(x => x.Value);
    }

    private IEnumerable<Reindeer> Parse()
    {
        foreach (var input in SplitInput)
        {
            var regex = ReindeerRegex().Match(input);
            if (!regex.Success)
                throw new DataException("Regex can't fail");

            var deer = regex.Groups[1].Value;
            var distance = int.Parse(regex.Groups[2].Value);
            var seconds = int.Parse(regex.Groups[3].Value);
            var rest = int.Parse(regex.Groups[4].Value);

            yield return new(deer, distance, seconds, rest);
        }
    }

    private record struct Reindeer(string Deer, int Distance, int Seconds, int Rest);

    [GeneratedRegex("(.*) can fly (\\d+) km\\/s for (\\d+) seconds, but then must rest for (\\d+) seconds")]
    private static partial Regex ReindeerRegex();
}
