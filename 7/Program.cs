static long PartOne(string path)
{
    using var reader = new StreamReader(path);
    var start = reader.ReadLine()?.IndexOf('S') ?? 0;

    HashSet<int> beams = [start];
    var count = 0;

    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        var print = line.ToCharArray();
        var newBeams = new HashSet<int>();

        foreach (var beam in beams.ToArray())
        {
            if (line[beam] == '^')
            {
                count++;
                newBeams.Add(beam - 1);
                newBeams.Add(beam + 1);
                beams.Remove(beam);
            }
        }

        beams.UnionWith(newBeams);

        foreach (var beam in beams)
            print[beam] = '|';

        Console.WriteLine(string.Join("", print));
    }

    return count;
}

static long PartTwo(string path)
{
    static void Add(Dictionary<int, long> dict, int key, long delta)
    {
        if (dict.TryGetValue(key, out var cur))
            dict[key] = checked(cur + delta);
        else
            dict[key] = delta;
    }

    using var reader = new StreamReader(path);
    var start = reader.ReadLine()?.IndexOf('S') ?? 0;
    var beams = new Dictionary<int, long> { [start] = 1 };

    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        var next = new Dictionary<int, long>(beams);
        foreach (var (col, count) in beams)
        {
            if (line[col] == '^')
            {
                next.Remove(col);

                Add(next, col - 1, count);
                Add(next, col + 1, count);
            }
        }
        beams = next;
    }

    long total = 0;
    checked
    {
        foreach (var v in beams.Values)
            total += v;
    }
    return total;
}

var input = "input.txt";
Console.WriteLine(PartOne(input));
Console.WriteLine(PartTwo(input));
