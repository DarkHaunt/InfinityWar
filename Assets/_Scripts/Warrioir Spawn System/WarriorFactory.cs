using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.WarriorFactory
{
    public static class WarriorFactory
    {
        public static Warrior InstantiateWarriorOnPosition(Warrior warrioirPrefab) => MonoBehaviour.Instantiate(warrioirPrefab);
/*        {
            var warrioir = MonoBehaviour.Instantiate(warrioirPrefab);
            warrioir.transform.position = position;

            return warrioir;
        }*/
    }
}
