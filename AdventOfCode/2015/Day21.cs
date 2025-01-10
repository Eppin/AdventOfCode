namespace AdventOfCode._2015;

public class Day21 : Day
{
    public Day21() : base()
    {
    }

    [Answer("91", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("158", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private int Solve(bool isPartB)
    {
        var boss = Parse();
        var items = StoreSet();

        var cheapest = int.MaxValue;
        var expensive = 0;

        foreach (var (costs, damage, armor) in items)
        {
            var player = new Player(100, damage, armor);
            var won = Battle(boss, player);

            if (won)
                cheapest = Math.Min(cheapest, costs);
            else
                expensive = Math.Max(expensive, costs);
        }

        return isPartB ? expensive : cheapest;
    }

    private static bool Battle(Player boss, Player player)
    {
        var won = false;
        do
        {
            boss.HitPoints -= Math.Max(player.Attack - boss.Armor, 1);

            if (boss.HitPoints <= 0)
            {
                won = true;
                break;
            }

            player.HitPoints -= Math.Max(boss.Attack - player.Armor, 1);
        } while (boss.HitPoints > 0 && player.HitPoints > 0);

        return won;
    }

    private static IEnumerable<(int Costs, int Damage, int Armor)> StoreSet()
    {
        var (weapons, armors, ringSet) = GetStore();

        foreach (var weapon in weapons)
        {
            foreach (var armor in armors)
            {
                foreach (var ring1 in ringSet)
                {
                    foreach (var ring2 in ringSet)
                    {
                        if (ring1.Cost == ring2.Cost)
                            continue;

                        var totalCost = weapon.Cost + armor.Cost + ring1.Cost + ring2.Cost;
                        var totalDamage = weapon.Damage + armor.Damage + ring1.Damage + ring2.Damage;
                        var totalArmor = weapon.Armor + armor.Armor + ring1.Armor + ring2.Armor;

                        yield return (totalCost, totalDamage, totalArmor);
                    }
                }
            }
        }
    }

    private Player Parse()
    {
        var input = SplitInput.ToList();

        var hp = int.Parse(input[0].Replace("Hit Points:", ""));
        var attack = int.Parse(input[1].Replace("Damage:", ""));
        var armor = int.Parse(input[2].Replace("Armor:", ""));

        return new Player(hp, attack, armor);
    }

    private static (List<Item> Weapons, List<Item> Armors, List<Item> RingSet) GetStore()
    {
        var weapons = new List<Item>
        {
            new("Dagger", 8, 4, 0),
            new("Shortsword", 10, 5, 0),
            new("Warhammer", 25, 6, 0),
            new("Longsword", 40, 7, 0),
            new("Greataxe", 74, 8, 0)
        };

        var armors = new List<Item>
        {
            new("None", 0, 0, 0),
            new("Leather", 13, 0, 1),
            new("Chainmail", 31, 0, 2),
            new("Splintmail", 53, 0, 3),
            new("Bandedmail", 75, 0, 4),
            new("Platemail", 102, 0, 5)
        };

        var ringSet = new List<Item>
        {
            new("None", 0, 0, 0),
            new("Damage +1", 25, 1, 0),
            new("Damage +2", 50, 2, 0),
            new("Damage +3", 100, 3, 0),
            new("Defense +1", 20, 0, 1),
            new("Defense +2", 40, 0, 2),
            new("Defense +3", 80, 0, 3)
        };

        return (weapons, armors, ringSet);
    }

    private record struct Player(int HitPoints, int Attack, int Armor);

    private record struct Item(string Name, int Cost, int Damage, int Armor);
}
