namespace AdventOfCode._2015;

using System.Text.RegularExpressions;

public partial class Day23 : Day
{
    private static List<Computer> _instructions = null!;

    public Day23() : base()
    {
        _instructions = Parse().ToList();
    }

    public override string SolveA()
    {
        return Solve(false);
    }

    public override string SolveB()
    {
        return Solve(true);
    }

    private static string Solve(bool isPartB)
    {
        var registerA = isPartB ? 1 : 0;
        var registerB = 0;

        for (var i = 0; i < _instructions.Count; i++)
        {
            var instruction = _instructions[i].Instruction;
            var register = _instructions[i].Register;
            var offset = _instructions[i].Offset;

            switch (instruction)
            {
                case InstructionType.Half:
                    if (register == RegisterType.A)
                        registerA /= 2;
                    else
                        registerB /= 2;
                    break;

                case InstructionType.Triple:
                    if (register == RegisterType.A)
                        registerA *= 3;
                    else
                        registerB *= 3;
                    break;

                case InstructionType.Increment:
                    if (register == RegisterType.A)
                        registerA++;
                    else
                        registerB++;
                    break;

                case InstructionType.Jump:
                    i += offset!.Value - 1;
                    break;

                case InstructionType.JumpEven:
                    if ((register == RegisterType.A && registerA % 2 == 0) || (register == RegisterType.B && registerB % 2 == 0))
                        i += offset!.Value - 1;
                    break;

                case InstructionType.JumpOne:
                    if ((register == RegisterType.A && registerA == 1) || (register == RegisterType.B && registerB == 1))
                        i += offset!.Value - 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return $"Register A: {registerA}, register B: {registerB}";
    }

    private IEnumerable<Computer> Parse()
    {
        foreach (var line in SplitInput)
        {
            var regex = Regex().Match(line);

            var instruction = regex.Groups[1].Value;
            var register = "a".Equals(regex.Groups[2].Value) ? RegisterType.A : RegisterType.B;

            switch (instruction)
            {
                case "hlf":
                    yield return new Computer(InstructionType.Half, register);
                    break;

                case "tpl":
                    yield return new Computer(InstructionType.Triple, register);
                    break;

                case "inc":
                    yield return new Computer(InstructionType.Increment, register);
                    break;

                case "jie":
                    var offsetJie = int.Parse(regex.Groups[4].Value);
                    yield return new Computer(InstructionType.JumpEven, register, offsetJie);
                    break;

                case "jio":
                    var offsetJio = int.Parse(regex.Groups[4].Value);
                    yield return new Computer(InstructionType.JumpOne, register, offsetJio);
                    break;

                default:
                {
                    if (regex.Groups[0].Value.StartsWith("jmp"))
                    {
                        var offsetJmp = int.Parse(regex.Groups[5].Value);
                        yield return new Computer(InstructionType.Jump, register, offsetJmp);
                    }
                    else
                        throw new ArgumentOutOfRangeException();

                    break;
                }
            }
        }
    }

    private enum InstructionType
    {
        Half,
        Triple,
        Increment,
        Jump,
        JumpEven,
        JumpOne
    }

    private enum RegisterType
    {
        A,
        B
    }

    private record struct Computer(InstructionType Instruction, RegisterType Register, int? Offset = null);

    [GeneratedRegex(@"(hlf|tpl|inc|jie|jio) ([ab])(, ([+-]\d+))?|jmp ([+-]\d+)")]
    private static partial Regex Regex();
}
