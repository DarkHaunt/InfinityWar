using InfinityGame.Projectiles;
using InfinityGame.ObjectPooling;
using UnityEngine;



namespace InfinityGame.Factories.ProjectileFactory
{
    public static class ProjectileFactory
    {
        private static readonly ObjectPooler<Projectile> _projectilePool = new ObjectPooler<Projectile>();



        public static Projectile Instantiate(Projectile prefab, Vector2 position, string fractionTag)
        {
            if (!_projectilePool.TryGetFromPool(prefab.PoolTag, out Projectile projectile))
            {
                projectile = MonoBehaviour.Instantiate(prefab);
                projectile.SetFractionTag(fractionTag);
                projectile.OnExploitationEnd += () => _projectilePool.AddToPool(projectile);
            }

            projectile.transform.position = position;

            return projectile;
        }
    }
}