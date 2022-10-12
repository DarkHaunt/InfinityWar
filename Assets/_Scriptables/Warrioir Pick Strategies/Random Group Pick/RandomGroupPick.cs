using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnStrategy", menuName = "Data/WarrioirChoseStrategies/RandomGroupPick", order = 52)]
public class RandomGroupPick : WarrioirsPickStrategy
{
    private System.Random _randomizer = new System.Random();
    [SerializeField] private int _groupMembersCount;


    public override IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs)
    {
        for (int i = 0; i < _groupMembersCount; i++)
            yield return warrioirs[_randomizer.Next(0, warrioirs.Count)];
    }
}
