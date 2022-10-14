using System;

/// <summary>
/// Sub class for making math operations in project less complicated
/// </summary>
internal static class StaticNumberOperator
{
    public static readonly Random Randomizer = new Random();

    public static readonly int[] Sign = new int[2]
    {
        1,
        -1
    };


    public static int GetRandomSign() => Sign[Randomizer.Next(0, Sign.Length)];
}
