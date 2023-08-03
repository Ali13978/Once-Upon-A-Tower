using System.Collections;
using UnityEngine;

public class FallingSpikeCube : WorldObject
{
	public int MaxRange = 25;

	public float FallAccelerationMagnitude = 25f;

	public float FallDelay = 0.6f;

	private bool falling;

	private void FixedUpdate()
	{
		if (base.Broken || falling)
		{
			return;
		}
		for (int i = 1; i < MaxRange; i++)
		{
			Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + new Coord(0, i));
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			if (tile is Digger || tile is Enemy)
			{
				break;
			}
			if (tile != null)
			{
				return;
			}
		}
		StartCoroutine(TrembleAndFallCoroutine());
	}

	private IEnumerator TrembleAndFallCoroutine()
	{
		falling = true;
		yield return new WaitForSeconds(FallDelay);
		Projectile.Move(FallAccelerationMagnitude * Vector3.down, Vector3.zero);
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		base.OnHit(direction, hitter, medium);
		Projectile.Stop();
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (wo is Digger || wo is Enemy || wo is Chicken)
		{
			wo.OnHit(direction, this);
			wo.RemoveCoord();
			return true;
		}
		Breakable.Break(direction);
		return false;
	}
}
