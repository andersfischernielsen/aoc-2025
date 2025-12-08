using QuikGraph;
using QuikGraph.Algorithms;

static double EuclideanDistance((long, long, long) first, (long, long, long) second)
{
    var dx = first.Item1 - second.Item1;
    var dy = first.Item2 - second.Item2;
    var dz = first.Item3 - second.Item3;
    return Math.Sqrt(dx * dx + dy * dy + dz * dz);
}

static long PartOne(string input, long toTake = 1000)
{
    var lines = File.ReadAllLines(input);
    var circuits = new Dictionary<(long, long, long), HashSet<(long, long, long)>>();

    var coordinates = lines
        .Select(l =>
        {
            var parts = l.Split(",");
            return (long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
        })
        .ToList();

    var distances = new List<((long, long, long) first, (long, long, long) second, double distance)>();

    for (int i = 0; i < coordinates.Count; i++)
    {
        for (int j = i + 1; j < coordinates.Count; j++)
        {
            var dist = EuclideanDistance(coordinates[i], coordinates[j]);
            distances.Add((coordinates[i], coordinates[j], dist));
        }
    }

    foreach (var coord in coordinates)
    {
        circuits[coord] = [coord];
    }

    var orderedDistances = distances
        .OrderBy(d => d.distance)
        .ThenBy(d => d.first.Item1).ThenBy(d => d.first.Item2).ThenBy(d => d.first.Item3)
        .ThenBy(d => d.second.Item1).ThenBy(d => d.second.Item2).ThenBy(d => d.second.Item3);

    var usedLinks = 0;

    foreach (var (first, second, distance) in orderedDistances)
    {
        if (usedLinks == toTake)
        {
            break;
        }

        usedLinks++;

        var mergedCircuit = new HashSet<(long, long, long)>(circuits[first]);
        mergedCircuit.UnionWith(circuits[second]);
        foreach (var coord in mergedCircuit)
        {
            circuits[coord] = mergedCircuit;
        }
    }

    var orderedCircuits = circuits.Values
        .Distinct()
        .OrderByDescending(c => c.Count)
        .ToList();

    var circuitSizes = orderedCircuits
        .Take(3)
        .Select(c => c.Count)
        .ToList();

    var result = circuitSizes[0] * circuitSizes[1] * circuitSizes[2];
    return result;
}


static long PartTwo(string input)
{
    var lines = File.ReadAllLines(input);
    var coordinates = lines
        .Select(l =>
        {
            var parts = l.Split(",");
            return (long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
        })
        .ToList();

    var graph = new UndirectedGraph<(long, long, long), TaggedEdge<(long, long, long), double>>();
    graph.AddVertexRange(coordinates);

    for (int i = 0; i < coordinates.Count; i++)
    {
        for (int j = i + 1; j < coordinates.Count; j++)
        {
            var dist = EuclideanDistance(coordinates[i], coordinates[j]);
            graph.AddEdge(new TaggedEdge<(long, long, long), double>(coordinates[i], coordinates[j], dist));
        }
    }

    var spanningTree = graph.MinimumSpanningTreeKruskal(e => e.Tag).ToList();
    var lastEdge = spanningTree.OrderByDescending(e => e.Tag).First();

    return lastEdge.Source.Item1 * lastEdge.Target.Item1;
}

Console.WriteLine(PartOne("input.txt", 1000));
Console.WriteLine(PartTwo("input.txt"));
