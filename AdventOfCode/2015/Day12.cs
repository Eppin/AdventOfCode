namespace AdventOfCode._2015;

using System.Text.Json;
using System.Text.RegularExpressions;

public partial class Day12 : Day
{
    public Day12() : base()
    {
    }

    public override string SolveA()
    {
        var sum = SolveARegex().Matches(Input)
            .Select(r => r.Groups[1])
            .Where(r => r.Success)
            .Select(r => int.Parse(r.Value))
            .Sum();

        var sum2 = X(JsonSerializer.Deserialize<JsonElement>(Input), false);

        return $"{sum}/{sum2}";
    }

    public override string SolveB()
    {
        return $"{X(JsonSerializer.Deserialize<JsonElement>(Input), true)}";
    }

    private static int X(JsonElement json, bool isPartB)
    {
        // Console.WriteLine(json);

        if (json.ValueKind == JsonValueKind.Array)
        {
            var sum = 0;
            // var enumerate2 = json.EnumerateArray().ToList();
            //
            // if (enumerate2.Any(vk => vk.ValueKind == JsonValueKind.String && vk.GetString() == "red"))
            // {
            //     return 0;
            // }

            foreach (var element in json.EnumerateArray())
            {
                // if (element.ValueKind == JsonValueKind.String && element.GetString() == "red")
                // {
                //     
                // }
                if (element.ValueKind == JsonValueKind.Object)
                {
                    var x2 = element.EnumerateObject().ToList();
                    var and = x2.Any(vk => vk.Value.ValueKind == JsonValueKind.String && vk.Value.GetString() == "red");

                    if (!and)
                        sum += X(element, isPartB);
                }
                // Console.WriteLine($"RED {element.GetType()}");
                else
                    sum += X(element, isPartB);
            }

            return sum;
        }

        if (json.ValueKind == JsonValueKind.Object)
        {
            var sum = 0;
            var enumerate = json.EnumerateObject().ToList();

            if (enumerate.Any(vk => vk.Value.ValueKind == JsonValueKind.String && vk.Value.GetString() == "red"))
            {
                return 0;
            }

            foreach (var element in enumerate)
            {
                if (element.Value.ValueKind == JsonValueKind.Object)
                {
                    var x2 = element.Value.EnumerateObject().ToList();
                    var and = x2.Any(vk => vk.Value.ValueKind == JsonValueKind.String && vk.Value.GetString() == "red");

                    if (!and)
                        sum += X(element.Value, isPartB);
                }
                // Console.WriteLine($"RED {element.GetType()}");
                else
                    sum += X(element.Value, isPartB);
            }

            return sum;
        }

        if (json.ValueKind == JsonValueKind.Number)
        {
            var value = json.GetInt32();
            Console.WriteLine(value);
            return value;
        }

        return 0;
    }

    [GeneratedRegex("(-\\d+|\\d+)")]
    private static partial Regex SolveARegex();
}
