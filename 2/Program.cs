
using System.Text.RegularExpressions;

static IEnumerable<long> CreateRange(long start, long count)
{
    var limit = start + count;
    while (start < limit)
    {
        yield return start;
        start++;
    }
}

using var reader = new StreamReader("input.txt");
var regex = "^(\\d+)\\1{1,}$";
var matches = new List<long>();

var line = reader.ReadLine();
while (line != null)
{
    var ranges = line.Split(',');
    foreach (var raw in ranges)
    {
        if (raw.IsWhiteSpace()) continue;

        var parts = raw.Split("-");
        var start = long.Parse(parts[0]);
        var end = long.Parse(parts[1]);
        var matching = CreateRange(start, end - start + 1)
            .Where(n => Regex.IsMatch(n.ToString(), regex, RegexOptions.IgnoreCase));
        matches.AddRange(matching);
    }
    line = reader.ReadLine();
}

Console.WriteLine(matches.Sum());
