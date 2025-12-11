using System.Text.RegularExpressions;

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

static int? BFS<State, Next>(State initial, Func<State, bool> isTarget, Func<State, Next, State> transition, IEnumerable<Next> moves, IEqualityComparer<State>? comparer = null, Func<State, bool>? heurestic = null)
{
    comparer ??= EqualityComparer<State>.Default;

    var queue = new Queue<(State state, int steps)>();
    var visited = new HashSet<State>(comparer);

    queue.Enqueue((initial, 0));
    visited.Add(initial);

    while (queue.Count > 0)
    {
        var (currentState, steps) = queue.Dequeue();

        if (isTarget(currentState))
            return steps;

        foreach (var move in moves)
        {
            var next = transition(currentState, move);

            if (heurestic?.Invoke(next) ?? false || visited.Contains(next))
                continue;

            visited.Add(next);
            queue.Enqueue((next, steps + 1));
        }
    }

    return null;
}

static int[] ApplyButton(int[] voltage, int[] button)
{
    var result = (int[])voltage.Clone();
    foreach (var v in button)
    {
        result[v] += 1;
    }
    return result;
}

static bool Invariant(int[] current, int[] target)
{
    for (int i = 0; i < current.Length; i++)
        if (current[i] > target[i])
            return true;
    return false;
}

// ------

static long PartOne(string input)
{
    var buttonPresses = 0;

    var targetRegex = new Regex(@"\[(.*)\]");
    var buttonRegex = new Regex(@"\(([^)]+)\)");
    var reader = new StreamReader(input);
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        if (line == null) break;

        var targetMatch = targetRegex.Match(line).Groups[1].Value;
        var target = Convert.ToInt64(targetMatch
            .Select(c => c == '#' ? '1' : '0')
            .Aggregate("", (acc, c) => acc + c), 2);
        var buttons = buttonRegex.Matches(line).Select(m =>
        {
            var indices = m.Groups[1].Value.Split(',').Select(int.Parse);
            var mask = indices.Aggregate(new string('0', targetMatch.Length), (m, idx) =>
            {
                var chars = m.ToCharArray();
                chars[idx] = '1';
                return new string(chars);
            });
            return Convert.ToInt64(mask, 2);
        }
        ).ToList();

        var steps = BFS(0L, state => state == target, (current, button) => current ^ button, buttons);
        buttonPresses += steps ?? 0;
    }

    return buttonPresses;
}

static long PartTwo(string input)
{
    return 0;
}

Console.WriteLine(PartOne("input.test.txt"));
Console.WriteLine(PartOne("input.txt"));

Console.WriteLine(PartTwo("input.test.txt"));
Console.WriteLine(PartTwo("input.txt"));

// ------

sealed class VoltageStateComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[]? x, int[]? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Length != y.Length) return false;
        for (int i = 0; i < x.Length; i++)
            if (x[i] != y[i]) return false;
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        unchecked
        {
            int hash = 17;
            foreach (var v in obj)
                hash = hash * 31 + v;
            return hash;
        }
    }
}