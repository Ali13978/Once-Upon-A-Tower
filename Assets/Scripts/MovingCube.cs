using UnityEngine;

public class MovingCube : WorldObject
{
	public GameObjectTranslation Translation;

	public Vector3 Delta;

	private Vector3 startPosition;

	private Vector3 lastUnstuckPosition;

	private void Start()
	{
		lastUnstuckPosition = (startPosition = base.transform.localPosition);
	}

	private void FixedUpdate()
	{
		Delta = startPosition + Translation.AtTime(Time.fixedTime) - base.transform.localPosition;
		base.transform.localPosition += Delta;
		bool flag = false;
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section];
		Coord coordFromLocalPosition = tileMap.GetCoordFromLocalPosition(tileMap.transform.InverseTransformPoint(base.transform.position));
		if (coordFromLocalPosition != Coord)
		{
			WorldObject worldObject = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Up).Normalize());
			if (worldObject != null && worldObject.Walker == null)
			{
				worldObject = null;
			}
			Coord coord = (coordFromLocalPosition + Coord.Up).Normalize();
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(coordFromLocalPosition);
			if ((tile2 != null && tile2 != worldObject) || (worldObject != null && tile != null && tile != this))
			{
				flag = true;
				base.transform.localPosition = lastUnstuckPosition;
			}
			else if (tile2 == worldObject)
			{
				if (worldObject != null)
				{
					worldObject.MoveCoord(coord);
				}
				MoveCoord(coordFromLocalPosition);
			}
			else
			{
				MoveCoord(coordFromLocalPosition);
				if (worldObject != null)
				{
					worldObject.MoveCoord(coord);
				}
			}
		}
		if (!flag)
		{
			lastUnstuckPosition = base.transform.localPosition;
		}
	}
}
