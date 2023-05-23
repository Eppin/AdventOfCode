namespace AdventOfCode._2015;

using System.Text.Json;

public class Day12 : Day
{
    public Day12() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(JsonSerializer.Deserialize<JsonElement>(Input), false).ToString();
    }

    public override string SolveB()
    {
        return Solve(JsonSerializer.Deserialize<JsonElement>(Input), true).ToString();
    }

    private static int Solve(JsonElement json, bool isPartB)
    {
        switch (json.ValueKind)
        {
            case JsonValueKind.Array:
            {
                return json.EnumerateArray().Aggregate(0, (current, element) => Sum(isPartB, element, current));
            }

            case JsonValueKind.Object:
            {
                var enumerate = json.EnumerateObject();

                if (isPartB && enumerate.Any(vk => vk.Value.ValueKind == JsonValueKind.String && vk.Value.GetString() == "red"))
                    return 0;

                return enumerate.Aggregate(0, (current, element) => Sum(isPartB, element.Value, current));
            }

            default:
                return json.ValueKind == JsonValueKind.Number
                    ? json.GetInt32()
                    : 0;
        }
    }

    private static int Sum(bool isPartB, JsonElement element, int sum)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var properties = element.EnumerateObject().ToList();

            if (!(isPartB && properties.Any(vk => vk.Value.ValueKind == JsonValueKind.String && vk.Value.GetString() == "red")))
                sum += Solve(element, isPartB);
        }
        else
            sum += Solve(element, isPartB);

        return sum;
    }
}
