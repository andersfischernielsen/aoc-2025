static string findLargest(string line, int toSelect)
{
    var stack = new Stack<char>();
    var skipCount = 0;
    for (int i = 0; i < line.Length; i++)
    {
        while (stack.Count > 0 && skipCount < line.Length - toSelect && stack.Peek() < line[i])
        {
            stack.Pop();
            skipCount++;
        }
        stack.Push(line[i]);
    }
    return new string([.. stack.Reverse().Take(toSelect)]);
}

// Part 1
using var reader1 = new StreamReader("input.txt");
var batteries = new List<int>();
var line = reader1.ReadLine();
while (line != null)
{
    var found = findLargest(line, 2);
    var parsed = int.Parse(found);
    batteries.Add(parsed);
    line = reader1.ReadLine();
}
Console.WriteLine(batteries.Sum());

// Part 2
using var reader2 = new StreamReader("input.txt");
var batteries2 = new List<long>();
var line2 = reader2.ReadLine();
while (line2 != null)
{
    var result = findLargest(line2, 12);
    var parsed = long.Parse(result);
    batteries2.Add(parsed);
    line2 = reader2.ReadLine();
}
Console.WriteLine(batteries2.Sum());
