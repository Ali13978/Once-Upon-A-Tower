using UnityEngine;

public class Spike : WorldObject
{
	public bool EnableOnlyOnUnbreakable;

	public override void Initialize()
	{
		base.Initialize();
		if (EnableOnlyOnUnbreakable)
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Down).Normalize());
			if (tile == null || tile.Breakable != null)
			{
				base.gameObject.SetActive(value: false);
				RemoveCoord();
			}
		}
	}

	public override void OnSteppedOn(WorldObject wo)
	{
		wo.OnHit(Vector3.up, this);
		if (!wo.Broken || !(wo is Digger))
		{
			OnHit(Vector3.down, wo);
		}
	}
}
