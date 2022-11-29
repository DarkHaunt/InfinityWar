using UnityEngine;



namespace InfinityGame.GameEntities
{
    public abstract class MeleeWarrior : Warrior
    {
        [Header("--- Melee Warrioir Parameters ---")]
        [SerializeField] private float _meleeDamage;



        protected float MeleeDamage => _meleeDamage;
    } 
}
