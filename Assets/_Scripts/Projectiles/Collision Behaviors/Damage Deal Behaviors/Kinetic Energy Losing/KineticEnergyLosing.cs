using InfinityGame.GameEntities;
using InfinityGame.Projectiles;
using UnityEngine;



namespace  InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Makes projectile lose damage for passed distance
    /// </summary>
    [CreateAssetMenu(fileName = "KineticEnergyLosingBehavior", menuName = "Data/Projectile Collision Behaviors/KineticEnergyLosing", order = 52)]
    public class KineticEnergyLosing : DamageDealBehavior
    {
        [Range(0f, 1f)]
        [Tooltip("Minimal projectile damage, that will be used for damaging")]
        [SerializeField] private float _minimalDamagePercent = 0.1f;

        [Range(0f, 1f)]
        [Tooltip("Pecent of damage, that will be losed for 1 passed unit by projectile")]
        [SerializeField] private float _damageLoosingPercentPerUnit = 0.1f;



        public override void OnCollisionBehave(GameEntity target, Projectile projectile)
        {
            var passedDistanceInUnits = projectile.Speed * projectile.LifeTime;

            var rawDamagePercent = 1f - (passedDistanceInUnits * _damageLoosingPercentPerUnit);
            var damagePercent = (rawDamagePercent < _minimalDamagePercent) ? _minimalDamagePercent : rawDamagePercent;

            target.GetDamage(Damage * damagePercent);
        }
    } 
}
