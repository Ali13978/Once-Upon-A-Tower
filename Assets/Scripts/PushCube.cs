using UnityEngine;

public class PushCube : WorldObject
{
	public WorldObject RedirectCoins;

	public AudioSource HitAudio;

	public AudioSource StopAudio;

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (direction != Vector3.left && direction != Vector3.right)
		{
			base.OnHit(direction, hitter, medium);
			return;
		}
		RedirectCoins = hitter;
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + direction);
		if (tile != null)
		{
			if (ShouldHit(tile))
			{
				tile.OnHit(direction, (!(RedirectCoins != null)) ? this : RedirectCoins, this);
			}
			if (!tile.Broken)
			{
				base.OnHit(direction, hitter, medium);
				return;
			}
		}
		Walker.Move(direction);
		if ((bool)HitAudio)
		{
			HitAudio.Play();
		}
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (ShouldHit(wo))
		{
			wo.OnHit(direction, (!(RedirectCoins != null)) ? this : RedirectCoins, this);
		}
		if (wo.Broken)
		{
			Walker.Move(direction);
		}
		else if ((bool)StopAudio)
		{
			StopAudio.Play();
		}
		return false;
	}

	private bool ShouldHit(WorldObject wo)
	{
		return wo is Enemy || wo is Coin || wo is PushCube || wo is Digger;
	}

	public override void IncrementCoins(WorldObject source, int coins)
	{
		if ((bool)RedirectCoins)
		{
			RedirectCoins.IncrementCoins(source, coins);
		}
	}

	public override void OnGrounded()
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Down).Normalize());
		if (ShouldHit(tile))
		{
			tile.OnHit(Vector3.down, (!(RedirectCoins != null)) ? this : RedirectCoins, this);
		}
	}

	public override void Disarm()
	{
		Breakable.Break(Vector3.up);
	}
}
