namespace AdventOfCode._2023;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("2545", Regular)]
    public override object SolveA()
    {
        const int redPossible = 12;
        const int greenPossible = 13;
        const int bluePossible = 14;

        var games = Parse();
        var total = 0;

        foreach (var game in games)
        {
            var validGame = false;

            foreach (var set in game.Colors)
            {
                var validSet = false;

                foreach (var colorCount in set)
                {
                    if (colorCount is { Color: Color.Red, Count: > redPossible })
                    {
                        validSet = false;
                        break;
                    }

                    if (colorCount is { Color: Color.Green, Count: > greenPossible })
                    {
                        validSet = false;
                        break;
                    }

                    if (colorCount is { Color: Color.Blue, Count: > bluePossible })
                    {
                        validSet = false;
                        break;
                    }

                    validSet = true;
                }

                if (!validSet)
                {
                    validGame = false;
                    break;
                }

                validGame = true;
            }

            if (validGame)
                total += game.GameId;
        }

        return total;
    }

    [Answer("78111", Regular)]
    public override object SolveB()
    {
        return Parse()
            .Sum(game => game.Colors
                .SelectMany(cc => cc)
                .GroupBy(cc => cc.Color)
                .Select(g => g.Max(cc => cc.Count))
                .Aggregate(1, (previous, next) => previous * next))
            .ToString();
    }

    private List<Game> Parse()
    {
        var games = new List<Game>();

        foreach (var line in SplitInput)
        {
            var split = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 2 || !int.TryParse(split[0].Replace("Game ", string.Empty), out var gameId))
                throw new InvalidDataException("Line must contain an ID and cubes");

            split = split[1].Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var sets = new List<List<ColorCount>>();

            foreach (var set in split)
            {
                var colors = new List<ColorCount>();

                var cs = set.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                foreach (var c in cs)
                {
                    // Extract count + color
                    var cc = c.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                    if (cc.Length != 2 || !int.TryParse(cc[0], out var count))
                        throw new InvalidDataException("Set should contain a count and color");

                    var color = cc[1] switch
                    {
                        "blue" => Color.Blue,
                        "red" => Color.Red,
                        "green" => Color.Green,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    colors.Add(new ColorCount(color, count));
                }

                sets.Add(colors);
            }

            games.Add(new Game(gameId, sets));
        }

        return games;
    }

    private record struct Game(int GameId, List<List<ColorCount>> Colors);

    private record struct ColorCount(Color Color, int Count);

    private enum Color
    {
        Blue,
        Red,
        Green
    }
}
