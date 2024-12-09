namespace AdventOfCode.Y2024.Day09;

using Tool;
using FileId = int?;

public class Y2024Day09 : Solver
{
    public object PartOne(List<string> input)
    {
        var blocks = ParseSingleBlocks(input);
        CompactBlocks(blocks);
        return CalculateChecksum(blocks);
    }

    public object PartTwo(List<string> input)
    {
        var chunks = ParseChunks(input);
        CompactChunks(chunks);
        return CalculateChecksum(chunks.SelectMany(ChunkToFileIds));
    }

    private static long CalculateChecksum(IEnumerable<FileId> input) => input.Select((it, i) => (it ?? 0) * i * 1L).Sum();

    private static IEnumerable<FileId> ChunkToFileIds(Chunk input) => Enumerable.Repeat(input.Id, input.Size);

    private static void CompactBlocks(List<FileId> blocks)
    {
        var left = 0;
        var right = blocks.Count - 1;
        while (left < right)
        {
            if (blocks[left] is not null)
            {
                left++;
            }
            else
            {
                (blocks[left], blocks[right]) = (blocks[right], blocks[left]);
                right--;
            }
        }
    }

    private static void CompactChunks(List<Chunk> chunks)
    {
        var fileIdsDescending = chunks.Where(IsFile).Select(file => file.Id!.Value).Reverse().ToList();

        foreach (var fileIndex in fileIdsDescending.Select(fileId => chunks.FindLastIndex(chunk => chunk.Id == fileId)))
        {
            TryMoveToFirstFreeSpace(chunks, fileIndex);
        }
    }

    private static void Dump(IEnumerable<FileId> fileIds) => Console.WriteLine(string.Join("", fileIds.Select(it => it is not null ? it.ToString() : ".")));

    private static bool IsEmptySpace(Chunk input) => input.Id is null;

    private static bool IsFile(Chunk input) => input.Id is not null;

    private static List<Chunk> ParseChunks(List<string> input) =>
        input[0]
            .Select(@char => @char - '0')
            .Select(
                (blockSize, i) =>
                {
                    var id = i / 2;
                    var even = i % 2 == 0;
                    return new Chunk(even ? id : null, blockSize);
                })
            .ToList();

    private static List<FileId> ParseSingleBlocks(List<string> input) =>
        ParseChunks(input).SelectMany(ChunkToFileIds).ToList();

    private static void TryMoveToFirstFreeSpace(List<Chunk> chunks, int fileIndex)
    {
        var file = chunks[fileIndex];
        for (var freeIndex = 0; freeIndex < fileIndex; freeIndex++)
        {
            var free = chunks[freeIndex];
            if (!IsEmptySpace(free) || file.Size > free.Size)
            {
                continue;
            }

            chunks[freeIndex] = file;
            chunks[fileIndex] = file with { Id = null };

            if (file.Size < free.Size)
            {
                // re-add a smaller empty chunk
                chunks.Insert(freeIndex + 1, free with { Size = free.Size - file.Size });
            }

            return;
        }
    }

    private sealed record Chunk(FileId Id, int Size);
}
