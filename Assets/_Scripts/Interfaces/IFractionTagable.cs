using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Belonging for any fraction
/// </summary>
public interface IFractionTagable
{
    string FractionTag { get; }


    bool IsSameFraction(string fractionTag);
}
