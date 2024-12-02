namespace AdventOfCode.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AnswerAttribute(string value, Input input) : Attribute
{
    public string Answer { get; private set; } = value;
    public Input Input { get; private set; } = input;

    public string? Data { get; set; }
}
