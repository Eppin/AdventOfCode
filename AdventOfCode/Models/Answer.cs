namespace AdventOfCode.Models;

public class Answer(string value, Input input, string? data) 
{
    public string Value { get; } = value;
    public Input Input { get; } = input;
    public string? Data { get; } = data;

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Input, Data);
    }
}
