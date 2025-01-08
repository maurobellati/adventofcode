namespace AdventOfCode.Tests.Y2024.Day22;

using AdventOfCode.Y2024.Day22;

public class Y2024Day22Test : SolverTest<Y2024Day22>
{
    [Fact]
    public void NextTest()
    {
        Enumerable.Range(0, 10).Last().Should().Be(9);
        // Y2024Day22.Next(123, 10).Last().Should().Be(5908254);

        Y2024Day22.Next(123, 10).Should().BeEquivalentTo(
        [
            15887950,
            16495136,
            527345,
            704524,
            1553684,
            12683156,
            11100544,
            12249484,
            7753432,
            5908254
        ]);
    }

    protected override IEnumerable<TestCase> PartOnes() => [new("""
                                                                1
                                                                10
                                                                100
                                                                2024
                                                                """, 37327623)];

    protected override IEnumerable<TestCase> PartTwos() => [new("""
                                                                1
                                                                2
                                                                3
                                                                2024
                                                                """, 23)];
}
