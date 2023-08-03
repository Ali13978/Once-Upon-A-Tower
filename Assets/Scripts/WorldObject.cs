using UnityEngine;

public class WorldObject : MonoBehaviour
{
	public Coord Coord;

	public Walker Walker;

	public Projectile Projectile;

	public Breakable Breakable;

	public BoxCollider Collider;

	public Animator Animator;

	public TileParts[] TileParts;

	public string PrefabId;

	public int Coins;

	public bool Hermetic;

	public bool Slippery;

	public bool IsTrigger;

	public Transform FlipContent;

	public bool DebugLog;

	public bool IncludeInEditor = true;

	[HideInInspector]
	public bool Disarmed;

	public WorldObjectType WorldObjectType;

	public bool Outleft;

	public bool Outright;

	public Bounds Bounds => new Bounds(base.transform.position + Collider.center, Vector3.Scale(Collider.size, base.transform.lossyScale));

	public bool Broken => Breakable != null && Breakable.Broken;

	public bool OnScreen
	{
		get
		{
			Camera camera = SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
			Camera camera2 = camera;
			Vector3 position = camera.transform.position;
			Vector3 vector = camera2.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f - position.z));
			Camera camera3 = camera;
			Vector3 position2 = camera.transform.position;
			Vector3 vector2 = camera3.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f - position2.z));
			Vector3 position3 = base.transform.position;
			int result;
			if (position3.y < vector.y + 0.4f)
			{
				Vector3 position4 = base.transform.position;
				result = ((position4.y > vector2.y - 0.4f) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public virtual void OnSteppedOn(WorldObject stepper)
	{
	}

	public virtual void OnGrounded()
	{
	}

	public virtual void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if ((bool)Breakable)
		{
			Breakable.Break(direction, hitter);
			if (Coins > 0 && hitter != null)
			{
				hitter.IncrementCoins(this, Coins);
			}
		}
	}

	public virtual bool BreakableBy(WorldObject hitter)
	{
		return Breakable != null;
	}

	public virtual void Initialize()
	{
		if (Walker != null)
		{
			Walker.Initialize();
		}
	}

	public virtual void Flip()
	{
	}

	public virtual bool CanMoveTo(Coord newCoord, WorldObject tile)
	{
		return tile == null;
	}

	public virtual void MoveCoord(Coord newCoord)
	{
		SingletonMonoBehaviour<World>.Instance.Move(this, newCoord);
		Coord = newCoord;
		TileMap componentInParent = base.transform.GetComponentInParent<TileMap>();
		if (componentInParent != null && componentInParent.Index != Coord.Section)
		{
			if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(Coord))
			{
				UnityEngine.Debug.LogError("Trying to move to a Section that doesn't exist", this);
			}
			else if (!SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section].IsLoadingSection)
			{
				base.transform.parent = SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section].transform;
			}
		}
	}

	public virtual bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		return false;
	}

	public virtual void StoppedBeforeFall()
	{
	}

	public void DisableWithComponents()
	{
		base.enabled = false;
		if ((bool)Walker)
		{
			Walker.enabled = false;
		}
		if ((bool)Projectile)
		{
			Projectile.enabled = false;
		}
	}

	public void EnableWithComponents()
	{
		base.enabled = true;
		if ((bool)Walker)
		{
			Walker.enabled = true;
		}
		if ((bool)Projectile)
		{
			Projectile.enabled = true;
		}
	}

	public virtual void IncrementCoins(WorldObject source, int coins)
	{
		Coins += coins;
	}

	public virtual void RemoveCoord()
	{
		if (Coord.Section < 0 || Coord.Section >= SingletonMonoBehaviour<World>.Instance.Sections.Count)
		{
			return;
		}
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section];
		if (tileMap != null)
		{
			if (tileMap.GetTile(Coord) != this)
			{
				UnityEngine.Debug.LogWarning("Trying to remove WorldObject not in the matrix", this);
			}
			else
			{
				tileMap.Remove(Coord);
			}
		}
	}

	public virtual void Disarm()
	{
		Disarmed = true;
	}
}
