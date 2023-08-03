using System.Collections;
using UnityEngine;

public class EnemyFire : Enemy
{
	public Flyer Flyer;

	private Vector3 direction;

	private float startTime;

	private void Start()
	{
		direction = ((Random.Range(0, 2) != 0) ? Vector3.left : Vector3.right);
		startTime = Time.time + Random.Range(0f, 0.5f);
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		WorldObject wo2 = null;
		if (!ShouldAttack(direction, out wo2))
		{
			this.direction = -direction;
		}
		return false;
	}

	private void FixedUpdate()
	{
		if (Time.time < startTime)
		{
			return;
		}
		WorldObject wo = null;
		if (ShouldAttack(direction + Vector3.down, out wo))
		{
			Attack(direction, wo);
		}
		else if (ShouldAttack(direction, out wo))
		{
			Attack(direction, wo);
		}
		if (!base.Attacking)
		{
			Flyer.Move(direction);
			if ((bool)Animator)
			{
				Animator.SetFloat("direction", (!Flyer.trajectory.Active || !Flyer.trajectory.decelerating || !(wo == null)) ? direction.x : (0f - direction.x));
			}
		}
	}

	private bool ShouldAttack(Vector3 direction, out WorldObject wo)
	{
		wo = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + direction).Normalize());
		if (attackCoroutine == null)
		{
			if (direction == Vector3.left || direction == Vector3.right)
			{
				return fallbackCoroutine == null && wo != null && (wo is Coin || wo is Enemy);
			}
			return ShouldAttack(wo);
		}
		return false;
	}

	protected override IEnumerator AttackCoroutine(Vector3 direction, WorldObject target, Coord targetCoord, string triggerName)
	{
		Flyer.Stop();
		while (Flyer.Moving)
		{
			yield return null;
		}
		Flyer.enabled = false;
		yield return base.AttackCoroutine(direction, target, targetCoord, triggerName);
		Flyer.enabled = !base.Broken;
	}

	public override void Initialize()
	{
		base.Initialize();
		if (Flyer != null)
		{
			Flyer.Initialize();
		}
	}
}
