using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Type of damage 
    /// </summary>
    public abstract class DamageDealBehavior : ProjectileColliisionBehavior
    {
        [SerializeField] private float _damage;



        public float Damage => _damage;
    } 
}
