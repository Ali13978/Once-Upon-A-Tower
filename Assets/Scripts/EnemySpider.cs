using System;
using System.Collections;
using UnityEngine;

public class EnemySpider : Enemy
{
	public AudioSource RunAudio;

	public float Speed = 5f;

	private Vector3 direction = Vector3.right;

	private Vector3 up = Vector3.up;

	private bool moving;

	private Vector3 LocalDirection
	{
		get
		{
			Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward, up));
			return rotation * direction;
		}
	}

	public override void Initialize()
	{
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			direction *= -1f;
		}
	}

	private void FixedUpdate()
	{
		if (RunAudio != null)
		{
			if (!base.Attacking)
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
		if (base.Attacking)
		{
			return;
		}
		if (moving)
		{
			if ((bool)Animator && Animator.isActiveAndEnabled)
			{
				Animator animator = Animator;
				Vector3 localDirection = LocalDirection;
				animator.SetFloat("direction", localDirection.x);
			}
			return;
		}
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord - up));
		WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction));
		WorldObject tile3 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction - up));
		if (tile == null)
		{
			Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + Coord.Down);
			tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			StartCoroutine(Move((!(tile != null)) ? coord : Coord, Vector3.up, Vector3.right, 1f / Speed));
		}
		else if (tile2 != null)
		{
			if (ShouldAttack(tile2))
			{
				string triggerName = (!(LocalDirection == Vector3.right)) ? "attackLeftPrepare" : "attackRightPrepare";
				Attack(direction, tile2, triggerName);
			}
			else
			{
				StartCoroutine(Move(Coord, -direction, up, (float)Math.PI / 4f / Speed));
			}
		}
		else if (tile3 != null)
		{
			StartCoroutine(Move(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction), up, direction, 1f / Speed));
		}
		else
		{
			StartCoroutine(Move(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction - up), direction, -up, (float)Math.PI / 2f / Speed));
		}
	}

	private void OnDisable()
	{
		if (moving)
		{
			moving = false;
			base.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Coord);
		}
	}

	private IEnumerator Move(Coord target, Vector3 targetUp, Vector3 targetDirection, float time)
	{
		if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(target))
		{
			direction = -targetDirection;
			yield break;
		}
		moving = true;
		Vector3 startUp = up;
		Vector3 startPosition = base.transform.position;
		Vector3 targetPosition = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(target);
		Quaternion startRotation = Quaternion.LookRotation(Vector3.forward, up);
		Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetUp);
		if (target.Section < 0 || target.Section >= SingletonMonoBehaviour<World>.Instance.Sections.Count || SingletonMonoBehaviour<World>.Instance.Sections[target.Section] == null)
		{
			moving = false;
			base.gameObject.SetActive(value: false);
			yield break;
		}
		MoveCoord(target);
		float startTime = Time.fixedTime;
		WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
		while (Time.fixedTime + Time.fixedDeltaTime - startTime < time)
		{
			float a = (Time.fixedTime + Time.fixedDeltaTime - startTime) / time;
			if (startUp != targetUp && Vector3.Distance(startPosition - startUp, targetPosition - targetUp) < 0.1f)
			{
				base.transform.position = startPosition - startUp + Vector3.Slerp(startUp, targetUp, a);
			}
			else
			{
				base.transform.position = Vector3.Lerp(startPosition, targetPosition, a);
			}
			base.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, a);
			yield return waitForFixedUpdate;
		}
		up = targetUp;
		direction = targetDirection;
		moving = false;
	}

	public override bool ShouldAttack(WorldObject wo)
	{
		return fallbackCoroutine == null && wo != null && (wo is Digger || wo is Coin || wo is Chicken);
	}
}
