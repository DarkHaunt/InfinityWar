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
            // TODO: Тег присваивать тут
            if (!_projectilePool.TryGetFromPool(prefab.PoolTag, out Projectile projectile))
            {
                projectile = MonoBehaviour.Instantiate(prefab);
                projectile.OnExpluatationEnd += () => _projectilePool.AddToPool(projectile);
            }

            return projectile;
        }
    }
}