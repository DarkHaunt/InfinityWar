using InfinityGame.GameEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    public class SingleTouchProjectile : Projectile
    {
        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            EndExploitation();
        }
    } 
}
