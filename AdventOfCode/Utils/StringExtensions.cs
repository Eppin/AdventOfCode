namespace AdventOfCode.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Replace part of string
    /// </summary>
    /// <param name="str">Source string</param>
    /// <param name="index">Starting index</param>
    /// <param name="length">Length of characters to be removed</param>
    /// <param name="replace">String that is replacing the removed characters</param>
    /// <returns></returns>
    public static string ReplaceAt(this string str, int index, int length, string replace)
    {
        return str
            .Remove(index, Math.Min(length, str.Length - index))
            .Insert(index, replace);
    }
}
