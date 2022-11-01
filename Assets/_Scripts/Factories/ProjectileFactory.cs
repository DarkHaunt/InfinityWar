using InfinityGame.Projectiles;
using InfinityGame.ObjectPooling;
using UnityEngine;


namespace InfinityGame.Factories.ProjectileFactory
{
    public static class ProjectileFactory
    {
        private static readonly ObjectPooler<Projectile> _projectilePool = new ObjectPooler<Projectile>();

        public static Projectile Instantiate(Projectile prefab)
        {
            if (!_projectilePool.TryGetFromPool(prefab.PoolTag, out Projectile projectile))
            {
                projectile = MonoBehaviour.Instantiate(prefab);
                projectile.OnExploitationEnd += () => _projectilePool.AddToPool(projectile);
            }

            return projectile;
        }
    }
}