using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WorldObject
{
	public float AttackHitDelay = 0.2f;

	public int MoveOnAttack;

	public float MoveOnAttackTime;

	public AudioSource AttackAudio;

	public AudioSource PrepareAttackAudio;

	public AudioSource AfterAttackAudio;

	protected IEnumerator attackCoroutine;

	[HideInInspector]
	public bool AttackWaitState;

	public ParticleSystem ParticlesHitRight;

	public ParticleSystem ParticlesHitLeft;

	public ParticleSystem LowCoinsHitEffect;

	public ParticleSystem HighCoinsHitEffect;

	public List<Renderer> EnableOnAttack = new List<Renderer>();

	public bool IsHitBySteppedOn = true;

	public int HitResistance = 1;

	protected int hitCount;

	protected IEnumerator fallbackCoroutine;

	public float FallBackTime = 0.4f;

	public bool Attacking => attackCoroutine != null;

	public bool FallingBack => fallbackCoroutine != null;

	private void Start()
	{
		for (int i = 0; i < EnableOnAttack.Count; i++)
		{
			EnableOnAttack[i].enabled = false;
		}
	}

	public override void OnSteppedOn(WorldObject stepper)
	{
		if (FallingBack)
		{
			return;
		}
		if (IsHitBySteppedOn)
		{
			OnHit(Vector3.down, stepper);
			if (stepper is Digger)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.StepOnEnemy);
			}
			return;
		}
		stepper.OnHit(Vector3.up, this);
		if (!stepper.Broken)
		{
			OnHit(Vector3.down, stepper);
			if (stepper is Digger)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.StepOnEnemy);
			}
		}
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (!SaveGame.Instance.TutorialComplete && SingletonMonoBehaviour<World>.Instance.GetSection(Coord.Section).IsTutorial)
		{
			StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(1f, 1f, 0.2f));
		}
		hitCount++;
		if (HitResistance <= hitCount)
		{
			base.OnHit(direction, hitter, medium);
			if (Coins > 20 && (bool)HighCoinsHitEffect)
			{
				HighCoinsHitEffect.Play();
			}
			else if ((bool)LowCoinsHitEffect)
			{
				LowCoinsHitEffect.Play();
			}
			if (hitter is Digger || (hitter is PushCube && ((PushCube)hitter).RedirectCoins is Digger))
			{
				SingletonMonoBehaviour<Game>.Instance.DiggerKilledEnemy(this);
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillEnemy);
			}
			if (base.OnScreen && hitter.OnScreen)
			{
				if (hitter is Fireball)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillEnemyWithFireball);
				}
				else if (hitter is Spike)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillEnemyWithSpike);
				}
				else if (hitter is GiftCube)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillEnemyWithGiftBox);
				}
				else if (hitter is Enemy)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.LetThemDoTheWork);
				}
				else if ((medium is PushCube && hitter is Digger) || (hitter is PushCube && ((PushCube)hitter).RedirectCoins is Digger))
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.RunOverEnemy);
					SingletonMonoBehaviour<Game>.Instance.LevelKillCount++;
				}
			}
		}
		else if (fallbackCoroutine == null)
		{
			StartCoroutine(fallbackCoroutine = FallBackCoroutine(direction));
		}
	}

	public void Attack(Vector3 direction, WorldObject wo)
	{
		if (attackCoroutine == null)
		{
			string triggerName = (!(direction == Vector3.right)) ? "attackLeftPrepare" : "attackRightPrepare";
			StartCoroutine(attackCoroutine = AttackCoroutine(direction, wo, wo.Coord, triggerName));
		}
	}

	public void Attack(Vector3 direction, WorldObject wo, string triggerName)
	{
		if (attackCoroutine == null)
		{
			StartCoroutine(attackCoroutine = AttackCoroutine(direction, wo, wo.Coord, triggerName));
		}
	}

	protected virtual IEnumerator AttackCoroutine(Vector3 direction, WorldObject target, Coord targetCoord, string triggerName)
	{
		for (int i = 0; i < EnableOnAttack.Count; i++)
		{
			EnableOnAttack[i].enabled = true;
		}
		Coord myCoord = Coord;
		if ((bool)PrepareAttackAudio)
		{
			PrepareAttackAudio.Play();
		}
		if ((bool)Animator)
		{
			Animator.SetTrigger(triggerName);
		}
		while (Animator != null && !AttackWaitState)
		{
			yield return null;
		}
		if ((bool)target.Walker)
		{
			while ((!target.Walker.IsGrounded || target.Walker.Velocity.x != 0f) && myCoord == Coord && targetCoord == target.Coord)
			{
				yield return null;
			}
		}
		if (SingletonMonoBehaviour<World>.Instance.GetSection(Coord.Section).IsTutorial && !SaveGame.Instance.TutorialComplete)
		{
			StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(0.05f, 0.5f, 0.1f));
		}
		if ((bool)Animator)
		{
			Animator.SetTrigger("attack");
		}
		if ((bool)AttackAudio)
		{
			AttackAudio.Play();
		}
		WorldObject onside = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction));
		if (onside == target)
		{
			StartCoroutine(MoveOnAttackCoroutine(direction));
		}
		yield return new WaitForSeconds(AttackHitDelay);
		if ((bool)AfterAttackAudio)
		{
			AfterAttackAudio.Play();
		}
		if (ParticlesHitLeft != null && triggerName == "attackLeftPrepare")
		{
			ParticlesHitLeft.Play();
		}
		if (ParticlesHitRight != null && triggerName == "attackRightPrepare")
		{
			ParticlesHitRight.Play();
		}
		if ((Walker == null || Walker.IsGrounded) && myCoord == Coord && targetCoord == target.Coord && !base.Broken)
		{
			target.OnHit(direction, this);
			if (target.Broken && MoveOnAttack > 0)
			{
				MoveCoord(targetCoord);
			}
		}
		while (Animator != null && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Right") && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Left") && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			yield return null;
		}
		if (ParticlesHitLeft != null)
		{
			ParticlesHitLeft.Stop();
		}
		if (ParticlesHitRight != null)
		{
			ParticlesHitRight.Stop();
		}
		for (int j = 0; j < EnableOnAttack.Count; j++)
		{
			EnableOnAttack[j].enabled = false;
		}
		attackCoroutine = null;
	}

	public override void OnGrounded()
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Down).Normalize());
		if (ShouldAttack(tile))
		{
			tile.OnHit(Vector3.down, this);
		}
	}

	private IEnumerator MoveOnAttackCoroutine(Vector3 direction)
	{
		if (MoveOnAttack == 0)
		{
			yield break;
		}
		Walker.enabled = false;
		float startTime = Time.time;
		Vector3 startPosition = base.transform.position;
		Vector3 endPosition = base.transform.position + direction * MoveOnAttack;
		while (attackCoroutine != null && Time.time - startTime < MoveOnAttackTime)
		{
			float t2 = (Time.time - startTime) / MoveOnAttackTime;
			t2 = Tween.CubicEaseInOut(t2, 0f, 1f, 1f);
			base.transform.position = Vector3.Lerp(startPosition, endPosition, t2);
			yield return null;
		}
		Walker.enabled = !base.Broken;
		if (attackCoroutine != null)
		{
			base.transform.position = endPosition;
			TileMap section = SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section];
			Coord newCoord = section.GetCoordFromLocalPosition(section.transform.InverseTransformPoint(base.transform.position));
			if (SingletonMonoBehaviour<World>.Instance.GetTile(newCoord) == null && !base.Broken)
			{
				MoveCoord(newCoord);
			}
		}
	}

	private IEnumerator FallBackCoroutine(Vector3 direction)
	{
		Walker.enabled = false;
		yield return Breakable.Flash();
		CancelAttack();
		Vector3 startPosition = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Coord);
		Coord newCoord = SingletonMonoBehaviour<World>.Instance.GetCoordFromPosition(base.transform.position + direction);
		Vector3 endPosition = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(newCoord);
		if (SingletonMonoBehaviour<World>.Instance.GetTile(newCoord) == null)
		{
			MoveCoord(newCoord);
			float startTime = Time.time;
			while (Time.time - startTime < FallBackTime)
			{
				float t2 = (Time.time - startTime) / FallBackTime;
				t2 = Tween.CubicEaseInOut(t2, 0f, 1f, 1f);
				base.transform.position = Vector3.Lerp(startPosition, endPosition, t2);
				yield return null;
			}
			base.transform.position = endPosition;
		}
		else
		{
			yield return new WaitForSeconds(FallBackTime);
		}
		Walker.enabled = !base.Broken;
		fallbackCoroutine = null;
	}

	public void CancelAttack()
	{
		if (attackCoroutine != null)
		{
			Animator.SetTrigger("cancelAttack");
			StopCoroutine(attackCoroutine);
			attackCoroutine = null;
			for (int i = 0; i < EnableOnAttack.Count; i++)
			{
				EnableOnAttack[i].enabled = false;
			}
		}
	}

	public virtual bool ShouldAttack(WorldObject wo)
	{
		return fallbackCoroutine == null && wo != null && (wo is Digger || wo is Coin || wo is Enemy || wo is Chicken);
	}

	public override void Disarm()
	{
		Breakable.Break(Vector3.up);
	}

	public override void IncrementCoins(WorldObject source, int coins)
	{
		if (base.OnScreen)
		{
			base.IncrementCoins(source, coins);
		}
	}
}
