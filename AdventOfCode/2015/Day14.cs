namespace AdventOfCode._2015;

using System.Data;
using System.Text.RegularExpressions;

public partial class Day14 : Day
{
    public Day14() : base()
    {
    }

    public override string SolveA()
    {
        return Solve().ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private int Solve()
    {
        var reindeers = Parse().ToList();

        // Total distance passed
        var distance = reindeers.ToDictionary(r => r.Deer, _ => 0);

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
                {
                    Console.WriteLine($"{reindeer.Deer} starting to rest");
                    status[reindeer.Deer] = (false, reindeer.Rest);
                }
                // Reset to running
                if (status[reindeer.Deer].Seconds == 0 && !isRunning)
                    status[reindeer.Deer] = (true, reindeer.Seconds);

                // if (seconds[reindeer.Deer] > 0)
                // {
                //     seconds[reindeer.Deer] -= 1;
                //     distance[reindeer.Deer] += reindeer.Distance;
                // }
                // else if (resting[reindeer.Deer] > 0)
                // {
                //     resting[reindeer.Deer] -= 1;
                //
                // }
                // else if (resting[reindeer.Deer] == 0)
                // {
                //     seconds[reindeer.Deer] = reindeer.Seconds;
                //     resting[reindeer.Deer] = reindeer.Rest;
                // }
            }

            Console.WriteLine(i + 1);
            foreach (var (key, value) in distance)
            {
                var state = status[key];
                Console.WriteLine($"{key}: {value} -> {state.Item1}, {state.Seconds}");
            }
        }

        return distance.Max(x => x.Value);
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
