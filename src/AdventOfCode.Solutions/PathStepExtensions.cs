namespace AdventOfCode;

public static class PathStepExtensions
{
    public static PathStep Move(this PathStep step, int size = 1) =>
        step with { Cell = step.Cell.Move(step.Direction, size) };

    public static PathStep Rotate(this PathStep step, Rotation rotation) =>
        step with { Direction = step.Direction.Rotate(rotation) };

    public static PathStep Turn(this PathStep step, Direction direction) =>
        step with { Direction = direction };
}
