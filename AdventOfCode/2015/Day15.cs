namespace AdventOfCode._2015;

using System.Data;
using System.Text.RegularExpressions;

public partial class Day15 : Day
{
    private int Ingredients;
    private const int Teaspoons = 100;

    public Day15() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var ingredients = Parse().ToList();
        Ingredients = ingredients.Count;

        // var ingredients = 3;
        // var teaspoons = 5;

        var tot = 0;

        // for (var i = 0; i <= Teaspoons; i++)
        // {
        //     for (var j = 0; j <= Teaspoons - i; j++)
        //     {
        //         for (var k = 0; k <= Teaspoons - i - j; k++)
        //         {
        //             // if(i + j + k == teaspoons && i != 0 && j != 0 && k != 0)
        //             tot++;
        //             Console.WriteLine($"{i},{j},{k} = {i + j + k == Teaspoons} {i != 0 && j != 0 && k != 0} {tot}");
        //         }
        //     }
        // }

        var xx = Nested2(Ingredients, Teaspoons, new List<int>());
        var xg = xx.Where(x => x.Sum(g => g) == Teaspoons && x.All(g => g > 0));

        var max = 0;
        var maxPartB = 0;

        foreach (var xa in xg)
        {
            // Console.WriteLine("Sum start:");
            var s = Sum(xa, ingredients);

            if (s.Item1 > max)
                max = s.Item1;

            if (s.Item2 == 500 && s.Item1 > maxPartB)
                maxPartB = s.Item1;

            // Console.WriteLine($"Tot{s.Item1}, cal: {s.Item2}");
        }

        return isPartB
            ? maxPartB
            : max;
    }

    private static int tot = 0;

    private List<List<int>> Nested2(int ingredients, int teaspoons, List<int> x)
    {
        if (ingredients == 0)
            return new List<List<int>> { x };

        var results = new List<List<int>>();
        for (var i = 0; i <= teaspoons; i++)
        {
            var a = new List<int>(x) { i };

            // if (a.Count == Ingredients)
            // {
            //     tot++;
            //     Console.WriteLine($"Ingr: {ingredients}, teas: {i}, tot: {string.Join(",", a)}, tot2: {tot}");
            // }

            var ab = Nested2(ingredients - 1, teaspoons - i, a);
            results.AddRange(ab);
        }

        return results;
    }

    // Working example
    // private void Nested2(int ingredients, int teaspoons, string x)
    // {
    //     if (ingredients == 0)
    //         return;
    //
    //     for (var i = 0; i <= teaspoons; i++)
    //     {
    //         if ($"{x}{i}".Length == Ingredients)
    //         {
    //             tot++;
    //             Console.WriteLine($"Ingr: {ingredients}, teas: {i}, tot: {x}{i}, tot2: {tot}");
    //         }
    //
    //         Nested2(ingredients - 1, teaspoons - i, $"{x}{i}");
    //     }
    // }

    // private void Nested(int ingredients, int teaspoons)
    // {
    //     if (ingredients == 0)
    //     {
    //         Console.WriteLine($"[{ingredients}]={teaspoons} <-- bottom");
    //         return;
    //     }
    //
    //     for (var i = 0; i < teaspoons; i++)
    //     {
    //         Console.WriteLine($"[{ingredients}]={i}");
    //         Nested(ingredients - 1, teaspoons - i);
    //     }
    // }

    // private void Nested(List<int> spread, int position, int teaspoon)
    // {
    // if (spread[position] == 1)
    // {
    //     spread[position - 1] = 1;
    //     spread[position - 2] += 1;
    //     spread[position] += teaspoon - spread.Sum();
    // }
    // else

    // if (position == 0)
    // {
    //     if (spread.Last() == 1)
    //         return;
    //
    //     position = spread.Count - 1;
    // }

    // spread[position] -= 1;
    // spread[position - 1] += 1;
    //
    // if (spread.Contains(0))
    //     return;
    //
    // Console.WriteLine(string.Join(",", spread));
    //
    // if (spread[position] == 1)
    // {
    //     spread[position - 2] += 1;
    //     spread[position - 1] = 1;
    //     spread[position] += teaspoon - spread.Sum();
    //     
    //     Console.WriteLine(string.Join(",", spread));
    //     
    //     Nested(spread, position, teaspoon);
    // }
    // else
    // {
    //     var pos = spread.Last() == 1 ? position - 2 : position;
    //
    //
    //
    //     Nested(spread, pos, teaspoon);
    // }
    // }

    private (int, int) Sum(List<int> teaspoons, List<Ingredient> ingredients)
    {
        // Console.WriteLine($"Spoons: {string.Join(",", teaspoons)}, ingr: {String.Join(",", ingredients)}");

        var capacity = 0;
        var durability = 0;
        var flavor = 0;
        var texture = 0;
        var calories = 0;

        foreach (var (teaspoon, index) in teaspoons.Select((t, i) => (t, i)))
        {
            var ingredient = ingredients[index];
            // foreach (var ingredient in ingredients)
            // {
            capacity += ingredient.Capacity * teaspoon;
            durability += ingredient.Durability * teaspoon;
            flavor += ingredient.Flavor * teaspoon;
            texture += ingredient.Texture * teaspoon;

            calories += ingredient.Calories * teaspoon;

            // yield return (capacity + durability + flavor + texture, calories);
            // }
        }

        if (capacity < 0 || durability < 0 || flavor < 0 || texture < 0)
            return (0, calories);

        return (Math.Max(capacity * durability * flavor * texture, 0), calories);
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
