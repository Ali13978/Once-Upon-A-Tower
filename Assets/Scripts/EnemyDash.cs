using System.Collections;
using UnityEngine;

public class EnemyDash : Enemy
{
	public int DashRange = 2;

	public float DashDelay = 0.4f;

	public float DashTime = 0.3f;

	private Vector3 direction;

	private float startTime;

	private void Start()
	{
		direction = ((Random.Range(0, 2) != 0) ? Vector3.left : Vector3.right);
		Walker.Animator = Animator;
		startTime = Time.time + Random.Range(0f, 0.5f);
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (!ShouldAttack(wo))
		{
			this.direction = -direction;
		}
		return false;
	}

	public override void StoppedBeforeFall()
	{
		direction = -direction;
	}

	private void FixedUpdate()
	{
		if (!(Time.time < startTime))
		{
			if (!base.Attacking)
			{
				Walker.Move(direction);
				Animator.SetFloat("direction", (!Walker.trajectory.Active || !Walker.trajectory.decelerating || ShouldAttack(Walker.moveTo)) ? direction.x : (0f - direction.x));
			}
			WorldObject wo;
			if (ShouldAttack(direction, out wo))
			{
				Attack(direction, wo);
			}
		}
	}

	private bool ShouldAttack(Vector3 direction, out WorldObject wo)
	{
		wo = null;
		if (direction != Vector3.left && direction != Vector3.right)
		{
			return false;
		}
		for (int i = 1; i <= DashRange; i++)
		{
			Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + i * direction);
			wo = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			if (ShouldAttack(wo))
			{
				return true;
			}
		}
		return false;
	}

	protected override IEnumerator AttackCoroutine(Vector3 direction, WorldObject target, Coord targetCoord, string triggerName)
	{
		Vector3 endPosition = target.transform.position;
		Vector3 position = base.transform.position;
		endPosition.y = position.y;
		Walker.Stop();
		while (Walker.Moving)
		{
			yield return null;
		}
		Walker.enabled = false;
		yield return new WaitForSeconds(DashDelay);
		float startTime = Time.time;
		Vector3 startPosition = base.transform.position;
		while (Time.time - startTime < DashTime)
		{
			float t2 = (Time.time - startTime) / DashTime;
			t2 = Tween.CubicEaseInOut(t2, 0f, 1f, 1f);
			base.transform.position = Vector3.Lerp(startPosition, endPosition, t2);
			Coord newCoord = SingletonMonoBehaviour<World>.Instance.GetCoordFromPosition(base.transform.position);
			if (!base.Broken && !target.Broken)
			{
				if (target.Coord == newCoord)
				{
					target.OnHit(direction, this);
				}
				if (SingletonMonoBehaviour<World>.Instance.GetTile(newCoord) == null)
				{
					MoveCoord(newCoord);
				}
			}
			yield return null;
		}
		Walker.enabled = !base.Broken;
		attackCoroutine = null;
	}
}
