using UnityEngine;



namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Projectile, which has straight line traectory of moving
    /// </summary>
    public abstract class RotatableProjectile : Projectile
    {
        private readonly ObjectRotator _objectRotator = new ObjectRotator();



        private void RotateToTarget(Vector2 targetDirection) => _objectRotator.RoteteObjectToTarget(RigidBody2D, targetDirection);

        public override void SetFlyDirection(Vector2 direction)
        {
            RotateToTarget(direction);

            base.SetFlyDirection(direction);
        }
    }
}
