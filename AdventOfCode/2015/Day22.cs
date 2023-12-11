namespace AdventOfCode._2015;

using System.Net;
using System.Numerics;
using Utils;

public class Day22 : Day
{
    private readonly List<State> _beatableStates = [];
    
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
        var state = new State
        {
            Player = new(50, 0, 0, 500), //new(10, 0, 0, 250),
            Boss = Parse() // new(14, 8, 0, 0)
        };

        Battle(state);

        var winned = _beatableStates.Count;


        return _beatableStates.Min(s => s.ManaSpend);
    }

    private void Battle(State state)
    {
        //Console.WriteLine($"-- Player turn: {state.Turn} --");
        //Console.WriteLine($"- Player has {state.Player.HitPoints} hit point{(state.Player.HitPoints == 1 ? "" : "s")}, {state.Player.Armor} armor, {state.Player.Mana} mana");
        //Console.WriteLine($"- Boss has {state.Boss.HitPoints} hit point{(state.Boss.HitPoints == 1 ? "" : "s")}");
        // . My turn
        // 1. Cast + Reduce spell effects
        // 1.1 Cast active spells
        CastSpells(state);
        // 1.2 Reduce turn active spells
        ReduceSpells(state);

        // 2. Cast spell
        var possibleSpells = CanCastSpell(state);
        if (possibleSpells == null || possibleSpells.Count == 0)
        {
            //Console.WriteLine("Player lost! Can't cast..");
            //Console.WriteLine("--");
            //Console.WriteLine();
            return;
        }

        foreach (var possibleSpell in possibleSpells)
        {
            //Console.WriteLine($"Spell attempt: {possibleSpell.Type}, T:{state.Turn}");
            var copied = (State)state.Clone();

            CastSpell(copied, possibleSpell);

            //Console.WriteLine();
            //Console.WriteLine($"-- Boss turn: {copied.Turn} --");
            //Console.WriteLine($"- Player has {copied.Player.HitPoints} hit point{(copied.Player.HitPoints == 1 ? "" : "s")}, {copied.Player.Armor} armor, {copied.Player.Mana} mana");
            //Console.WriteLine($"- Boss has {copied.Boss.HitPoints} hit point{(copied.Boss.HitPoints == 1 ? "" : "s")}");

            if (copied.Boss.HitPoints <= 0)
            {
                _beatableStates.Add(copied);
                //Console.WriteLine("Player WON! T:{copied.Turn}");
                //Console.WriteLine("--");
                //Console.WriteLine();
                continue;
            }

            // . Boss turn
            // 1. Cast + Reduce spell effects
            // 1.1 Cast active spells
            CastSpells(copied);
            // 1.2 Reduce turn active spells
            ReduceSpells(copied);

            if (copied.Boss.HitPoints <= 0)
            {
                _beatableStates.Add(copied);
                //Console.WriteLine("Player WON (in boss' turn! T:{copied.Turn}");
                //Console.WriteLine("--");
                //Console.WriteLine();
                continue;
            }

            // 2. Boss Attack
            copied.Player.HitPoints -= Math.Max(copied.Boss.Attack - copied.Player.Armor, 1);

            // Can we do another round?
            if (copied.Player.HitPoints <= 0)
            {
                //Console.WriteLine($"Player lost! No hit points. T:{copied.Turn}");
                //Console.WriteLine("--");
                //Console.WriteLine();
                continue;
            }

            // Call Battle()
            //Console.WriteLine();
            copied.Turn++;
            Battle(copied);
        }
    }

    private static void CastSpells(State state)
    {
        foreach (var spell in state.Spells)
        {
            CastSpell(state, spell, true);
        }
    }

    private static void CastSpell(State state, Spell spell, bool isEffect = false)
    {
        //Console.WriteLine($"Cast: {spell.Type}");

        if (!isEffect)
        {
            state.Player.Mana -= spell.Cost;
            state.ManaSpend += spell.Cost;
        }

        switch (spell.Type)
        {
            case SpellType.MagicMissile:
                state.Boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Drain:
                state.Player.HitPoints += spell.Heal;
                state.Boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Shield:
                state.Player.Armor = spell.Armor;
                break;

            case SpellType.Poison when isEffect:
                state.Boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Recharge when isEffect:
                state.Player.Mana += spell.Mana;
                break;
        }

        if (spell.Turns > 0 && state.Spells.TrueForAll(p => p.Type != spell.Type))
        {
            state.Spells.Add(spell);
        }
    }

    private static void ReduceSpells(State state)
    {
        var spell = state.Spells;

        if (spell.Count == 0)
            return;

        for (var i = spell.Count - 1; i >= 0; i--)
        {
            spell[i].Turns -= 1;

            //Console.WriteLine($"Reduced: {spell[i].Type}, timer {spell[i].Turns}");

            if (spell[i].Turns <= 0)
            {
                // Special case
                if (spell[i].Type == SpellType.Shield)
                    state.Player.Armor -= spell[i].Armor;

                spell.Remove(spell[i]);
            }
        }
    }

    private static List<Spell>? CanCastSpell(State state)
    {
        //var spells = new List<Spell>();

        var active = state.Spells.Select(s => s.Type);
        var manaAvailable = state.Player.Mana;

        var availableSpells = Spells.Where(s => !active.Contains(s.Type) && manaAvailable >= s.Cost).ToList();

        //Console.WriteLine($"Available spells: {string.Join(',', availableSpells.Select(s => s.Type))}");
        

        return availableSpells.Count == 0
            ? null
            : availableSpells;
    }

    private Player Parse()
    {
        var input = SplitInput.ToList();

        var hp = int.Parse(input[0].Replace("Hit Points:", ""));
        var attack = int.Parse(input[1].Replace("Damage:", ""));

        return new Player(hp, attack, 0, 0);
    }

    private static IEnumerable<Spell> Spells =>
        new Spell[]
        {
            new(SpellType.MagicMissile, 53, 4, 0, 0, 0, 0),
            new(SpellType.Drain, 73, 2, 0, 2, 0, 0),
            new(SpellType.Shield, 113, 0, 7, 0, 0, 6),
            new(SpellType.Poison, 173, 3, 0, 0, 0, 6),
            new(SpellType.Recharge, 229, 0, 0, 0, 101, 5)
        };

    private sealed class Player(int hitPoints, int attack, int armor, int mana)
    {
        public int HitPoints { get; set; } = hitPoints;
        public int Attack { get; set; } = attack;
        public int Armor { get; set; } = armor;
        public int Mana { get; set; } = mana;
    }

    private sealed class Spell(SpellType type, int cost, int damage, int armor, int heal, int mana, int turns) : ICloneable
    {
        public SpellType Type { get; set; } = type;
        public int Cost { get; set; } = cost;
        public int Damage { get; set; } = damage;
        public int Armor { get; set; } = armor;
        public int Heal { get; set; } = heal;
        public int Mana { get; set; } = mana;
        public int Turns { get; set; } = turns;

        public object Clone()
        {
            return (Spell)MemberwiseClone();
        }
    }

    private sealed class State : ICloneable
    {
        public int Turn { get; set; }
        public int ManaSpend { get; set; }
        
        public Player Player { get; set; }
        public Player Boss { get; set; }

        public List<Spell> Spells { get; set; } = [];

        public object Clone()
        {
            var state = (State)MemberwiseClone();

            state.Player = new Player(Player.HitPoints, Player.Attack, Player.Armor, Player.Mana);
            state.Boss = new Player(Boss.HitPoints, Boss.Attack, Boss.Armor, Boss.Mana);

            state.Spells = [];
            foreach (var address in this.Spells)
            {
                var clonedAddress = (Spell)address.Clone();

                state.Spells.Add(clonedAddress);
            }

            return state;
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
