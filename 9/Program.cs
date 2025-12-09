static long PartOne(string path)
{
    var lines = File.ReadAllLines(path);
    List<(int x, int y)> coordinates = [
        .. lines
            .Select(l => { var split = l.Split(','); return (int.Parse(split[0]), int.Parse(split[1])); })
            .OrderBy(c => c.Item1)
            .ThenBy(c => c.Item2)
        ];

    long largestArea = 0;
    for (var i = 0; i < coordinates.Count; i++)
    {
        for (var j = i + 1; j < coordinates.Count; j++)
        {
            var rect = new Rectangle(coordinates[i], coordinates[j]);
            if (rect.Area > largestArea)
                largestArea = rect.Area;
        }
    }

    return largestArea;
}

static List<Edge> BuildEdges(List<(int x, int y)> red)
{
    var edges = new List<Edge>();
    for (var i = 0; i < red.Count; i++)
    {
        var from = red[i];
        var to = red[(i + 1) % red.Count];
        edges.Add(new Edge(from, to));
    }

    return edges;
}

static bool IsOnSegment(int x, int y, Edge edge)
{
    var (x1, y1) = edge.From;
    var (x2, y2) = edge.To;

    if (x < Math.Min(x1, x2) || x > Math.Max(x1, x2))
        return false;
    if (y < Math.Min(y1, y2) || y > Math.Max(y1, y2))
        return false;

    var isVertical = x1 == x2;
    if (isVertical)
        return x == x1;
    if (y1 == y2)
        return y == y1;

    return false;
}

static bool IsInside(int x, int y, List<Edge> edges)
{
    var crossings = 0;
    foreach (var edge in edges)
    {
        var (x1, y1) = edge.From;
        var (x2, y2) = edge.To;

        if (IsOnSegment(x, y, edge))
            return true;

        var isHorizontal = y1 == y2;
        if (isHorizontal)
            continue;

        if (y1 > y2)
        {
            (x1, x2) = (x2, x1);
            (y1, y2) = (y2, y1);
        }

        if (y >= y1 && y < y2)
        {
            var xIntersect = x1 + (double)(x2 - x1) * (y - y1) / (y2 - y1);
            if (xIntersect > x)
                crossings++;
        }
    }

    return crossings % 2 == 1;
}

static bool IsRectangleInside(Rectangle rect, List<Edge> edges)
{
    if (!IsInside(rect.MinX, rect.MinY, edges)) return false;
    if (!IsInside(rect.MaxX, rect.MinY, edges)) return false;
    if (!IsInside(rect.MinX, rect.MaxY, edges)) return false;
    if (!IsInside(rect.MaxX, rect.MaxY, edges)) return false;

    return !edges.Any(e => CrossesInterior(e, rect));
}

static bool CrossesInterior(Edge edge, Rectangle rect)
{
    var (x1, y1) = edge.From;
    var (x2, y2) = edge.To;

    var isVertical = x1 == x2;
    if (isVertical)
    {
        var minY = Math.Min(y1, y2);
        var maxY = Math.Max(y1, y2);
        return x1 > rect.MinX && x1 < rect.MaxX && maxY > rect.MinY && minY < rect.MaxY;
    }

    var minX = Math.Min(x1, x2);
    var maxX = Math.Max(x1, x2);
    return y1 > rect.MinY && y1 < rect.MaxY && maxX > rect.MinX && minX < rect.MaxX;
}

static long PartTwo(string path)
{
    var lines = File.ReadAllLines(path);
    List<(int x, int y)> red = [.. lines.Select(l => { var split = l.Split(','); return (int.Parse(split[0]), int.Parse(split[1])); })];
    var edges = BuildEdges(red);

    long largestArea = 0;
    for (int i = 0; i < red.Count; i++)
    {
        for (int j = i + 1; j < red.Count; j++)
        {
            var rectangle = new Rectangle(red[i], red[j]);
            if (IsRectangleInside(rectangle, edges) && rectangle.Area > largestArea)
                largestArea = rectangle.Area;
        }
    }

    return largestArea;
}

Console.WriteLine(PartOne("input.test.txt"));
Console.WriteLine(PartOne("input.txt"));
Console.WriteLine(PartTwo("input.test.txt"));
Console.WriteLine(PartTwo("input.txt"));

record Edge((int x, int y) From, (int x, int y) To);

record Rectangle(int MinX, int MaxX, int MinY, int MaxY)
{
    public Rectangle((int x, int y) a, (int x, int y) b)
        : this(Math.Min(a.x, b.x), Math.Max(a.x, b.x), Math.Min(a.y, b.y), Math.Max(a.y, b.y)) { }

    public long Area => (long)(MaxX - MinX + 1) * (MaxY - MinY + 1);
}
