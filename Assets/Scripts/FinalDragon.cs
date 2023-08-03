using Achievements;
using System.Collections;
using UnityEngine;

public class FinalDragon : MonoBehaviour
{
	public Animator Animator;

	public Transform FireStart;

	public float AttackPeriod = 1f;

	public DragonFireball Fireball;

	public ParticleSystem FireParticles;

	public float HitPoints = 3f;

	public AudioSource AttackAudio;

	public AudioSource HitAudio;

	public AudioSource MusicAudio;

	public AudioSource LandAudio;

	public AudioSource BiteAudio;

	public AudioSource BellyHit;

	public Ending Ending;

	private float lastAttack;

	private float hitTime;

	private bool firstAttack = true;

	private bool started;

	private bool IsDead;

	private void Start()
	{
		Animator.enabled = true;
	}

	private void FixedUpdate()
	{
		if (HitPoints <= 0f)
		{
			return;
		}
		Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
		if (Time.fixedTime - lastAttack > AttackPeriod && Time.fixedTime - hitTime > 2f && Vector3.Distance(digger.transform.position, base.transform.position) < 10f && digger.Walker.IsGrounded)
		{
			if (!started)
			{
				started = true;
				Animator.SetTrigger("start");
			}
			lastAttack = Time.fixedTime;
			StartCoroutine(AttackCoroutine());
			firstAttack = false;
		}
		if (!MusicAudio.isPlaying && !IsDead && Vector3.Distance(digger.transform.position, base.transform.position) < 12f)
		{
			MusicAudio.Play();
		}
		if (MusicAudio.isPlaying && digger != null && digger.IsDead)
		{
			MusicAudio.Stop();
		}
	}

	private IEnumerator AttackCoroutine()
	{
		for (int i = 0; i < 2; i++)
		{
			if (IsDead)
			{
				break;
			}
			if (SingletonMonoBehaviour<Game>.Instance.Digger.Coord.x > 5)
			{
				if (BiteAudio != null)
				{
					BiteAudio.Play();
				}
				Animator.SetTrigger("attack_bite");
			}
			else
			{
				Animator.SetTrigger("attack");
			}
			yield return new WaitForSeconds(1.5f);
		}
	}

	private void AttackStartEvent()
	{
		AttackAudio.Play();
	}

	private void IntroAudioEvent()
	{
		if (LandAudio != null)
		{
			LandAudio.Play();
		}
	}

	private void BiteAttackEvent()
	{
		if (!IsDead && SingletonMonoBehaviour<Game>.Instance.Digger.Coord.x > 5)
		{
			Coord coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
			coord.x = 8;
			SingletonMonoBehaviour<Game>.Instance.Digger.OnHit(Vector3.down, SingletonMonoBehaviour<World>.Instance.GetTile(coord));
		}
	}

	private void AttackEvent()
	{
		if (!IsDead)
		{
			Coord coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
			if (coord.x > 5)
			{
				coord.x = 5;
			}
			Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
			positionFromCoord += new Vector3(0f, -0.5f, 0f) + new Vector3(firstAttack ? 1 : 0, 0f, 0f);
			firstAttack = false;
			Vector3 position = FireStart.transform.position;
			DragonFireball component = UnityEngine.Object.Instantiate(Fireball.gameObject).GetComponent<DragonFireball>();
			component.Fire(position, positionFromCoord);
			FireParticles.Play();
		}
	}

	public void OnHit(Vector3 direction, WorldObject hitter)
	{
		hitTime = Time.fixedTime;
		HitAudio.Play();
		HitPoints -= 1f;
		if (HitPoints <= 0f)
		{
			Die();
		}
		else
		{
			Animator.SetTrigger("hit");
		}
	}

	private void Die()
	{
		if (!IsDead)
		{
			IsDead = true;
			Animator.SetBool("dead", value: true);
			MusicAudio.Stop();
			SingletonMonoBehaviour<Game>.Instance.Digger.IncrementCoins(null, 1000);
			SaveGame.Instance.SetCharacterRescued(SaveGame.Instance.CurrentCharacter, value: true);
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.EscapeTower);
			SingletonMonoBehaviour<AchievementManager>.Instance.NotifyEscapeTower();
			if (SingletonMonoBehaviour<Game>.Instance.Chicken != null && !SingletonMonoBehaviour<Game>.Instance.Chicken.Broken)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.EscapeTowerWithChicken);
				SingletonMonoBehaviour<AchievementManager>.Instance.NotifyEscapeTowerWithChicken();
			}
			Ending.DoEnding();
		}
	}

	public void OnHammerHit()
	{
		Animator.SetTrigger("bellybounce");
		BellyHit.Play();
	}
}
