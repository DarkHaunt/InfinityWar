using System;

internal static class StaticData
{
    public static readonly Random Randomizer = new Random();

    public static readonly int[] Sign = new int[2]
    {
        1,
        -1
    };


    public static int GetRandomSign() => Sign[Randomizer.Next(0, Sign.Length)];
}
