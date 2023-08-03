using UnityEngine;

public class RandomizeStartPosition : CubeRandomizer
{
	public Coord[] PossibleCoords;

	public override void Randomize(TileMap section)
	{
		WorldObject component = GetComponent<WorldObject>();
		int num = Random.Range(0, PossibleCoords.Length + 1);
		Coord coord = (num >= PossibleCoords.Length) ? component.Coord : PossibleCoords[num];
		coord.Section = component.Coord.Section;
		base.transform.position = section.transform.TransformPoint(section.LocalPosition(coord));
		component.MoveCoord(coord);
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (PossibleCoords == null)
		{
			return;
		}
		TileMap componentInParent = GetComponentInParent<TileMap>();
		if (!(componentInParent == null))
		{
			Gizmos.color = Color.yellow;
			for (int i = 0; i < PossibleCoords.Length; i++)
			{
				Coord coord = PossibleCoords[i];
				Vector3 pos = componentInParent.transform.TransformPoint(componentInParent.LocalPosition(coord));
				Gizmos.matrix = Matrix4x4.TRS(pos, Quaternion.FromToRotation(new Vector3(1f, 1f, 1f), Vector3.forward), Vector3.one * 0.6f);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);
			}
		}
	}
}
