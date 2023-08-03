using UnityEngine;

public class FinalDragonHitBox : WorldObject
{
	public FinalDragon Dragon;

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		Dragon.OnHit(direction, hitter);
	}

	private void FixedUpdate()
	{
		if (Dragon.HitPoints <= 0f && SingletonMonoBehaviour<World>.Instance.GetTile(Coord) == this)
		{
			RemoveCoord();
		}
	}
}
