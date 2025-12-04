using System.Diagnostics;

static int? Adjacent((int, int) index, char[][] floor)
{
    int get(int row, int col, char[][] floor) => row < floor.Length && row >= 0 && col < floor[row].Length && col >= 0
        ? floor[row][col]
        : '\0';
    bool isAt(int row, int col) => get(row, col, floor) == '@';

    var (row, col) = index;
    var count = 0;
    if (floor[row][col] != '@') return null;

    if (isAt(row - 1, col)) count++;
    if (isAt(row + 1, col)) count++;
    if (isAt(row, col - 1)) count++;
    if (isAt(row, col + 1)) count++;
    if (isAt(row + 1, col + 1)) count++;
    if (isAt(row + 1, col - 1)) count++;
    if (isAt(row - 1, col + 1)) count++;
    if (isAt(row - 1, col - 1)) count++;

    return count;
}

static int BruteForce(string filename)
{
    using var reader = new StreamReader(filename);
    var line = reader.ReadLine();
    char[][] floor = null!;
    int rowNum = 0;
    while (line != null)
    {
        if (floor == null)
        {
            floor = new char[line.Length][];
            for (int i = 0; i < line.Length; i++)
            {
                floor[i] = new char[line.Length];
            }
        }

        for (int col = 0; col < line.Length; col++)
        {
            floor[rowNum][col] = line[col];
        }
        rowNum++;
        line = reader.ReadLine();
    }

    int totalRemoved = 0;
    int removedThisRound;

    do
    {
        removedThisRound = 0;

        int?[][] map = new int?[floor.Length][];
        for (int i = 0; i < floor.Length; i++)
        {
            map[i] = new int?[floor[i].Length];
        }

        for (var row = 0; row < floor.Length; row++)
        {
            for (var col = 0; col < floor[row].Length; col++)
            {
                map[row][col] = Adjacent((row, col), floor);
            }
        }

        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] != null && map[row][col] < 4)
                {
                    floor[row][col] = '.';
                    removedThisRound++;
                }
            }
        }

        totalRemoved += removedThisRound;
    } while (removedThisRound > 0);

    return totalRemoved;
}

static int BFS(string filename)
{
    using var reader = new StreamReader(filename);
    var lines = new List<string>();
    var l = reader.ReadLine();
    while (l != null)
    {
        lines.Add(l);
        l = reader.ReadLine();
    }

    char[][] grid = new char[lines.Count][];
    int[][] adjacency = new int[lines.Count][];
    for (int i = 0; i < lines.Count; i++)
    {
        grid[i] = lines[i].ToCharArray();
        adjacency[i] = new int[grid[i].Length];
    }

    int[] dr = [-1, -1, -1, 0, 0, 1, 1, 1];
    int[] dc = [-1, 0, 1, -1, 1, -1, 0, 1];

    bool isValid(int r, int c) => r >= 0 && r < grid.Length && c >= 0 && c < grid[r].Length;

    var queue = new Queue<(int row, int col)>();
    for (int r = 0; r < grid.Length; r++)
    {
        for (int c = 0; c < grid[r].Length; c++)
        {
            var adj = Adjacent((r, c), grid);
            if (adj == null) continue;

            adjacency[r][c] = adj.Value;

            if (adj < 4)
            {
                queue.Enqueue((r, c));
            }
        }
    }

    int removed = 0;
    while (queue.Count > 0)
    {
        var (r, c) = queue.Dequeue();
        if (grid[r][c] != '@') continue;
        grid[r][c] = '.';
        removed++;
        for (int d = 0; d < 8; d++)
        {
            int nr = r + dr[d];
            int nc = c + dc[d];

            if (isValid(nr, nc) && grid[nr][nc] == '@')
            {
                adjacency[nr][nc]--;
                if (adjacency[nr][nc] < 4)
                {
                    queue.Enqueue((nr, nc));
                }
            }
        }
    }

    return removed;
}

const string filename = "input.txt";

var stopwatch = Stopwatch.StartNew();
int bruteForce = BruteForce(filename);
stopwatch.Stop();
Console.WriteLine($"Brute force ({stopwatch.ElapsedMilliseconds}ms):");
Console.WriteLine(bruteForce);

stopwatch = Stopwatch.StartNew();
int bfs = BFS(filename);
stopwatch.Stop();
Console.WriteLine($"BFS ({stopwatch.ElapsedMilliseconds}ms):");
Console.WriteLine(bfs);
