static int PartOne(string filename)
{
    using var reader = new StreamReader(filename);
    var line = reader.ReadLine();
    var freshIngredients = new List<(long start, long end)>();
    var isReadingRanges = true;
    var found = 0;
    while (line != null)
    {
        if (line == "")
        {
            isReadingRanges = false;
            line = reader.ReadLine();
            continue;
        }

        if (isReadingRanges)
        {
            var parts = line.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);
            freshIngredients.Add((start, end));

        }
        else
        {
            var ingredient = long.Parse(line);
            if (freshIngredients.Any(r => ingredient >= r.start && ingredient <= r.end))
            {
                found++;
            }
        }

        line = reader.ReadLine();
    }

    return found;
}

static long PartTwo(string filename)
{
    using var reader = new StreamReader(filename);
    var line = reader.ReadLine();
    var isReadingRanges = true;
    var smallest = (long?)null;
    var largest = (long)0;
    var freshIngredients = new List<(long start, long end)>();
    while (line != null)
    {
        if (line == "")
        {
            isReadingRanges = false;
            line = reader.ReadLine();
            break;
        }

        if (isReadingRanges)
        {
            var parts = line.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);
            var newStart = start;
            var newEnd = end;

            for (int i = freshIngredients.Count - 1; i >= 0; i--)
            {
                var r = freshIngredients[i];
                if (newStart <= r.end && r.start <= newEnd)
                {
                    newStart = Math.Min(newStart, r.start);
                    newEnd = Math.Max(newEnd, r.end);
                    freshIngredients.RemoveAt(i);
                }
            }

            freshIngredients.Add((newStart, newEnd));

            smallest = smallest is null ? newStart : Math.Min(smallest.Value, newStart);
            if (newEnd > largest) largest = newEnd;

            line = reader.ReadLine();
        }
    }

    return freshIngredients.Select(r => r.end - r.start + 1).Sum();
}

Console.WriteLine(PartOne("input.txt"));
Console.WriteLine(PartTwo("input.txt"));