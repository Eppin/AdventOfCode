namespace AdventOfCode._2015;

public class Day21 : Day
{
    public Day21() : base()
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
        var boss = Parse();
        var items = StoreSet();

        var cheapest = int.MaxValue;

        foreach (var (costs, damage, armor) in items)
        {
            Console.WriteLine($"C{costs}/D{damage}/{armor}");
            var player = new Player(100, damage, armor);
            var won = Battle(boss, player);

            if (won && costs < cheapest)
                cheapest = costs;
        }

        return cheapest;
    }

    private static bool Battle(Player boss, Player player)
    {
        var won = false;
        do
        {
            boss.HitPoints -= Math.Max(player.Attack - boss.Armor, 1);
            Console.WriteLine($"Atk boss: {boss.HitPoints}");

            if (boss.HitPoints <= 0)
            {
                won = true;
                break;
            }

            player.HitPoints -= Math.Max(boss.Attack - player.Armor, 1);
            Console.WriteLine($"Atk play: {player.HitPoints}");
        } while (boss.HitPoints > 0 && player.HitPoints > 0);

        Console.WriteLine($"Won: {won}");
        return won;
    }

    private static IEnumerable<(int Costs, int Damage, int Armor)> StoreSet()
    {
        var (weapons, armors, ringSet1, ringSet2) = GetStore();

        foreach (var t3 in weapons)
        {
            foreach (var t2 in armors)
            {
                foreach (var t1 in ringSet1)
                {
                    foreach (var t in ringSet2)
                    {
                        var cost = t3.Cost + t2.Cost + t1.Cost + t.Cost;
                        var damage = t3.Damage + t2.Damage + t1.Damage + t.Damage;
                        var armor = t3.Armor + t2.Armor + t1.Armor + t.Armor;

                        yield return (cost, damage, armor);
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

    private static (List<Item> Weapons, List<Item> Armors, List<Item> RingSet1, List<Item> RingSet2) GetStore()
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

        var ringSet1 = new List<Item>
        {
            new("None", 0, 0, 0),
            new("Damage +1", 25, 1, 0),
            new("Damage +2", 50, 2, 0),
            new("Damage +3", 75, 3, 0)
        };

        var ringSet2 = new List<Item>
        {
            new("None", 0, 0, 0),
            new("Defense +1", 20, 0, 1),
            new("Defense +2", 40, 0, 2),
            new("Defense +3", 80, 0, 3)
        };

        return (weapons, armors, ringSet1, ringSet2);
    }

    private record struct Player(int HitPoints, int Attack, int Armor);

    private record struct Item(string Name, int Cost, int Damage, int Armor);
}
