using UnityEngine;

public class RandomizeEnemies : CubeRandomizer
{
	public GameObject[] Enemies;

	public Coord[] Positions;

	public int Count;

	public override void Randomize(TileMap section)
	{
		for (int i = 0; i < Count; i++)
		{
			Coord coord = Coord.None;
			for (int j = 0; j < 10; j++)
			{
				coord = Positions[Random.Range(0, Positions.Length)];
				if (section.GetTile(coord) == null)
				{
					break;
				}
			}
			if (!(section.GetTile(coord) != null))
			{
				GameObject prefab = Enemies[Random.Range(0, Enemies.Length)];
				section.InstantiateFromPrefab(prefab, coord);
			}
		}
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Positions == null)
		{
			return;
		}
		TileMap componentInParent = GetComponentInParent<TileMap>();
		if (!(componentInParent == null))
		{
			Gizmos.color = Color.red;
			for (int i = 0; i < Positions.Length; i++)
			{
				Coord coord = Positions[i];
				Vector3 pos = componentInParent.transform.TransformPoint(componentInParent.LocalPosition(coord));
				Gizmos.matrix = Matrix4x4.TRS(pos, Quaternion.FromToRotation(new Vector3(1f, 1f, 1f), Vector3.forward), Vector3.one * 0.6f);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);
			}
		}
	}
}
