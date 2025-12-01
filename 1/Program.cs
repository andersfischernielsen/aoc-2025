var dial = 50;
var occurrences = 0;

var wrapped = (int current, int difference, int range) =>
{
    if (difference == 0) return (current, 0);

    var raw = current + difference;
    var next = ((raw % range) + range) % range;
    var steps = Math.Abs(difference);
    var distance = difference > 0
        ? (range - current) % range
        : current % range;

    if (distance == 0) distance = range;
    var zeros = steps >= distance
        ? 1 + (steps - distance) / range
        : 0;

    return (next, zeros);
};

var wrapped100 = (int current, int difference) => wrapped(current, difference, 100);

var reader = new StreamReader("input.txt");
var line = reader.ReadLine();
while (line != null)
{
    var direction = line[0];
    var amount = int.Parse(line[1..]);
    var (next, zeroHits) = direction switch
    {
        'R' => wrapped100(dial, amount),
        'L' => wrapped100(dial, -amount),
        _ => (dial, 0)
    };

    dial = next;
    occurrences += zeroHits;

    line = reader.ReadLine();
}
reader.Close();

Console.WriteLine(occurrences);
