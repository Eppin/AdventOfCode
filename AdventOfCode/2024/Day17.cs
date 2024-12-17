namespace AdventOfCode._2024;

public class Day17 : Day
{
    public Day17() : base()
    {
    }

    [Answer("4,6,3,5,6,3,5,2,1,0", Example,
        Data = "Register A: 729{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,1,5,4,3,0")]
    //[Answer("", Example, Data = "Register A: 0{nl}Register B: 1{nl}Register C: 9{nl}{nl}Program: 2,6")]
    //[Answer("0,1,2", Example, Data = "Register A: 10{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 5,0,5,1,5,4")]
    //[Answer("4,2,5,6,7,7,7,7,3,1,0", Example, Data = "Register A: 2024{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,1,5,4,3,0")]
    //[Answer("", Example, Data = "Register A: 0{nl}Register B: 29{nl}Register C: 0{nl}{nl}Program: 1,7")]
    //[Answer("", Example, Data = "Register A: 0{nl}Register B: 2024{nl}Register C: 43690{nl}{nl}Program: 4,0")]
    [Answer("7,1,3,7,5,1,0,3,4", Regular)]
    public override string SolveA()
    {
        return Solve();
    }

    [Answer("0,3,5,4,3,0", Example, Data = "Register A: 117440{nl}Register B: 0{nl}Register C: 0{nl}{nl}Program: 0,3,5,4,3,0")]
    public override string SolveB()
    {
        return Solve();
    }

    private string Solve()
    {
        var (registerA, registerB, registerC, program) = Parse();
        var pointer = 0;
        var results = new List<int>();

        do
        {
            var opcode = program[pointer];
            var operand = Operand(program[pointer + 1], registerA, registerB, registerC);

            Console.WriteLine($"O:{opcode} en {operand}");

            switch (opcode)
            {
                case 0: // adv
                    var adv0 = registerA / Math.Pow(2, operand); // TODO is this safe enough?
                    registerA = (int)adv0;
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

                    registerB = bst2; // TODO keep lowest 3 bits!!
                    pointer += 2;
                    break;

                case 3: // jnz
                    if (registerA != 0)
                        pointer = operand;
                    else
                        pointer += 2;
                    break;

                case 4: //bxc
                    registerB = registerB ^ registerC;
                    pointer += 2;
                    break;

                case 5: // out
                    var out5 = operand % 8;
                    results.Add(out5);
                    pointer += 2;
                    break;

                case 6: //bdv
                    var bdv = registerA / Math.Pow(2, operand); // TODO is this safe enough?
                    registerB = (int)bdv;
                    pointer += 2;
                    break;

                case 7: // cdv
                    var cdv = registerA / Math.Pow(2, operand); // TODO is this safe enough?
                    registerC = (int)cdv;
                    pointer += 2;
                    break;
            }
        } while (pointer < program.Count);

        Console.WriteLine();
        Console.WriteLine($"A:{registerA}");
        Console.WriteLine($"B:{registerB}");
        Console.WriteLine($"C:{registerC}");
        Console.WriteLine();
        Console.WriteLine($"P:{pointer}");
        Console.WriteLine();

        return string.Join(',', results);
    }

    private static int Operand(int operand, int registerA, int registerB, int registerC)
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

    private (int RegisterA, int RegisterB, int RegisterC, List<int> Program) Parse()
    {
        var lines = GetSplitInput();

        var registerA = int.Parse(lines[0].Replace("Register A: ", string.Empty));
        var registerB = int.Parse(lines[1].Replace("Register B: ", string.Empty));
        var registerC = int.Parse(lines[2].Replace("Register C: ", string.Empty));

        var program = lines[3]
            .Replace("Program: ", string.Empty).Split(',')
            .Select(int.Parse)
            .ToList();

        return (registerA, registerB, registerC, program);
    }

    private enum InstructionType // Opcode
    {
        Adv,
        Bxl,
        Bst,
        Jnz,
        Bxc,
        Out,
        Bdv,
        Cdv
    }

    private enum RegisterType
    {
        A,
        B,
        C
    }
}
