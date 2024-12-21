namespace AdventOfCode.Utils;

public class Dijkstra<TNode> where TNode : notnull
{
    private readonly HashSet<TNode> _visited = [];
    private readonly Dictionary<TNode, int> _distances = [];

    public Func<TNode, IEnumerable<TNode>> GetNeighbours { get; set; } = _ => [];
    public Func<TNode, TNode, int> GetDistance { get; set; } = (_, _) => 1;
    public Func<TNode, bool> EndReached { get; set; } = _ => true;

    // Debugging
    public Action<HashSet<TNode>> Draw { get; set; } = _ => { Console.WriteLine("Did you forgot to implement?!"); };

    public int ShortestPath(TNode start)
    {
        // Clear list, in case we run this method multiple times
        _visited.Clear();
        _distances.Clear();
        
        _distances[start] = 0;

        var queue = new PriorityQueue<TNode, int>();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out _))
        {
            if (EndReached(current))
                return _distances[current];

            _visited.Add(current);

            // Debugging
            // Draw(_visited);

            foreach (var neighbour in GetNeighbours(current))
            {
                if (_visited.Contains(neighbour))
                    continue;

                var tmpDistance = _distances[current] + GetDistance(current, neighbour);

                if (tmpDistance >= _distances.GetValueOrDefault(neighbour, int.MaxValue)) continue;

                _distances[neighbour] = tmpDistance;
                queue.Enqueue(neighbour, tmpDistance);
            }
        }

        return -1;
    }
}
