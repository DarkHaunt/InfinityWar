using System.Collections;
using System.Collections.Generic;
using InfinityGame.Projectiles;
using UnityEngine;


namespace InfinityGame.Factories.ProjectileFactory
{
    public static class ProjectileFactory
    {
        public static ProjectileType Instantiate<ProjectileType>(ProjectileType prefab) where ProjectileType : Projectile => MonoBehaviour.Instantiate(prefab);
    }
}