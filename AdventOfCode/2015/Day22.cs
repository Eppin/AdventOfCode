namespace AdventOfCode._2015;

using Utils;

public class Day22 : Day
{
    public Day22() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private int Solve(bool isPartB)
    {
        var maxTries = 100_000;
        var tries = 0;
        var won = false;

        var spend = int.MaxValue;

        while (tries <= maxTries)
        {
            var boss = Parse();
            var player = new Player(50, 0, 0, 500, new List<Spell>());

            (won, var s) = Battle(boss, player);

            if (won && s < spend)
                spend = s;
            
            Console.WriteLine($"{won}, {spend}");
            tries++;
        }

        //
        //
        // foreach (var (costs, damage, armor) in items)
        // {
        //     var player = new Player(10, damage, 250);
        //     // var won = Battle(boss, player);
        //     //
        //     // if (won)
        //     //     cheapest = Math.Min(cheapest, costs);
        //     // else
        //     //     expensive = Math.Max(expensive, costs);
        // }
        //
        // // return isPartB ? expensive : cheapest;
        // return 0;
        return tries;
    }

    private static (bool, int) Battle(Player boss, Player player)
    {
        var turn = 1;
        var won = false;
        var spend = 0;
        do
        {
            // Console.WriteLine($"-- Turn: {turn} --");
            // Console.WriteLine($"{player.HitPoints}, {player.Attack}, {player.Armor}, {player.Mana}");
            // Console.WriteLine($"{boss.HitPoints}");
            // Console.WriteLine("--");
            //
            // Console.WriteLine("- Cast any effect + reduce (player turn)");
            ReduceSpells(player, boss);

            // Console.WriteLine("- Cast a random spell (player turn)");
            var spell = CanCastSpell(player);
            if (spell == null)
                break;

            spend += CastSpell(player, boss, spell);

            if (boss.HitPoints <= 0)
            {
                won = true;
                break;
            }

            // Console.WriteLine("- Cast any effect + reduce (boss turn starts)");
            ReduceSpells(player, boss);
            player.HitPoints -= Math.Max(boss.Attack - player.Armor, 1);
            turn++;
        } while (boss.HitPoints > 0 && player.HitPoints > 0);

        // Console.WriteLine($"{won}: {spend}");
        return (won, spend);
    }

    private static void ReduceSpells(Player player, Player boss)
    {
        var spell = player.Spells;

        if (spell?.Any() == false)
            return;

        for (var i = spell.Count - 1; i >= 0; i--)
        {
            spell[i].Turns -= 1;
            CastSpell(player, boss, spell[i]);
            // Console.WriteLine($"Spell {spell[i].Type} effect went to {spell[i].Turns}");

            if (spell[i].Turns <= 0)
            {
                if (spell[i].Type == SpellType.Shield)
                    player.Armor -= spell[i].Armor;

                player.Spells.Remove(spell[i]);
            }
        }
    }

    private static Spell? CanCastSpell(Player player)
    {
        var spells = new List<Spell>();
        foreach (var spell in GetSpells())
        {
            if (player.Spells?.All(s => !s.Type.Equals(spell.Type)) == true)
            {
                // Console.WriteLine($"Can cast {spell.Type}");
                spells.Add(spell);
            }
        }

        if (!spells.Any())
            return null;

        spells.Shuffle();
        return spells.First();
    }

    private static int CastSpell(Player player, Player boss, Spell spell)
    {
        // Console.WriteLine($"Cast spell {spell.Type}");

        switch (spell.Type)
        {
            case SpellType.MagicMissile:
                boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Drain:
                player.HitPoints += spell.Heal;
                boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Shield:
                player.Armor = spell.Armor;
                break;

            case SpellType.Poison:
                boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Recharge:
                player.Mana += spell.Mana;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(spell.Type), spell.Type, null);
        }

        if (spell.Turns > 0 && player.Spells?.All(p => p.Type != spell.Type) == true)
        {
            player.Spells.Add(spell);
            return spell.Cost;
        }

        return 0;
    }

    private Player Parse()
    {
        var input = SplitInput.ToList();

        var hp = int.Parse(input[0].Replace("Hit Points:", ""));
        var attack = int.Parse(input[1].Replace("Damage:", ""));

        return new Player(hp, attack, 0, 0, new List<Spell>());
    }

    private static IEnumerable<Spell> GetSpells()
    {
        return new Spell[]
        {
            new(SpellType.MagicMissile, 53, 4, 0, 0, 0, 0),
            new(SpellType.Drain, 73, 2, 0, 0, 0, 2),
            new(SpellType.Shield, 113, 0, 7, 0, 0, 6),
            new(SpellType.Poison, 173, 3, 0, 0, 0, 6),
            new(SpellType.Recharge, 229, 0, 0, 0, 101, 5)
        };
    }

    private class Player
    {
        public int HitPoints { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
        public int Mana { get; set; }
        public List<Spell>? Spells { get; set; }

        public Player(int hitPoints, int attack, int armor, int mana, List<Spell>? spells = null)
        {
            HitPoints = hitPoints;
            Attack = attack;
            Armor = armor;
            Mana = mana;
            Spells = spells;
        }
    }

    private class Spell
    {
        public SpellType Type { get; set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int Heal { get; set; }
        public int Mana { get; set; }
        public int Turns { get; set; }

        public Spell(SpellType type, int cost, int damage, int armor, int heal, int mana, int turns)
        {
            Type = type;
            Cost = cost;
            Damage = damage;
            Armor = armor;
            Heal = heal;
            Mana = mana;
            Turns = turns;
        }
    }

    private enum SpellType
    {
        MagicMissile,
        Drain,
        Shield,
        Poison,
        Recharge
    }
}
