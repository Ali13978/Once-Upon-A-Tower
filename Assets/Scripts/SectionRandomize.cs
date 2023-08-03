using System;
using System.Collections.Generic;
using UnityEngine;

public class SectionRandomize : MonoBehaviour
{
	private struct CubeWeight
	{
		public string Name;

		public float Weight;

		public CubeWeight(string name, float weight)
		{
			Name = name;
			Weight = weight;
		}
	}

	public bool RandomizeFirst;

	public bool RandomizeMid;

	public bool RandomizeLast = true;

	public bool AllowLastUnbreakables = true;

	public bool FillLast;

	private List<CubeWeight> cubeWeights = new List<CubeWeight>(6);

	public void Randomize(float emptyWeight)
	{
		TileMap component = GetComponent<TileMap>();
		if (RandomizeFirst)
		{
			Randomize(0, fill: false, allowUnbreakable: false, emptyWeight);
		}
		if (RandomizeMid)
		{
			for (int i = 1; i < component.Rows - 1; i++)
			{
				Randomize(i, fill: false, allowUnbreakable: false, emptyWeight);
			}
		}
		if (RandomizeLast)
		{
			Randomize(component.Rows - 1, FillLast, AllowLastUnbreakables, emptyWeight);
		}
	}

	private void Randomize(int row, bool fill, bool allowUnbreakable, float emptyWeight)
	{
		TileMap component = GetComponent<TileMap>();
		component.CreateMatrix();
		int i;
		for (i = 0; i < component.Columns && component.TileMatrix[i, row] == null; i++)
		{
		}
		for (; i < component.Columns && component.TileMatrix[i, row] != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, component.TileMatrix[i, row].PrefabId) != -1; i++)
		{
		}
		int num = component.Columns - 1;
		while (num >= 0 && component.TileMatrix[num, row] == null)
		{
			num--;
		}
		while (num >= 0 && component.TileMatrix[num, row] != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, component.TileMatrix[num, row].PrefabId) != -1)
		{
			num--;
		}
		int num2 = num - i + 1;
		if (num2 <= 0)
		{
			return;
		}
		if (fill)
		{
			emptyWeight = 0f;
		}
		cubeWeights.Clear();
		cubeWeights.Add(new CubeWeight(string.Empty, emptyWeight));
		cubeWeights.Add(new CubeWeight("Cube", 1f));
		cubeWeights.Add(new CubeWeight("UnbreakableCube", allowUnbreakable ? 1 : 0));
		cubeWeights.Add(new CubeWeight("CrackedCube", (!fill) ? 0.05f : 0f));
		cubeWeights.Add(new CubeWeight("PushCube", (!fill) ? 0.08f : 0f));
		float num3 = 0f;
		for (int j = 0; j < cubeWeights.Count; j++)
		{
			float num4 = num3;
			CubeWeight cubeWeight = cubeWeights[j];
			num3 = num4 + cubeWeight.Weight;
		}
		string[] array = new string[num2];
		bool flag = true;
		for (int k = 0; k < num2; k++)
		{
			float num5 = UnityEngine.Random.Range(0f, num3);
			for (int l = 0; l < cubeWeights.Count; l++)
			{
				CubeWeight cubeWeight2 = cubeWeights[l];
				if (cubeWeight2.Weight != 0f)
				{
					string[] array2 = array;
					int num6 = k;
					CubeWeight cubeWeight3 = cubeWeights[l];
					array2[num6] = cubeWeight3.Name;
					float num7 = num5;
					CubeWeight cubeWeight4 = cubeWeights[l];
					num5 = num7 - cubeWeight4.Weight;
					if (num5 <= 0f)
					{
						break;
					}
				}
			}
			Coord coord = new Coord(component.Index, i + k, row);
			if (array[k] == string.Empty && EmptyHeight(coord) >= 7)
			{
				array[k] = "Cube";
			}
			if (array[k] != "UnbreakableCube" && component.TileMatrix[coord.x, coord.y] == null)
			{
				flag = false;
			}
		}
		if (flag)
		{
			for (int m = 0; m < num2; m++)
			{
				array[m] = "Cube";
			}
		}
		for (int n = 0; n < num2; n++)
		{
			Coord tilePos = new Coord(component.Index, i + n, row);
			if (!(component.TileMatrix[tilePos.x, tilePos.y] != null))
			{
				string text = array[n];
				if (!string.IsNullOrEmpty(text))
				{
					component.InstantiateFromPrefab(text, tilePos);
				}
			}
		}
	}

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, Quaternion.FromToRotation(new Vector3(1f, 1f, 1f), Vector3.forward), Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);
	}

	private int EmptyHeight(Coord coord)
	{
		int i;
		for (i = 0; i <= 10; i++)
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((coord + Vector3.up * i).Normalize());
			if (tile != null && tile.Hermetic)
			{
				break;
			}
		}
		return i;
	}
}
