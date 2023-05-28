namespace AdventOfCode.Utils;

public static class ArrayExtensions
{
    public static IEnumerable<T[]> Permutations<T>(this T[] values)
    {
        return Permutations(values, 0).Select(v => (T[])v.Clone());
    }

    private static IEnumerable<T[]> Permutations<T>(this T[] values, int fromInd)
    {
        if (fromInd + 1 == values.Length)
            yield return values;
        else
        {
            foreach (var v in Permutations(values, fromInd + 1))
                yield return v;

            for (var i = fromInd + 1; i < values.Length; i++)
            {
                SwapValues(values, fromInd, i);

                foreach (var v in Permutations(values, fromInd + 1))
                    yield return v;

                SwapValues(values, fromInd, i);
            }
        }
    }

    private static void SwapValues<T>(IList<T> values, int pos1, int pos2)
    {
        if (pos1 != pos2)
            (values[pos1], values[pos2]) = (values[pos2], values[pos1]);
    }
}
