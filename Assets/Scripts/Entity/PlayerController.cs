using UnityEngine;

public class PlayerController : BaseController
{
    private Camera camera;

	protected override void Start()
	{
		base.Start();
		camera = Camera.main;
	}

	protected override void handleAction()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		movementDirection = new Vector2(horizontal, vertical).normalized;

		Vector2 mousePosition = Input.mousePosition;
		Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
		lookDirection = (worldPos - (Vector2)transform.position);

		if (lookDirection.magnitude < .9f)
			lookDirection = Vector2.zero;
		else
			lookDirection = lookDirection.normalized;

		isAttacking = Input.GetMouseButton(0);
	}
	

}
