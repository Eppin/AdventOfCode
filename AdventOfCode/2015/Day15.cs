namespace AdventOfCode._2015;

public partial class Day15 : Day
{
    private int _totalIngredients;
    private const int TotalTeaspoons = 100;

    public Day15() : base()
    {
    }

    [Answer("222870", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("117936", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var ingredients = Parse().ToList();
        _totalIngredients = ingredients.Count;

        var calculated = Nested(_totalIngredients, TotalTeaspoons, new List<int>())
            .Where(x => x.Sum(g => g) == TotalTeaspoons && x.All(g => g > 0))
            .Select(x => Sum(x, ingredients));

        return isPartB
            ? calculated.Where(x => x.Calories == 500).Max(x => x.Score)
            : calculated.Max(x => x.Score);
    }

    private static IEnumerable<List<int>> Nested(int ingredients, int teaspoons, List<int> x)
    {
        if (ingredients == 0)
            return new List<List<int>> { x };

        var results = new List<List<int>>();
        for (var i = 1; i <= teaspoons; i++)
        {
            var a = new List<int>(x) { i };

            var ab = Nested(ingredients - 1, teaspoons - i, a);
            results.AddRange(ab);
        }

        return results;
    }

    private static (int Score, int Calories) Sum(IEnumerable<int> teaspoons, List<Ingredient> ingredients)
    {
        var capacity = 0;
        var durability = 0;
        var flavor = 0;
        var texture = 0;
        var calories = 0;

        foreach (var (teaspoon, index) in teaspoons.Select((t, i) => (t, i)))
        {
            var ingredient = ingredients[index];

            capacity += ingredient.Capacity * teaspoon;
            durability += ingredient.Durability * teaspoon;
            flavor += ingredient.Flavor * teaspoon;
            texture += ingredient.Texture * teaspoon;

            calories += ingredient.Calories * teaspoon;
        }

        if (capacity < 0 || durability < 0 || flavor < 0 || texture < 0)
            return (0, calories);

        return (capacity * durability * flavor * texture, calories);
    }

    private IEnumerable<Ingredient> Parse()
    {
        foreach (var input in SplitInput)
        {
            var regex = IngredientRegex().Match(input);
            if (!regex.Success)
                throw new DataException("Regex can't fail");

            var name = regex.Groups[1].Value;
            var capacity = int.Parse(regex.Groups[2].Value);
            var durability = int.Parse(regex.Groups[3].Value);
            var flavor = int.Parse(regex.Groups[4].Value);
            var texture = int.Parse(regex.Groups[5].Value);
            var calories = int.Parse(regex.Groups[6].Value);

            yield return new(name, capacity, durability, flavor, texture, calories);
        }
    }

    private record struct Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories);

    [GeneratedRegex(@"^(.*): capacity (-\d+|\d+), durability (-\d+|\d+), flavor (-\d+|\d+), texture (-\d+|\d+), calories (-\d+|\d+)")]
    private static partial Regex IngredientRegex();
}
