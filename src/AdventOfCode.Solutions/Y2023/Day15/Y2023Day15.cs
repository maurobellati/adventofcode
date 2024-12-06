namespace AdventOfCode.Y2023.Day15;

using Tool;
using KeyValue = (string Key, int Value);

public class Y2023Day15 : Solver
{
    public object PartOne(List<string> input) => GetOperations(input).Select(HashMap.Hash).Sum();

    public object PartTwo(List<string> input)
    {
        var hashMap = new HashMap();
        foreach (var operation in GetOperations(input))
        {
            if (operation.EndsWith('-'))
            {
                var key = operation.Strip("-");
                // Console.WriteLine($"remove {key}");
                hashMap.Remove(key);
            }
            else if (operation.ContainsInvariant('='))
            {
                var key = operation.Before('=');
                var value = operation.After('=').ToInt();
                // Console.WriteLine($"put {key} = {value}");
                hashMap.Put(key, value);
            }
        }

        return hashMap.FocusingPower();
    }

    private static string[] GetOperations(List<string> input) => input.First().Split(',');

    private sealed class HashMap
    {
        private readonly List<List<KeyValue>> map = Enumerable.Range(0, 256).Select(_ => new List<KeyValue>()).ToList();

        internal static int Hash(string input) => input.Aggregate(0, (current, c) => (current + c) * 17 % 256);

        public int FocusingPower() => map.SelectMany((values, box) => values.Select((lens, i) => (box + 1) * (i + 1) * lens.Value)).Sum();

        public void Put(string key, int value)
        {
            var values = map[Hash(key)];
            var index = values.FindIndex(kv => kv.Key == key);

            //If there is already a lens in the box with the same label
            if (index >= 0)
            {
                // replace the old lens with the new lens IN PLACE
                values[index] = new(key, value);
                return;
            }

            // otherwise, add the lens to the box immediately behind any lenses already in the box
            values.Add(new(key, value));
        }

        public void Remove(string key) =>
            // remove the lens with the given label if it is present in the box
            map[Hash(key)].RemoveAll(kv => kv.Key == key);
    }
}
