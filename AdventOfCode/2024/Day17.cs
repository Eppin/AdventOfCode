namespace AdventOfCode._2024;

public class Day17 : Day
{
    public Day17() : base()
    {
    }

    [Answer("4,6,3,5,6,3,5,2,1,0", Example, Data = "Register A: 729{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,1,5,4,3,0")]
    [Answer("0,1,2", Example, Data = "Register A: 10{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 5,0,5,1,5,4")]
    [Answer("4,2,5,6,7,7,7,7,3,1,0", Example, Data = "Register A: 2024{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,1,5,4,3,0")]
    [Answer("7,1,3,7,5,1,0,3,4", Regular)]
    public override object SolveA()
    {
        var (registerA, registerB, registerC, program) = Parse();
        return string.Join(',', Solve(registerA, registerB, registerC, program));
    }

    [Answer("117440", Example, Data = "Register A: 2024{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,3,5,4,3,0")]
    [Answer("190384113204239", Regular)]
    public override object SolveB()
    {
        var (_, registerB, registerC, program) = Parse();

        var start = 0L;
        var shift = 0;
        var programLength = 1;

        while (true)
        {
            for (var i = 0L; i < 1024; i++)
            {
                var registerA = (i << (shift * 3)) + start;
                var solve = Solve(registerA, registerB, registerC, program);

                var pr = program.Take(programLength);

                if (!solve.Take(programLength).SequenceEqual(pr)) continue;
                if (solve.SequenceEqual(program)) return registerA;

                start = registerA;
                programLength++;
            }

            shift++;
        }
    }

    private static List<long> Solve(long registerA, long registerB, long registerC, List<long> program)
    {
        var pointer = 0L;
        var results = new List<long>();

        do
        {
            var opcode = program[(int)pointer];
            var operand = Operand(program[(int)pointer + 1], registerA, registerB, registerC);

            switch (opcode)
            {
                case 0: // adv
                    registerA >>= (int)operand;
                    pointer += 2;
                    break;

                case 1: // bxl
                    registerB ^= operand;
                    pointer += 2;
                    break;

                case 2: // bst
                    var bst2 = operand % 8;
                    if (bst2 > 7)
                        bst2 = 7;

                    registerB = bst2;
                    pointer += 2;
                    break;

                case 3: // jnz
                    if (registerA != 0)
                        pointer = operand;
                    else
                        pointer += 2;
                    break;

                case 4: //bxc
                    registerB ^= registerC;
                    pointer += 2;
                    break;

                case 5: // out
                    var out5 = operand % 8;
                    results.Add(out5);
                    pointer += 2;
                    break;

                case 6: //bdv
                    registerB = registerA >> (int)operand;
                    pointer += 2;
                    break;

                case 7: // cdv
                    registerC = registerA >> (int)operand;
                    pointer += 2;
                    break;
            }
        } while (pointer < program.Count);

        return results;
    }

    private static long Operand(long operand, long registerA, long registerB, long registerC)
    {
        return operand switch
        {
            <= 3 => operand,
            4 => registerA,
            5 => registerB,
            6 => registerC,
            7 => operand,
            _ => throw new Exception($"Combo operand {operand} is reserved and will not appear in valid programs")
        };
    }

    private (long RegisterA, long RegisterB, long RegisterC, List<long> Program) Parse()
    {
        var lines = GetSplitInput();

        var registerA = long.Parse(lines[0].Replace("Register A: ", string.Empty));
        var registerB = long.Parse(lines[1].Replace("Register B: ", string.Empty));
        var registerC = long.Parse(lines[2].Replace("Register C: ", string.Empty));

        var program = lines[3]
            .Replace("Program: ", string.Empty).Split(',')
            .Select(long.Parse)
            .ToList();

        return (registerA, registerB, registerC, program);
    }
}
