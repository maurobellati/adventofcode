namespace AdventOfCode.Y2022.Day07;

using Tool;

public class Y2022Day07 : Solver
{
    public object PartOne(List<string> input) =>
        ParseFileSystem(input)
            .GetAllChildDirectories()
            .Where(d => d.GetSize() < 100_000)
            .Sum(d => d.GetSize());

    public object PartTwo(List<string> input)
    {
        const int MinSpace = 70_000_000 - 30_000_000;

        var root = ParseFileSystem(input);
        var excessDiskSpace = root.GetSize() - MinSpace;

        return root.GetAllChildDirectories()
            .Where(d => d.GetSize() >= excessDiskSpace)
            .MinBy(d => d.GetSize())!
            .GetSize();
    }

    private static Dir ParseFileSystem(List<string> input)
    {
        var root = Dir.Root;
        var currentDir = root;
        foreach (var line in input)
        {
            if (line.StartsWith("$ cd", InvariantCulture))
            {
                var dirName = line.After(" ");
                currentDir = dirName switch
                {
                    ".." => currentDir.Parent,
                    "/" => root,
                    _ => currentDir.GetChildDirectory(dirName)
                };
                continue;
            }

            if (line == "$ ls")
            {
                continue;
            }

            if (line.StartsWith("dir", InvariantCulture))
            {
                var dirName = line.After(" ");
                currentDir.AddDirectory(new(dirName, currentDir));
                continue;
            }

            var fileSize = line.Before(" ").ToInt();
            currentDir.AddFile(fileSize);
        }

        return root;
    }

    public class Dir(string name, Dir parent)
    {
        private readonly List<Dir> dirs = [];
        private int fileSize;

        private string Name { get; } = name;

        public Dir Parent { get; } = parent;

        public static Dir Root => new("root", null!);

        public void AddDirectory(Dir dir) => dirs.Add(dir);

        public void AddFile(int size) => fileSize += size;

        public IEnumerable<Dir> GetAllChildDirectories() => dirs.Union(dirs.SelectMany(dir => dir.GetAllChildDirectories()));

        public Dir GetChildDirectory(string name) => dirs.First(dir => dir.Name == name);

        public int GetSize() => fileSize + dirs.Sum(d => d.GetSize());
    }
}
