using System;
using System.Collections;
using UnityEngine;

public class Dragon : WorldObject
{
	private class AttackSpot
	{
		public WorldObject AttackTile;

		public Vector3 Direction;
	}

	public float FireDelay = 0.01f;

	public float AttackDelayTime = 1f;

	public ParticleSystem FireEffect;

	public ParticleSystem FireExplosionEffect;

	public AudioSource Sleeps;

	public AudioSource Fire;

	public AudioSource Wakes;

	public AudioSource Land;

	public AudioSource Flyby;

	public AudioSource Launch;

	public Renderer Renderer;

	public ParticleSystem[] SleepParticles;

	private Coord lastCoord;

	private AttackSpot attackSpot;

	private Vector3 lastAttackDir = Vector3.right;

	private bool awake;

	private float lastTimeOnAttackSpot;

	private Coord lastCoordOnAttackSpot;

	private ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[3];

	private void Start()
	{
		Animator = GetComponent<Animator>();
		Game instance = SingletonMonoBehaviour<Game>.Instance;
		instance.OnNoise = (Action<Noise>)Delegate.Combine(instance.OnNoise, (Action<Noise>)delegate
		{
			awake = true;
		});
	}

	private void FixedUpdate()
	{
		if (attackSpot != null && SingletonMonoBehaviour<Game>.Instance.Digger.Coord.y == attackSpot.AttackTile.Coord.y)
		{
			lastTimeOnAttackSpot = Time.time;
			lastCoordOnAttackSpot = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
		}
	}

	private void OnEnable()
	{
		StartCoroutine(DragonRoutine());
	}

	private IEnumerator DragonRoutine()
	{
		while (!SingletonMonoBehaviour<World>.Instance.Ready)
		{
			yield return null;
		}
		if (SaveGame.Instance.WorldLevel == 1 && SingletonMonoBehaviour<World>.Instance.Sections[0] != null)
		{
			base.transform.parent.position = SingletonMonoBehaviour<World>.Instance.Sections[0].transform.position;
		}
		else
		{
			awake = true;
		}
		while (!awake)
		{
			yield return null;
		}
		PlayNostrilParticles();
		Sleeps.Stop();
		Wakes.Play();
		Animator.SetBool("awake", value: true);
		yield return new WaitForSeconds(5f);
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		while (true)
		{
			Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
			if (!DiggerWasSaved() && digger.Walker.IsGrounded)
			{
				AttackSpot spot = FindAttack(SingletonMonoBehaviour<Game>.Instance.Digger);
				if (spot != null)
				{
					yield return AttackRoutine(spot);
				}
			}
			yield return wait;
		}
	}

	private AttackSpot FindAttack(WorldObject target)
	{
		if (lastAttackDir == Vector3.right)
		{
			return FindAttack(target, Vector3.right) ?? FindAttack(target, Vector3.left);
		}
		return FindAttack(target, Vector3.left) ?? FindAttack(target, Vector3.right);
	}

