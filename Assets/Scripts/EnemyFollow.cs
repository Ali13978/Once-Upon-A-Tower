using UnityEngine;

public class EnemyFollow : Enemy
{
	public AudioSource RunAudio;

	public AudioSource FallingAudio;

	protected bool asleep = true;

	protected Vector3 lockDirection;

	protected float yLockDirection = -100f;

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		base.OnHit(direction, hitter, medium);
		if ((bool)RunAudio)
		{
			RunAudio.Stop();
		}
		if ((bool)FallingAudio)
		{
			FallingAudio.Stop();
		}
		if (hitter is Digger && Vector3.up == direction)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.HitFallingWildPig);
		}
	}

	private void FixedUpdate()
	{
		if (base.Attacking)
		{
			return;
		}
		if (!asleep)
		{
			Vector3 position = base.transform.position;
			if (Mathf.Abs(position.y - yLockDirection) > 0.5f)
			{
				Vector3 position2 = SingletonMonoBehaviour<Game>.Instance.Digger.transform.position;
				float x = position2.x;
				Vector3 position3 = base.transform.position;
				if (x > position3.x)
				{
					Walker.Move(Vector3.right);
				}
				else
				{
					Walker.Move(Vector3.left);
				}
			}
			else
			{
				Walker.Move(lockDirection);
			}
			if (Walker.Direction.x != 0f)
			{
				Animator.SetFloat("direction", Walker.Direction.x);
			}
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Down).Normalize());
			if (ShouldAttack(tile))
			{
				string triggerName = (!(Walker.Direction.x > 0f)) ? "attackLeftPrepare" : "attackRightPrepare";
				Walker.Stop();
				Attack(Vector3.down, tile, triggerName);
			}
		}
		else
		{
			Vector3 position4 = SingletonMonoBehaviour<Game>.Instance.Digger.transform.position;
			float y = position4.y;
			Vector3 position5 = base.transform.position;
			if (y < position5.y + 0.5f)
			{
				asleep = false;
				Animator.SetBool("asleep", value: false);
			}
			else
			{
				Walker.Stop();
			}
		}
		if (RunAudio != null)
		{
			if (!asleep && Walker.Velocity.x != 0f)
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
		if (!(FallingAudio != null))
		{
			return;
		}
		if (!asleep && Walker.Velocity.y < -0.5f)
		{
			if (!FallingAudio.isPlaying)
			{
				FallingAudio.Play();
			}
		}
		else if (FallingAudio.isPlaying)
		{
			FallingAudio.Stop();
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
			Vector3 position = base.transform.position;
			yLockDirection = position.y;
			lockDirection = -direction;
		}
		return false;
	}
}
