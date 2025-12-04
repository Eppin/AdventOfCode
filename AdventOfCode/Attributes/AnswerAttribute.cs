namespace AdventOfCode.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AnswerAttribute(string value, Input input) : Attribute
{
    public string Answer { get; } = value;
    public Input Input { get; } = input;

    public string? Data { get; set; }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Answer, Input, Data);
    }
}
