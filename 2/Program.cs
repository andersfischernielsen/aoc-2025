
using System.Text.RegularExpressions;

var reader = new StreamReader("input.txt");
var line = reader.ReadLine();
var regex = "^(\\d+)\\1{1,}$";
var matches = new List<long>();

static IEnumerable<long> CreateRange(long start, long count)
{
    var limit = start + count;

    while (start < limit)
    {
        yield return start;
        start++;
    }
}

while (line != null)
{
    var ranges = line.Split(',');
    foreach (var raw in ranges)
    {
        if (raw == "") continue;

        var parts = raw.Split("-");
        var start = long.Parse(parts[0]);
        var end = long.Parse(parts[1]);
        var range = CreateRange(start, end - start + 1);
        foreach (var number in range)
        {
            if (Regex.IsMatch(number.ToString(), regex, RegexOptions.IgnoreCase))
            {
                matches.Add(number);
            }
        }
    }
    line = reader.ReadLine();
}

reader.Close();
Console.WriteLine(matches.Sum());
