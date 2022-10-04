using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This objects can pick a target, which will be main target for it until death or destroy this target
/// </summary>
public interface IGlobalTargetable
{
    HitableEntity GlobalTarget { get; }
}
