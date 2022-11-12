using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



/// <summary>
/// Doing splash damage around the radius of collided target
/// </summary>
[CreateAssetMenu(fileName = "ProjectileBehavior", menuName = "Data/Projectile Collision Action/SplashDamage", order = 52)]
public class SplashDamage : ProjectileEntityCollisionAction
{
    [Range(0f, 10f)]
    [SerializeField] private float _splashRadius = 1f;



    public override void OnCollisionBehave(Projectile projectile, GameEntity target)
    {
        var enemyDetector = new FractionEntityDetector(_splashRadius, projectile.FractionTag); // TODO: Сделать его статическим?
        var detectedEnemies = enemyDetector.GetDetectedFractionEntities(target.transform.position);

        foreach (var enemy in detectedEnemies)
            enemy.GetDamage(projectile.Damage);
    }
}
