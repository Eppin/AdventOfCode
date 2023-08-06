namespace AdventOfCode.Utils;

// Source: https://stackoverflow.com/a/40056071
public class Combinations<T>
{
    private List<T> List { get; }
    private int Length => List.Count;
    private readonly List<IEnumerable<T>> _allCombination;

    public Combinations(IEnumerable<T> list)
    {
        List = list.ToList();
        _allCombination = new List<IEnumerable<T>>();
    }

    public IEnumerable<IEnumerable<T>> AllCombinations
    {
        get
        {
            GenerateCombination(default, Enumerable.Empty<T>());

            return _allCombination;
        }
    }

    private void GenerateCombination(int position, IEnumerable<T> previousCombination)
    {
        for (var i = position; i < Length; i++)
        {
            var currentCombination = new List<T>();
            currentCombination.AddRange(previousCombination);
            currentCombination.Add(List.ElementAt(i));

            _allCombination.Add(currentCombination);

            GenerateCombination(i + 1, currentCombination);
        }
    }
}