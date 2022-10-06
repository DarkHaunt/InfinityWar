using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.WarriorFactory
{
    public static class WarriorFactory
    {
        public static Warrior SpawnWarrior(Warrior prefab) => MonoBehaviour.Instantiate(prefab);
    } 
}
