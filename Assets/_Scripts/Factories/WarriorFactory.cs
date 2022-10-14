using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.Factories.WarriorFactory
{
    public static class WarriorFactory
    {
        public static Warrior InstantiateWarrior(Warrior warrioirPrefab) => MonoBehaviour.Instantiate(warrioirPrefab);
    }
}
