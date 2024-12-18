namespace AdventOfCode.Utils;

using Coordinate = Coordinate<int>;

public class Dijkstra<TNode> where TNode : notnull
{
    private List<TNode> _unvisited;
    private List<TNode> _visited;
    private Dictionary<TNode, int> _distances;

    public Dijkstra(List<TNode> nodes, TNode start)
    {
        _unvisited = nodes;
        _visited = [start];
        _distances = new Dictionary<TNode, int> { { start, 0 } };
    }

    public void ShortestPath(TNode end, Func<TNode, IEnumerable<TNode>> neighbours, Func<TNode, int> distance)
    {
        var queue = new Queue<TNode>();
        queue.Enqueue(_visited[0]);

        while (queue.TryDequeue(out var node))
        {
            foreach (var n in neighbours.Invoke(node))
            {
                distance(n);
            }
        }
    }


}
