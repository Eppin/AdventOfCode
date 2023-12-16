namespace AdventOfCode._2015;

public class Day24 : Day
{
    private readonly HashSet<List<int>> _combinations = [];

    private static List<int> _list = null!;

    public Day24() : base()
    {
        _list = SplitInput
            .Select(int.Parse)
            .ToList();
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private long Solve(bool isPartB)
    {
        var groupCount = isPartB ? 4 : 3;
        var groupSize = _list.Sum() / groupCount;

        FindCombinations(_list, 0, groupSize);

        var size = int.MaxValue;
        var qe = long.MaxValue;

        foreach (var firstGroup in _combinations)
        {
            if (firstGroup.Count == size)
            {
                var newQe = firstGroup.Aggregate(1L, (a, b) => a * b);
                if (newQe <= qe)
                    qe = newQe;
            }
            else if (firstGroup.Count < size)
            {
                size = firstGroup.Count;
                var newQe = firstGroup.Aggregate(1L, (a, b) => a * b);
                qe = newQe;
            }
        }

        return qe;
    }

    private void FindCombinations(IReadOnlyList<int> arr, int index, int reducedNum, List<int>? combination = null)
    {
        if (reducedNum < 0)
            return;

        combination ??= [];

        if (reducedNum == 0)
        {
            _combinations.Add([..combination]);
            return;
        }

        var prev = index == 0 ? 1 : arr[index - 1];

        for (var k = index; k < arr.Count; k++)
        {
            if (arr[k] < prev)
                continue;

            combination.Add(arr[k]);
            FindCombinations(arr, k + 1, reducedNum - arr[k], combination);
            combination.RemoveAt(combination.Count - 1);
        }
    }
}
