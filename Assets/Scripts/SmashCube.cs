using System.Collections;
using UnityEngine;

public class SmashCube : WorldObject
{
	public Vector3 StartDirection = Vector3.down;

	public float AccelerationMagnitude = 160f;

	private float moveDelay = 2f;

	private float shakeDelay = 0.5f;

	private Vector3 direction;

	private IEnumerator shakeAndMoveCoroutine;

	private Vector3 Acceleration => AccelerationMagnitude * direction;

	private void Start()
	{
		if (SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section].Flipped)
		{
			StartDirection.x *= -1f;
		}
		direction = StartDirection;
	}

	private void FixedUpdate()
	{
		if (!base.Broken && shakeAndMoveCoroutine == null && !Projectile.Moving)
		{
			StartCoroutine(shakeAndMoveCoroutine = ShakeAndMoveCoroutine());
		}
	}

	private IEnumerator ShakeAndMoveCoroutine()
	{
		yield return new WaitForSeconds(moveDelay);
		yield return new WaitForSeconds(shakeDelay);
		Projectile.Move(Acceleration, Vector3.zero);
		shakeAndMoveCoroutine = null;
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (wo is Digger || wo is Enemy)
		{
			wo.OnHit(direction, this);
			if (wo.Broken)
			{
				return true;
			}
		}
		this.direction = -this.direction;
		return false;
	}
}
