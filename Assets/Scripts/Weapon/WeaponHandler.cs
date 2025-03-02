using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;
    [SerializeField] private float weaponSize = 1f;
    [SerializeField] private float power = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 10f;

    public float Delay { get => delay; set => delay = value; }
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }
    public float Power { get => power; set => power = value; }
    public float Speed { get => speed; set => speed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }


    public LayerMask target;


    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false;
    [SerializeField] private float knockbackTime = 0.5f;
    [SerializeField] private float knockbackPower = 0.1f;

    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }
    public float KnockbackTime {  get => knockbackTime; set => knockbackTime = value; }
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    
	public AudioClip attackSoundClip;

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public BaseController Controller { get; private set; }

    private Animator animator;
    private SpriteRenderer weaponRenderer;


	protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
		AttakAnimation();

		if (attackSoundClip != null)
			SoundManager.PlayClip(attackSoundClip);
	}

    public void AttakAnimation()
    {
        animator.SetTrigger("IsAttack");

	}

    public virtual  void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft; 
    }
}
