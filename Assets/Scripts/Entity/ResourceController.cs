using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceController : MonoBehaviour
{
	[SerializeField] private float healthChangeDelay = 0.5f;

	private BaseController baseController;
	private StatHandler statHandler;
	private AnimationHandler animationHandler;

	private float timeSinceLastChange = float.MaxValue;

	public float CurrentHealth { get; private set; }
	public float MaxHealth => statHandler.GetStat(StatType.Health);

	public AudioClip damageSound;
	private Action<float, float> OnChangedHealth;

	private void Awake()
	{
		statHandler = GetComponent<StatHandler>();
		baseController = GetComponent<BaseController>();
		animationHandler = GetComponent<AnimationHandler>();
	}

	private void Start()
	{
		CurrentHealth = statHandler.GetStat(StatType.Health);
	}

	private void Update()
	{
		if (timeSinceLastChange < healthChangeDelay)
		{
			timeSinceLastChange += Time.deltaTime;
			if (timeSinceLastChange >= healthChangeDelay)
				animationHandler.InvincibilityEnd();
		}
	}
	
	public bool ChangeHealth(float change)
	{
		if (change == 0 || timeSinceLastChange < healthChangeDelay)
			return false;

		timeSinceLastChange = 0f;
		CurrentHealth = Mathf.Clamp(CurrentHealth + change, 0, MaxHealth);

		OnChangedHealth?.Invoke(CurrentHealth, MaxHealth);

		if (change < 0)
			animationHandler.Damage();

		if (CurrentHealth <= 0.0f)
			Death();

		return true;
	} 

	private void Death()
	{
		baseController.Death();
	}

	public void AddHealthChangeEvent(Action<float, float> action)
	{
		OnChangedHealth += action;
	}

	public void RemoveHealthCangeEvent(Action<float, float> action)
	{
		OnChangedHealth -= action;
	}


}
