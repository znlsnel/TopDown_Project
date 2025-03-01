using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform progectileSpawnPosition;
    [SerializeField] private int bulletIndex;
    [SerializeField] private float bulletSize = 1;
    [SerializeField] private float duration;
    [SerializeField] private float spread;
	[SerializeField] private int numberofProjectilesPerShot;
	[SerializeField] private float multipleProjectilesAngle;
	[SerializeField] private Color projectileColor;
	public int BulletIndex { get { return bulletIndex; } }
    public float BulletSize { get { return bulletSize; } }
    public float Duration { get { return duration; } }
    public float Spread { get { return spread; } }
    public int NumberofProjectilesPerShot { get {  return numberofProjectilesPerShot; } }
    public float MultipleProjectilesAngle { get { return multipleProjectilesAngle; } }

    public Color ProjectileColor { get { return projectileColor; } }


	public override void Attack()
	{
		base.Attack();

        float projectilesAngleSpace = multipleProjectilesAngle;
        int numberOfProjectilesPerShot = numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace;

        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;

            CreateProjectile(Controller.LookDirection, angle);
        }
	}

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {

    }

    private static Vector2 RotateVector2 (Vector2 v, float degree) 
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }

}
