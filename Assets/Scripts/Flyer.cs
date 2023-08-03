using UnityEngine;

public class Flyer : Movable
{
	private void FixedUpdate()
	{
		if (!initialized)
		{
			return;
		}
		Velocity.y = 0f;
		Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Wo.Coord);
		Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + Direction);
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
		Vector3 positionFromCoord2 = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
		WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + Coord.Down));
		if (tile2 is Digger || tile2 is Enemy || tile2 is Spike)
		{
			tile2 = null;
		}
		if (Direction == Vector3.zero || tile != null)
		{
			if (Direction != Vector3.zero && tile != null)
			{
				moveTo = tile;
				moveToDirection = Direction;
			}
			Direction = Vector3.zero;
			if (!trajectory.Active)
			{
				Vector3 position = base.transform.position;
				if (Mathf.Abs(position.x - positionFromCoord.x) > 0.01f)
				{
					float x = Velocity.x;
					Vector3 position2 = base.transform.position;
					if (x * (position2.x - positionFromCoord.x) > 0f)
					{
						Velocity.x = 0f;
					}
					trajectory.Setup(this, positionFromCoord);
				}
			}
			if (trajectory.Apply(this))
			{
				if (moveTo != null)
				{
					Wo.OnMoveTo(moveToDirection, moveTo);
					moveTo = null;
				}
				Vector3 position3 = base.transform.position;
				position3.x = positionFromCoord.x;
				base.transform.position = position3;
				Velocity.x = 0f;
				trajectory.Cancel(this);
			}
		}
		else
		{
			trajectory.Cancel(this);
			Velocity += Direction * Acceleration * Time.fixedDeltaTime;
		}
		Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(MoveSpeed, Mathf.Abs(Velocity.x));
		base.transform.position += Velocity * Time.fixedDeltaTime;
		Vector3 position4 = base.transform.position;
		float num = Mathf.Abs(position4.x - positionFromCoord2.x);
		Vector3 position5 = base.transform.position;
		if (num < Mathf.Abs(position5.x - positionFromCoord.x) && Direction != Vector3.zero && tile == null)
		{
			Wo.MoveCoord(coord);
		}
	}

	public override void Move(Vector3 direction)
	{
		if (!(Wo == null))
		{
			Direction = direction;
		}
	}

	public void Stop()
	{
		if (!(Wo == null))
		{
			Direction = Vector3.zero;
			moveTo = null;
		}
	}
}
