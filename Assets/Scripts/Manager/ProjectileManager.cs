using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
	private static ProjectileManager instance;
	public static ProjectileManager Instance { get { return instance; } }

	[SerializeField] private GameObject[] projectilePrefabs;
	[SerializeField] private ParticleSystem impactParticleSystem;

	private ObjectPoolManager objectPoolManager;

	private void Awake()
	{
		instance = this; 
	}

    void Start()
    {
        objectPoolManager = ObjectPoolManager.Instnace;
    } 

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPostiion, Vector2 direction)
	{
		GameObject obj = objectPoolManager.GetObject(rangeWeaponHandler.BulletIndex, startPostiion, Quaternion.identity);

		ProjectileController projectileController = obj.GetComponent<ProjectileController>();
		projectileController.Init(direction, rangeWeaponHandler, this);
	}

	public void CreateImpactParticlesAtPostion(Vector3 position, RangeWeaponHandler weaponHandler)
	{
		impactParticleSystem.transform.position = position;
		ParticleSystem.EmissionModule em = impactParticleSystem.emission;
		em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));
		ParticleSystem.MainModule mainModule = impactParticleSystem.main;
		mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
		impactParticleSystem.Play();
	}

}
