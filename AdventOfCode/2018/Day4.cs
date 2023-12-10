namespace AdventOfCode._2018;

using System.Globalization;
using System.Text.RegularExpressions;

public class Day4 : Day
{
    public Day4() : base()
    {
    }
    
    public override string SolveA()
    {
        var guards = GenerateLogbook();

        var guardAsleep = guards.Aggregate((l, r) => l.Value.Values.Sum() > r.Value.Values.Sum() ? l : r);
        var minuteAsleep = guardAsleep.Value.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

        return $"{guardAsleep.Key * minuteAsleep}";
    }

    public override string SolveB()
    {
            var guards = GenerateLogbook();

            var guardAsleep = guards.Where(x => x.Value.Values.Count > 0).Aggregate((l, r) => l.Value.Values.Max() > r.Value.Values.Max() ? l : r);
            var minuteAsleep = guardAsleep.Value.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return $"{guardAsleep.Key * minuteAsleep}";
        }

        private Dictionary<int, Dictionary<double, int>> GenerateLogbook()
        {
            var parsedGuards = SplitInput
                .Select(g => new ParsedGuard(g))
                .OrderBy(g => g.Date)
                .ToList();

            for (var i = 1; i < parsedGuards.Count; i++)
            {
                var prevGuard = parsedGuards[i - 1];
                var currGuard = parsedGuards[i];

                if (currGuard.Type != "shiftStart")
                {
                    currGuard.Id = prevGuard.Id;
                }
            }

            var guards = new Dictionary<int, Dictionary<double, int>>();
            var asSleep = DateTime.Now; // Need to initialize..
            foreach (var parsedGuard in parsedGuards)
            {
                if (parsedGuard.Type == "shiftStart")
                {
                    var guardId = parsedGuard.Id;

                    if (!guards.ContainsKey(guardId))
                    {
                        guards.Add(parsedGuard.Id, new Dictionary<double, int>());
                    }
                }
                else if (parsedGuard.Type == "asSleep")
                {
                    asSleep = parsedGuard.Date;
                }
                else if (parsedGuard.Type == "wakeUp")
                {
                    var wakeUp = parsedGuard.Date;

                    for (var i = asSleep.Minute; i < wakeUp.Minute; i++)
                    {
                        if (!guards[parsedGuard.Id].ContainsKey(i))
                        {
                            guards[parsedGuard.Id].Add(i, 0);
                        }

                        guards[parsedGuard.Id][i]++;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return guards;
        }
    
    private class ParsedGuard
    {
        public int Id { get; set; }

        public string Type { get; }

        public DateTime Date { get; }

        public ParsedGuard(string line)
        {
            var beginShift = new Regex(@"\[(.*)\] Guard #(\d+) begins shift");
            var fallAsleep = new Regex(@"\[(.*)\] falls asleep");
            var wakeUp = new Regex(@"\[(.*)\] wakes up");

            var beginShiftMatch = beginShift.Match(line);
            var fallAsleepMatch = fallAsleep.Match(line);
            var wakeUpMatch = wakeUp.Match(line);

            // Guard started shift
            if (beginShiftMatch.Success)
            {
                Id = int.Parse(beginShiftMatch.Groups[2].Value);
                Date = DateTime.Parse(beginShiftMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                Type = "shiftStart";
            }
            else if (fallAsleepMatch.Success)
            {
                Id = -1;
                Date = DateTime.Parse(fallAsleepMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                Type = "asSleep";
            }
            else if (wakeUpMatch.Success)
            {
                Id = -1;
                Date = DateTime.Parse(wakeUpMatch.Groups[1].Value);
                Type = "wakeUp";
            }
            else
            {
                throw new NotImplementedException($"Weird message? {line}");
            }
        }
    }
}