	private AttackSpot FindAttack(WorldObject target, Vector3 dir)
	{
		Coord coord = target.Coord + dir;
		WorldObject worldObject = null;
		if (SingletonMonoBehaviour<World>.Instance.Sections.Count <= coord.Section)
		{
			return null;
		}
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections[coord.Section];
		while (coord.x >= 0 && coord.x < tileMap.Columns)
		{
			WorldObject tile = tileMap.GetTile(coord);
			if (tile != null)
			{
				if (tile.Hermetic)
				{
					return null;
				}
				worldObject = tile;
			}
			coord += dir;
		}
		if (worldObject == null)
		{
			return null;
		}
		for (int i = 1; i <= 3; i++)
		{
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(worldObject.Coord + Vector3.down * i));
			if (tile2 == null)
			{
				return null;
			}
			WorldObject tile3 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(worldObject.Coord + (Vector3.down * i + dir)));
			if (tile3 != null && i != 1)
			{
				return null;
			}
		}
		AttackSpot attackSpot = new AttackSpot();
		attackSpot.AttackTile = worldObject;
		attackSpot.Direction = -dir;
		return attackSpot;
	}

	private IEnumerator AttackRoutine(AttackSpot spot)
	{
		if (SaveGame.Instance.WorldLevel < 7)
		{
			yield return new WaitForSeconds(AttackDelayTime);
		}
		if (spot.AttackTile == null || !SingletonMonoBehaviour<World>.Instance.IsCoordValid(spot.AttackTile.Coord))
		{
			attackSpot = null;
			yield break;
		}
		Coord diff = SingletonMonoBehaviour<Game>.Instance.Digger.Coord - spot.AttackTile.Coord;
		if (diff == Coord.None || diff.y != 0)
		{
			attackSpot = null;
			yield break;
		}
		lastAttackDir = spot.Direction;
		base.transform.position = spot.AttackTile.transform.position;
		Animator.SetTrigger((!(spot.Direction == Vector3.right)) ? "landingRight" : "landingLeft");
		Flyby.Play();
		attackSpot = spot;
		float startTime = Time.time;
		while (Time.time - startTime < 7.5f && attackSpot != null)
		{
			yield return null;
		}
		attackSpot = null;
	}

	private void AttackEvent()
	{
		if (!DiggerWasSaved())
		{
			StartCoroutine(FireCoroutine());
			Fire.Play();
		}
	}

	private bool DiggerWasSaved()
	{
		Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
		int result;
		if (SingletonMonoBehaviour<World>.Instance.GetSection(digger.SavedCoord.Section) != null)
		{
			Coord coord = digger.Coord - digger.SavedCoord;
			result = ((coord.y <= 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void LandEvent()
	{
		Land.Play();
	}

	private void ShakeEvent()
	{
		if (!Renderer || Renderer.isVisible)
		{
			float num = 0.5f;
			if (attackSpot != null && attackSpot.Direction.x > 0f)
			{
				num = 0f - num;
			}
			SingletonMonoBehaviour<Game>.Instance.GameCamera.Shake(num);
		}
	}

	private void LaunchEvent()
	{
		Launch.Play();
		PlayNostrilParticles();
	}

	private void CanAbortAttack()
	{
		if (attackSpot != null && SingletonMonoBehaviour<Game>.Instance.Digger != null && attackSpot.AttackTile != null)
		{
			Coord coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord - attackSpot.AttackTile.Coord;
			if (coord.y != 0 && !Gui.TV)
			{
				attackSpot = null;
				Animator.SetTrigger("abort");
			}
		}
	}

	private IEnumerator FireCoroutine()
	{
		Coord coord = attackSpot.AttackTile.Coord;
		Vector3 position;
		while (true)
		{
			if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord))
			{
				yield break;
			}
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
			FireEffect.transform.position = position;
			if (tile != null)
			{
				tile.OnHit(attackSpot.Direction, this);
				if (tile.Broken)
				{
					tile.Breakable.LightFire();
				}
				if (tile.Hermetic && !tile.Broken)
				{
					break;
				}
				EmitParticles();
			}
			else
			{
				EmitParticles();
			}
			CheckNearMissDragonFire(coord);
			yield return new WaitForSeconds(FireDelay);
			if (attackSpot == null)
			{
				yield break;
			}
			coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + attackSpot.Direction);
		}
		FireExplosionEffect.transform.position = position + Vector3.back * 4f - attackSpot.Direction * 0.5f;
		FireExplosionEffect.Play();
	}

	private void CheckNearMissDragonFire(Coord coord)
	{
		if (coord == lastCoordOnAttackSpot && SingletonMonoBehaviour<Game>.Instance.Digger.Coord.y != coord.y && Time.time - lastTimeOnAttackSpot < 0.5f && !SingletonMonoBehaviour<Game>.Instance.Digger.IsDead)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.NearMissDragonFire);
		}
	}

	private void EmitParticles()
	{
		ParticleSystem[] componentsInChildren = FireEffect.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			var _temp_val_287 = particleSystem.emission; _temp_val_287.GetBursts(bursts);
			particleSystem.Emit(bursts[0].maxCount);
		}
	}

	private void PlayNostrilParticles()
	{
		for (int i = 0; i < SleepParticles.Length; i++)
		{
			SleepParticles[i].Play();
		}
	}

	private void DragonSleepStart()
	{
		PlayNostrilParticles();
		if (!awake && (!Sleeps.isPlaying || Sleeps.time > 3f))
		{
			Sleeps.Stop();
			Sleeps.Play();
		}
	}
}
