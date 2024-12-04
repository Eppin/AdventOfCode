namespace AdventOfCode._2015;

public class Day22 : Day
{
    private int _minimumSpend = int.MaxValue;
    private static Player? _boss;

    public Day22() : base()
    {
    }

    [Answer("1269", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("1309", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    public int Solve(bool isPartB)
    {
        _boss ??= Parse();

        for (var i = 0; i < 500_000; i++)
            Battle(isPartB);

        return _minimumSpend;
    }

    private void Battle(bool isPartB)
    {
        var queue = new Queue<State>();

        var state = new State
        {
            MyTurn = true,
            Player = new(50, 0, 500),
            Boss = _boss!.Clone()
        };

        queue.Enqueue(state);

        while (queue.TryDequeue(out state))
        {
            if (!HandleTurn(state, isPartB))
                break;

            var newState = (State)state.Clone();

            if (newState.Player.HitPoints > 0)
                queue.Enqueue(newState);
        }
    }

    private bool HandleTurn(State state, bool isPartB)
    {
        if (isPartB && state.MyTurn)
        {
            state.Player.HitPoints -= 1;

            if (!ContinueGame(state))
                return false;
        }

        CastSpells(state);
        ReduceSpells(state);

        if (!ContinueGame(state))
            return false;

        if (state.MyTurn)
        {
            var possibleSpell = CanCastSpell(state);
            if (possibleSpell == null)
                return false;

            CastSpell(state, possibleSpell);
        }
        else
        {
            var hasArmor = state.ActiveSpells.ContainsKey(SpellType.Shield);
            var armor = hasArmor
                ? Spells.Single(s => s.Type == SpellType.Shield).Armor
                : 0;

            state.Player.HitPoints -= Math.Max(state.Boss.Attack - armor, 1);
        }

        // Switch turns
        state.MyTurn = !state.MyTurn;

        return ContinueGame(state);
    }

    private bool ContinueGame(State state)
    {
        if (state.Boss.HitPoints <= 0)
        {
            _minimumSpend = Math.Min(_minimumSpend, state.ManaSpend);
            return false;
        }

        if (state.Player.HitPoints <= 0)
            return false;

        return true;
    }

    private static void CastSpells(State state)
    {
        foreach (var (spellType, _) in state.ActiveSpells)
        {
            var spell = Spells.Single(s => s.Type == spellType);
            CastSpell(state, spell, true);
        }
    }

    private static void CastSpell(State state, Spell spell, bool isEffect = false)
    {
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

            case SpellType.Poison when isEffect:
                state.Boss.HitPoints -= spell.Damage;
                break;

            case SpellType.Recharge when isEffect:
                state.Player.Mana += spell.Mana;
                break;
        }

        if (spell.Turns > 0 && !state.ActiveSpells.ContainsKey(spell.Type))
        {
            state.ActiveSpells.Add(spell.Type, spell.Turns);
        }
    }

    private static void ReduceSpells(State state)
    {
        foreach (var (spellType, _) in state.ActiveSpells)
        {
            var spell = Spells.Single(s => s.Type == spellType);
            state.ActiveSpells[spellType] -= 1;

            if (state.ActiveSpells[spellType] <= 0)
                state.ActiveSpells.Remove(spell.Type);
        }
    }

    private static Spell? CanCastSpell(State state)
    {
        var active = state.ActiveSpells;
        var manaAvailable = state.Player.Mana;

        var spells = Spells.Where(s => !active.ContainsKey(s.Type) && manaAvailable >= s.Cost).ToList();
        spells.Shuffle();

        return spells.FirstOrDefault();
    }

    private Player Parse()
    {
        var input = SplitInput.ToList();

        var hp = int.Parse(input[0].Replace("Hit Points:", ""));
        var attack = int.Parse(input[1].Replace("Damage:", ""));

        return new Player(hp, attack, 0);
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

    private sealed class Player(int hitPoints, int attack, int mana)
    {
        public int HitPoints { get; set; } = hitPoints;
        public int Attack { get; } = attack;
        public int Mana { get; set; } = mana;

        public Player Clone()
        {
            return new Player(HitPoints, Attack, Mana);
        }
    }

    private sealed class Spell(SpellType type, int cost, int damage, int armor, int heal, int mana, int turns) : ICloneable
    {
        public SpellType Type { get; } = type;
        public int Cost { get; } = cost;
        public int Damage { get; } = damage;
        public int Armor { get; } = armor;
        public int Heal { get; } = heal;
        public int Mana { get; } = mana;
        public int Turns { get; } = turns;

        public object Clone()
        {
            return (Spell)MemberwiseClone();
        }
    }

    private sealed class State : ICloneable
    {
        public bool MyTurn { get; set; }
        public int ManaSpend { get; set; }

        public Player Player { get; init; } = null!;
        public Player Boss { get; init; } = null!;

        public Dictionary<SpellType, int> ActiveSpells { get; private init; } = [];

        public object Clone()
        {
            return new State
            {
                MyTurn = MyTurn,
                ManaSpend = ManaSpend,
                Player = Player.Clone(),
                Boss = Boss.Clone(),
                ActiveSpells = ActiveSpells
            };
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
