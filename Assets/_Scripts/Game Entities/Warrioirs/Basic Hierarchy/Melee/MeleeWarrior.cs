using UnityEngine;



namespace InfinityGame.GameEntities.MeleeWarriors
{
    /// <summary>
    /// A warrior, that hit enemies in melee fight
    /// </summary>
    public abstract class MeleeWarrior : Warrior
    {
        [Header("--- Melee Warrioir Parameters ---")]
        [SerializeField] private float _meleeDamage;



        protected float MeleeDamage => _meleeDamage;
    } 
}
