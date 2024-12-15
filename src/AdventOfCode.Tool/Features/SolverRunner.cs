namespace AdventOfCode.Tool.Features;

using ErrorOr;
using Infrastructure;

public class SolverRunner(IProblemLoader problemLoader) : ISolverRunner
{
    public async IAsyncEnumerable<ErrorOr<SolutionResult>> Run(int? year = null, int? day = null, Action<string>? report = null)
    {
        await foreach (var problem in problemLoader.Load(year, day))
        {
            yield return problem.Then(solvableProblem => RunProblem(solvableProblem, report));
        }
    }

    private static ResultType GetResultType(string? answer, string? expectedAnswer) =>
        // Running an AoCProblem can result in 3 results:
        expectedAnswer is null ? ResultType.Pending : // Pending if there is no expected answer
        answer == expectedAnswer ? ResultType.Success : // Success if the answer is correct
        ResultType.Failure; // Failure if the answer is incorrect

    private static PartResult RunPart(Func<List<string>, object> solution, List<string> problemInput, string? expectedAnswer)
    {
        try
        {
            var answer = solution(problemInput).ToString();
            return new(GetResultType(answer, expectedAnswer), answer, expectedAnswer);
        }
        catch (Exception ex)
        {
            return new(ResultType.Failure, $"Error: {ex.Message}", expectedAnswer);
        }
    }

    private static SolutionResult RunProblem(SolvableProblem problem, Action<string>? report = null)
    {
        report?.Invoke($"Running {problem.Key}");
        var problemData = problem.Data;
        var problemInput = problemData.Input.Lines().ToList();
        var resultPart01 = RunPart(problem.Solver.PartOne, problemInput, problemData.Answers.FirstOrDefault());
        var resultPart02 = RunPart(problem.Solver.PartTwo, problemInput, problemData.Answers.Skip(1).FirstOrDefault());
        return new(problem, resultPart01, resultPart02);
    }
}
