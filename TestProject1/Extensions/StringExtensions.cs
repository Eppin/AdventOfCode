namespace TestProject1.Extensions;

public static class StringExtensions
{
    public static string[] GetSplit(this string str, bool removeEmptyEntries = true)
    {
        var splitOptions = StringSplitOptions.TrimEntries;
        if (removeEmptyEntries) splitOptions |= StringSplitOptions.RemoveEmptyEntries;

        return str.Split(Environment.NewLine, splitOptions);
    }
}
