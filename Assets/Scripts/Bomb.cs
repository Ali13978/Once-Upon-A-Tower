using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : WorldObject
{
	public float FireDelay = 0.1f;

	public int MaxRange = 5;

	public ParticleSystem FireEffect;

	private WorldObject activator;

	private List<Coord> exploded = new List<Coord>();

	private bool hitDragon;

	private ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[3];

	public void Activate(WorldObject activator)
	{
		this.activator = activator;
		GetComponent<Collider>().enabled = false;
		StartCoroutine(ExplodeCoroutine());
	}

	private IEnumerator ExplodeCoroutine()
	{
		GetComponentInChildren<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		yield return StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(0.2f, 1f, 0.2f));
		hitDragon = false;
		exploded.Clear();
		exploded.Add(SingletonMonoBehaviour<Game>.Instance.Digger.Coord);
		StartCoroutine(FireCoroutine(Coord, Vector3.up));
		StartCoroutine(FireCoroutine(Coord, Vector3.down));
		StartCoroutine(FireCoroutine(Coord, Vector3.left));
		StartCoroutine(FireCoroutine(Coord, Vector3.right));
		yield return new WaitForSeconds(0.05f);
		StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(1f, 1f, 0.4f));
	}

	private IEnumerator FireCoroutine(Coord coord, Vector3 direction, int range = 0)
	{
		Coord newCoord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + direction);
		if (exploded.Contains(newCoord) || range > MaxRange || !SingletonMonoBehaviour<World>.Instance.IsCoordValid(newCoord))
		{
			yield break;
		}
		exploded.Add(newCoord);
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(newCoord);
		Vector3 position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(newCoord);
		FireEffect.transform.position = position;
		if (tile != null && tile != SingletonMonoBehaviour<Game>.Instance.Digger && !(tile is StoreCube) && !hitDragon)
		{
			if (tile is FinalDragonHitBox)
			{
				hitDragon = true;
			}
			tile.OnHit(direction, activator);
			if (tile.Broken)
			{
				tile.Breakable.LightFire();
				EmitParticles();
				if (tile is Enemy)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillEnemiesWithBombs);
				}
			}
		}
		else
		{
			EmitParticles();
		}
		if (tile == null || !tile.Hermetic)
		{
			yield return new WaitForSeconds(FireDelay);
			int newRange = range + 1;
			StartCoroutine(FireCoroutine(newCoord, Vector3.up, newRange));
			StartCoroutine(FireCoroutine(newCoord, Vector3.down, newRange));
			StartCoroutine(FireCoroutine(newCoord, Vector3.left, newRange));
			StartCoroutine(FireCoroutine(newCoord, Vector3.right, newRange));
		}
	}

	private void EmitParticles()
	{
		ParticleSystem[] componentsInChildren = FireEffect.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			var _temp_val_161 = particleSystem.emission; _temp_val_161.GetBursts(bursts);
			particleSystem.Emit(bursts[0].maxCount);
		}
	}
}
