using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWarrior
{
    void Attack(IHitable target);
    bool IsOnAttackRadius();
}
