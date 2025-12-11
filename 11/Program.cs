using System.Text.RegularExpressions;
using QuikGraph;
using QuikGraph.Algorithms.RankedShortestPath;

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

// ------

static long PartOne(string input)
{
    var regex = new Regex(@"(\w+): (.+)");
    var reader = new StreamReader(input);
    var graph = new BidirectionalGraph<string, Edge<string>>();
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        if (line == null) break;

        var matches = regex.Match(line);
        var node = matches.Groups[1].Value;
        var destinations = matches.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var dest in destinations)
        {
            graph.AddVerticesAndEdge(new Edge<string>(node, dest));
        }
    }

    var ranked = new HoffmanPavleyRankedShortestPathAlgorithm<string, Edge<string>>(graph, e => 1.0)
    {
        ShortestPathCount = 10000
    };
    ranked.Compute("you", "out");

    return ranked.ComputedShortestPathCount;
}

static long PartTwo(string input)
{
    return 0;
}


Console.WriteLine(PartOne("input.test.txt"));
Console.WriteLine(PartOne("input.txt"));

Console.WriteLine(PartTwo("input.test2.txt"));
Console.WriteLine(PartTwo("input.txt"));
