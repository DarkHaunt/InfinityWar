using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.WarriorFactory
{
    public static class WarriorFactory
    {
        /*    private List<Warrior> _warriours = new List<Warrior>();
            private System.Random _randomizer = new System.Random();



            public WarriorFactory(List<Warrior> warriors)
            {
                _warriours = warriors;
            }


            public Warrior SpawnRandomWarrior()
            {
                var number = _randomizer.Next(0, _warriours.Count);

                return _warriours[number];
            }*/

        public static Warrior SpawnWarrior(Warrior prefab) => MonoBehaviour.Instantiate(prefab);
    } 
}
