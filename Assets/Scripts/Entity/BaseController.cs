using UnityEngine;
using UnityEngine.UIElements;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
            timeSinceLastAttack += Time.deltaTime;
        
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }

    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get => movementDirection; }

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get => lookDirection; }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;




    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
		animationHandler = GetComponent<AnimationHandler>();
		statHandler = GetComponent<StatHandler>();


		if (WeaponPrefab != null)
			weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
		else
			weaponHandler = GetComponentInChildren<WeaponHandler>();
	}

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
		HandleAttackDelay();
	}


    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
            knockbackDuration -= Time.fixedDeltaTime;
         
    }
     
    protected virtual void HandleAction()
    {

    } 

    private void Movment(Vector2 direction)
    {
        direction = direction * statHandler.Speed;
		if (knockbackDuration > 0.0f)
		{
			direction *= 0.2f;
			direction += knockback; 
		}
		_rigidbody.linearVelocity = direction;
        animationHandler.Move(direction);
	}

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isLeft;
        if (weaponPivot != null)
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power; 
    }

    
}
