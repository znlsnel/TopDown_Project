using UnityEngine;
using UnityEngine.UIElements;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get => movementDirection; }

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get => lookDirection; }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        handleAction();
        Rotate(lookDirection);

	}

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void handleAction()
    {

    }

    private void Movment(Vector2 direction)
    {
		direction = direction * 5;
		if (knockbackDuration > 0.0f)
		{
			direction *= 0.2f;
			direction += knockback;
		}
		_rigidbody.linearVelocity = direction;
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
