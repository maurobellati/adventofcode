namespace AdventOfCode;

public record PathStep(Cell Cell, Direction Direction)
{
    public override string ToString() => $"{Cell} {Direction}";
}
