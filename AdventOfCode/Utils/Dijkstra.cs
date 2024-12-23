namespace AdventOfCode.Utils;

public class Dijkstra<TNode> where TNode : notnull
{
    private readonly HashSet<TNode> _visited = [];

    private readonly Dictionary<TNode, TNode> _predecessor = []; // Used for single best path
    private readonly Dictionary<TNode, List<TNode>> _predecessors = []; // Used for all best paths
    
    private readonly Dictionary<TNode, int> _distances = [];

    public Func<TNode, IEnumerable<TNode>> GetNeighbours { get; set; } = _ => [];
    public Func<TNode, TNode, int> GetDistance { get; set; } = (_, _) => 1;
    public Func<TNode, bool> EndReached { get; set; } = _ => true;

    // Debugging
    public Action<HashSet<TNode>> Draw { get; set; } = _ => { Console.WriteLine("Did you forgot to implement?!"); };

    public (List<TNode> Path, int Distance) ShortestPath(TNode start)
    {
        // Clear list, in case we run this method multiple times
        _predecessor.Clear();
        _visited.Clear();
        _distances.Clear();

        _distances[start] = 0;

        var queue = new PriorityQueue<TNode, int>();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out _))
        {
            if (EndReached(current))
                return (ReconstructPath(_predecessor, current), _distances[current]);

            _visited.Add(current);

            // Debugging
            // Draw(_visited);

            foreach (var neighbour in GetNeighbours(current))
            {
                if (_visited.Contains(neighbour))
                    continue;

                var tmpDistance = _distances[current] + GetDistance(current, neighbour);

                if (tmpDistance >= _distances.GetValueOrDefault(neighbour, int.MaxValue)) continue;

                _predecessor[neighbour] = current;
                _distances[neighbour] = tmpDistance;
                queue.Enqueue(neighbour, tmpDistance);
            }
        }

        return ([], -1);
    }

    public (List<List<TNode>> Path, int Distance) ShortestPaths(TNode start, bool includeEnd = false)
    {
        // Clear list, in case we run this method multiple times
        _predecessors.Clear();
        _visited.Clear();
        _distances.Clear();

        _distances[start] = 0;

        TNode? endNode = default;
        var endReached = false;

        var queue = new PriorityQueue<TNode, int>();
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var current, out _))
        {
            if (!endReached && EndReached(current))
            {
                endNode = current;
                endReached = true;
            }

            _visited.Add(current);

            // Debugging
            // Draw(_visited);

            foreach (var neighbour in GetNeighbours(current))
            {
                if (_visited.Contains(neighbour))
                    continue;

                var tmpDistance = _distances[current] + GetDistance(current, neighbour);

                if (tmpDistance < _distances.GetValueOrDefault(neighbour, int.MaxValue))
                {
                    if (_predecessors.TryGetValue(neighbour, out var cameFromNeighbour))
                    {
                        cameFromNeighbour.Clear();
                        cameFromNeighbour.Add(current);
                    }
                    else
                        _predecessors.Add(neighbour, [current]);

                    _distances[neighbour] = tmpDistance;
                    queue.Enqueue(neighbour, tmpDistance);
                }
                else if (tmpDistance == _distances.GetValueOrDefault(neighbour, int.MaxValue))
                {
                    _predecessors[neighbour].Add(current);
                }
            }
        }

        return endNode == null
            ? ([], -1)
            : (ReconstructPaths(_predecessors, endNode, includeEnd), _distances[endNode]);
    }

    private static List<TNode> ReconstructPath(Dictionary<TNode, TNode> cameFrom, TNode node)
    {
        var path = new List<TNode>();

        while (cameFrom.TryGetValue(node, out node!))
            path.Add(node);

        path.Reverse();
        return path;
    }

    private static List<List<TNode>> ReconstructPaths(Dictionary<TNode, List<TNode>> cameFrom, TNode endNode, bool includeEnd)
    {
        var allPaths = new List<List<TNode>>();

        var queue = new Queue<(TNode Node, List<TNode> Path)>();
        queue.Enqueue((endNode, []));

        while (queue.Count > 0)
        {
            var (end, path) = queue.Dequeue();

            if (!cameFrom.TryGetValue(end, out var nodes))
            {
                path.Add(end);
                path.Reverse();

                allPaths.Add(includeEnd ? path : path[..^1]);
                continue;
            }

            foreach (var node in nodes)
                queue.Enqueue((node, [.. path, end]));
        }

        return allPaths;
    }
}
