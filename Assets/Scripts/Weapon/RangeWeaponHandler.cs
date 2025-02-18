using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get => bulletIndex; }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get => bulletSize; }

    [SerializeField] private float duration;
    public float Duration { get => duration; }

    [SerializeField] private float spread;
    public float Spread {  get => spread; }

    [SerializeField] private int numberoProjectilesPerShot;
    public int NumberoProjectilesPerShot { get => numberoProjectilesPerShot;}

    [SerializeField] private float multipleProjectileAngle;
    public float MultipleProjectileAngel {  get => multipleProjectileAngle;}

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get => projectileColor;}

    private ProjectileManager projectileManager;
    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberoProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace + 0.5f * multipleProjectileAngle;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }

    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        projectileManager.ShootBullet
            (this, projectileSpawnPosition.position, RotateVector2(_lookDirection, angle));
    }

    // 쿼터니언이 가지고 있는 회전 수치만큼 이 백터를 회전시켜 준다 (교환법칙이 섭립하지 않으므로 백터 * 쿼터니언은 안되는 계산식이다)
    private static Vector2 RotateVector2(Vector2 v, float degress)
    {
        return Quaternion.Euler(0, 0, degress) * v;
    }
}
