using UnityEngine;

public class EnemyPatrol : Enemy
{
	public AudioSource RunAudio;

	private Vector3 direction;

	private float startTime;

	private void Start()
	{
		direction = ((Random.Range(0, 2) != 0) ? Vector3.left : Vector3.right);
		Walker.Animator = Animator;
		startTime = Time.time + Random.Range(0f, 0.5f);
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		base.OnHit(direction, hitter, medium);
		if ((bool)RunAudio)
		{
			RunAudio.Stop();
		}
	}

	public override void OnSteppedOn(WorldObject stepper)
	{
		if ((bool)stepper.Walker && stepper.Walker.Velocity.x != 0f)
		{
			return;
		}
		base.OnSteppedOn(stepper);
		if (!IsHitBySteppedOn)
		{
			if (Animator != null)
			{
				Animator.SetTrigger("steppedOn");
			}
			Walker.Stop();
			startTime = Time.time + 0.6f;
		}
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (ShouldAttack(wo))
		{
			Attack(direction, wo);
		}
		else
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
		if (Time.time < startTime)
		{
			return;
		}
		if (!base.Attacking)
		{
			Walker.Move(direction);
			if ((bool)Animator)
			{
				Animator.SetFloat("direction", (!Walker.trajectory.Active || !Walker.trajectory.decelerating || ShouldAttack(Walker.moveTo) || base.FallingBack) ? direction.x : (0f - direction.x));
			}
		}
		if (!(RunAudio != null))
		{
			return;
		}
		if (Walker.Velocity.x != 0f)
		{
			if (!RunAudio.isPlaying)
			{
				RunAudio.Play();
			}
		}
		else if (RunAudio.isPlaying)
		{
			RunAudio.Stop();
		}
	}
}
