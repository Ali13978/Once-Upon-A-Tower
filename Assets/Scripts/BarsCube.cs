using UnityEngine;

public class BarsCube : WorldObject
{
	public GameObject[] LightRays;

	private const int MaxRange = 9;

	public override void Initialize()
	{
		Vector3[] array = new Vector3[2]
		{
			Vector3.left,
			Vector3.right
		};
		for (int i = 0; i < array.Length; i++)
		{
			LightRays[i].SetActive(value: false);
			for (int j = 1; j < 9; j++)
			{
				Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + j * array[i]);
				WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
				if (tile != null)
				{
					LightRays[i].SetActive(value: true);
					break;
				}
			}
		}
	}
}
