using UnityEngine;

public class PlayerController : BaseController
{
	private GameManager gameManager;
    private Camera cam;

	public void Init(GameManager gameManager)
	{
		this.gameManager = gameManager;
		cam = Camera.main;
	}

	protected override void HandleAction()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		movementDirection = new Vector2(horizontal, vertical).normalized;

		Vector2 mousePosition = Input.mousePosition;
		Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);
		lookDirection = (worldPos - (Vector2)transform.position);

		if (lookDirection.magnitude < .9f)
			lookDirection = Vector2.zero;
		else
			lookDirection = lookDirection.normalized;

		isAttacking = Input.GetMouseButton(0);
	}
	
	public override void Death()
	{
		base.Death();
		gameManager.GameOver();
	}

	 
	 public void UseItem(ItemData itemData)
	 {
		foreach (var statEntry in itemData.statEntries)
		{
			statHandler.ModifyStat(statEntry.statType, statEntry.baseValue, itemData.isTemporary, itemData.duration);
		}
	 }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ItemHandler>(out ItemHandler itemHandler)	)
        {
			if (itemHandler.ItemData == null)
				return;

            UseItem(itemHandler.ItemData);
            Destroy(collision.gameObject);
        }
    }
}
