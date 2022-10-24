using System;

/// <summary>
/// Sub class for making math operations in project less complicated
/// </summary>
public static class StaticRandomizer
{
    public static readonly Random Randomizer = new Random();

    public static int GetRandomSign() => (Randomizer.Next(0, 1) * 2) - 1;
}
