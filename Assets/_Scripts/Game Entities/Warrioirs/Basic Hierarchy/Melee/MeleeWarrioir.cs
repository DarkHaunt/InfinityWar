using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    public abstract class MeleeWarrioir : Warrior
    {
        [SerializeField] private float _meleeDamage;

        protected float MeleeDamage => _meleeDamage;
    } 
}
