using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public abstract class WarrioirsPickStrategy : ScriptableObject
{
    public abstract IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs);


/*    public enum SpawnType
    {
        RandomGroup, // Group of random warrioirs
        RandomSingle, // Random single warrioir
        QueueSingle, // Picks single warrioir in course from all avaliable warrioirs
        QueueGroup // Picks concrete warrioir group
    };*/


/*    [Serializable]
    public struct SpawnCycleData
    {
        public float NextSpawnTime;
        public float SpawnTimeDelta;
        //public SpawnType SpawnType;


        public SpawnCycleData(float nextSpawnTime, float spawnTimeDelta)
        {
            NextSpawnTime = nextSpawnTime;
            SpawnTimeDelta = spawnTimeDelta;
           // SpawnType = spawnType;
        }
    }*/
}
