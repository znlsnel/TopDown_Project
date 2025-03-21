using UnityEngine;
using System;

public class ProjectileController : MonoBehaviour, IPoolable
{
	[SerializeField] private LayerMask levelCollisionLayer;

	private RangeWeaponHandler rangeWeaponHandler;

	private float currentDuration;
	private Vector2 direction;
	private bool isReady;
	private Transform pivot;
	private Action<GameObject> returnToPool;

	private Rigidbody2D _rigidbody;
	private SpriteRenderer spriteRenderer;
	private ProjectileManager projectileManager;

	public bool fxOnDestory = true;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
		pivot = transform.GetChild(0);
	}

	private void Update()
	{
		if (!isReady)
			return;

		currentDuration += Time.deltaTime;

		if (currentDuration > rangeWeaponHandler.Duration)
			DestroyProjectile(transform.position, false);

		_rigidbody.linearVelocity = direction * rangeWeaponHandler.Speed;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((levelCollisionLayer.value == (levelCollisionLayer.value | 1 << collision.gameObject.layer)))
			DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);

		else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
		{
			ResourceController resourceController = collision.GetComponent<ResourceController>();	
			if (resourceController != null)
			{
				resourceController.ChangeHealth(-rangeWeaponHandler.Power);
				if (rangeWeaponHandler.IsOnKnockback)
				{
					BaseController controller = collision.GetComponent<BaseController>();
					if (controller != null)
						controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
					
				}

			}

			DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
		}

	}

	public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager pjm)
	{
		this.projectileManager = pjm;
		rangeWeaponHandler = weaponHandler;

		this.direction = direction;
		currentDuration = 0;
		transform.localScale = Vector3.one * weaponHandler.BulletSize;
		spriteRenderer.color = weaponHandler.ProjectileColor;

		transform.right = this.direction;
		 
		if (this.direction.x < 0)
			pivot.localRotation = Quaternion.Euler(180, 0, 0);
		else
			pivot.localRotation = Quaternion.Euler(0, 0, 0);

		isReady = true;
	}

	private void DestroyProjectile(Vector3 position, bool createFx)
	{
		if (createFx)
			projectileManager.CreateImpactParticlesAtPostion(position, rangeWeaponHandler);
		
		OnDespawn();
	}

	public void Initialize(Action<GameObject> returnAction)
	{
		returnToPool = returnAction;
	}

	public void OnSpawn()
	{
		gameObject.SetActive(true);
	}

	public void OnDespawn()
	{
		returnToPool?.Invoke(gameObject);

	}
}
