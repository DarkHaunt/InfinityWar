using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWarrior
{
    void Attack(FractionEntity target);
    bool IsOnAttackRadius();
}
