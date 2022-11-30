using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Will deal damage on collision with something
    /// </summary>
    public abstract class DamageDealBehavior : ProjectileColliisionBehavior
    {
        [SerializeField] private float _damage;



        public float Damage => _damage;
    } 
}
