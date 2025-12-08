var lines = File.ReadAllLines("input.txt");
var circuits = new Dictionary<(int, int, int), HashSet<(int, int, int)>>();

var coordinates = lines.Select(l =>
{
    var parts = l.Split(",");
    return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
}).ToList();

Console.WriteLine($"Total coordinates: {coordinates.Count}");

var distances = new List<((int, int, int) first, (int, int, int) second, double distance)>();

for (int i = 0; i < coordinates.Count; i++)
{
    for (int j = i + 1; j < coordinates.Count; j++)
    {
        var dx = coordinates[i].Item1 - coordinates[j].Item1;
        var dy = coordinates[i].Item2 - coordinates[j].Item2;
        var dz = coordinates[i].Item3 - coordinates[j].Item3;
        var dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);
        distances.Add((coordinates[i], coordinates[j], dist));
    }
}

Console.WriteLine($"Total distance pairs: {distances.Count}");

var shortestDistances = distances.OrderBy(d => d.distance).Take(1000).ToList();

foreach (var coord in coordinates)
{
    circuits[coord] = new HashSet<(int, int, int)> { coord };
}

foreach (var (first, second, _) in shortestDistances)
{
    var firstCircuit = circuits[first];
    var secondCircuit = circuits[second];

    var mergedCircuit = new HashSet<(int, int, int)>(firstCircuit);
    mergedCircuit.UnionWith(secondCircuit);

    foreach (var coord in mergedCircuit)
    {
        circuits[coord] = mergedCircuit;
    }
}

// Count unique circuits by reference
var uniqueByRef = circuits.Values.Distinct().Count();
Console.WriteLine($"Unique circuits by reference: {uniqueByRef}");

// Count unique circuits by content (using HashSet comparison)
var uniqueSets = new List<HashSet<(int,int,int)>>();
foreach (var set in circuits.Values)
{
    if (!uniqueSets.Any(s => ReferenceEquals(s, set)))
        uniqueSets.Add(set);
}
Console.WriteLine($"Unique circuits manual count: {uniqueSets.Count}");

var sizes = uniqueSets.Select(s => s.Count).OrderByDescending(x => x).ToList();
Console.WriteLine($"Top 5 sizes: {string.Join(", ", sizes.Take(5))}");
Console.WriteLine($"Sum of all circuit sizes: {sizes.Sum()}");
