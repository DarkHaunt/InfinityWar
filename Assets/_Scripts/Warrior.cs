using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Warrior : MonoBehaviour
{
    protected abstract void Attack(IHitable target);
    protected abstract bool IsOnArguingDistance();



    protected enum States
    {
        Attack,
        Walk,
        Arguing
    }
}
