using System.Text.RegularExpressions;

static long PartOne(string filename)
{
    var numberRegex = DigitRegex();
    var operatorRegex = OperatorRegex();

    using var reader = new StreamReader(filename);
    var line = reader.ReadLine();
    var numbers = new List<List<long>>();
    var results = new List<long>();
    while (line != null)
    {
        var numberMatches = numberRegex.Matches(line);
        for (var i = 0; i < numberMatches.Count; i++)
        {
            if (numbers.ElementAtOrDefault(i) == null) numbers.Add([]);
            numbers[i].Add(long.Parse(numberMatches[i].Value));
        }

        var operatorMatches = operatorRegex.Matches(line);
        for (var i = 0; i < operatorMatches.Count; i++)
        {
            if (operatorMatches[i].Value == "+")
            {
                results.Add(numbers[i].Sum());
            }
            else if (operatorMatches[i].Value == "*")
            {
                results.Add(numbers[i].Aggregate(1L, (a, b) => a * b));
            }
        }

        line = reader.ReadLine();
    }

    return results.Sum();
}

static long PartTwo(string filename)
{
    var lines = File.ReadAllLines(filename).ToList();
    var operatorLine = lines[^1];
    var length = lines.Count;
    var results = new List<long>();
    var grid = new char[lines.Count][];
    for (var row = 0; row < lines.Count; row++)
    {
        grid[row] = lines[row].ToCharArray();
    }

    var group = new List<long>();
    string? op = null;
    for (var column = 0; column < grid[0].Length; column++)
    {
        var columnString = new string([.. grid.Select(row => row[column])]);
        if (columnString.Trim() == "")
        {
            var result = op == "+" ? group.Sum() : group.Aggregate(1L, (a, b) => a * b);
            results.Add(result);
            group = [];
            continue;
        }

        var lastCharacter = columnString[^1];
        if (lastCharacter == '*' || lastCharacter == '+')
        {
            op = lastCharacter.ToString();
            group.Add(long.Parse(columnString[..^1].Trim()));
        }
        else
        {
            group.Add(long.Parse(columnString.Trim()));
        }
    }

    var lastResult = op == "+" ? group.Sum() : group.Aggregate(1L, (a, b) => a * b);
    results.Add(lastResult);

    return results.Sum();
}

Console.WriteLine(PartOne("input.txt"));
Console.WriteLine(PartTwo("input.txt"));

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex DigitRegex();

    [GeneratedRegex(@"\+|\*")]
    private static partial Regex OperatorRegex();
}
