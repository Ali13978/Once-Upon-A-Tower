using System;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	public enum AlignmentType
	{
		None = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,
		RightTop = 6,
		LeftTop = 5,
		RightBottom = 10,
		LeftBottom = 9
	}

	[Serializable]
	public class Feature
	{
		public GameObject Object;

		public int Columns = 1;

		public int Rows = 1;

		public int PoolSize = 1;

		public bool UseWhenLoading = true;

		public AlignmentType AlignmentType;

		public float Weight = 1f;

		private string name;

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					name = Object.name;
				}
				return name;
			}
		}
	}

	public Feature[] Features;

	public float FeatureChance = 0.1f;

	public GameObject Wall;

	public int PoolSize = 1;

	private int maxRows;

	private int maxColumns;

	private int[] emptyRowsPerColumn;

	private List<Feature> fitFeatures;

	private GameObject placeHolder;

	public void Init()
	{
		for (int i = 0; i < Features.Length; i++)
		{
			if (Features[i].Rows > maxRows)
			{
				maxRows = Features[i].Rows;
			}
			if (Features[i].Columns > maxColumns)
			{
				maxColumns = Features[i].Columns;
			}
		}
		emptyRowsPerColumn = new int[maxColumns];
		fitFeatures = new List<Feature>(Features.Length);
		placeHolder = SingletonMonoBehaviour<Game>.Instance.gameObject;
	}

	public Coord Setup(Coord coord, Coord lastFeature)
	{
		if (!SetupFeature(coord, ref lastFeature))
		{
			SetupWall(coord);
		}
		return lastFeature;
	}

	private bool SetupFeature(Coord coord, ref Coord lastFeature)
	{
		if (coord.Section == 0)
		{
			return false;
		}
		if (lastFeature.Section >= 0 && SingletonMonoBehaviour<World>.Instance.GetSection(lastFeature.Section) != null)
		{
			Coord coord2 = coord - lastFeature;
			if (coord2.x < 2 && coord2.y < 1)
			{
				return false;
			}
			if (coord2.y < 5 && FeatureChance <= UnityEngine.Random.Range(0f, 1f))
			{
				return false;
			}
		}
		else if (FeatureChance <= UnityEngine.Random.Range(0f, 1f))
		{
			return false;
		}
		for (int i = 0; i < maxColumns; i++)
		{
			emptyRowsPerColumn[i] = 0;
		}
		for (int j = 0; j < maxRows; j++)
		{
			bool flag = false;
			for (int k = 0; k < maxColumns; k++)
			{
				if (flag)
				{
					continue;
				}
				Coord c = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + new Coord(k, j));
				if (IsEmpty(c))
				{
					if (emptyRowsPerColumn[k] == j)
					{
						emptyRowsPerColumn[k] = j + 1;
					}
				}
				else
				{
					flag = true;
				}
			}
		}
		float num = 0f;
		fitFeatures.Clear();
		for (int l = 0; l < Features.Length; l++)
		{
			if (Fit(coord, Features[l], emptyRowsPerColumn) && SingletonMonoBehaviour<Pool>.Instance.ContainsObjects(Features[l].Name))
			{
				fitFeatures.Add(Features[l]);
				num += (float)(Features[l].Rows * Features[l].Columns) * Features[l].Weight;
			}
		}
		if (fitFeatures.Count == 0)
		{
			return false;
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		Feature feature = null;
		for (int m = 0; m < fitFeatures.Count; m++)
		{
			num2 -= (float)(fitFeatures[m].Rows * fitFeatures[m].Columns) * fitFeatures[m].Weight;
			if (num2 <= 0f)
			{
				feature = fitFeatures[m];
				break;
			}
		}
		if (feature == null)
		{
			feature = fitFeatures[fitFeatures.Count - 1];
		}
		GameObject gameObject = Setup(coord, feature.Object, feature.Name);
		for (int n = 0; n < feature.Rows; n++)
		{
			for (int num3 = 0; num3 < feature.Columns; num3++)
			{
				Coord a = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + new Coord(num3, n));
				TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(a.Section);
				if (section != null && section.BackgroundMatrix != null)
				{
					section.BackgroundMatrix[a.x, a.y] = ((!(a == coord)) ? placeHolder : gameObject);
				}
			}
		}
		lastFeature = coord + new Coord(feature.Columns - 1, feature.Rows - 1);
		return true;
	}

	private bool Fit(Coord coord, Feature feature, int[] emptyRowsPerColumn)
	{
		if (SingletonMonoBehaviour<World>.Instance.Sections[coord.Section].IsLoadingSection && !feature.UseWhenLoading)
		{
			return false;
		}
		if (emptyRowsPerColumn[feature.Columns - 1] < feature.Rows)
		{
			return false;
		}
		if (feature.AlignmentType != 0)
		{
			for (int i = 0; i < feature.Rows; i++)
			{
				if ((feature.AlignmentType & AlignmentType.Left) != 0 && !IsBorder(coord, new Coord(-1, i)))
				{
					return false;
				}
				if ((feature.AlignmentType & AlignmentType.Right) != 0 && !IsBorder(coord, new Coord(feature.Columns, i)))
				{
					return false;
				}
			}
			for (int j = 0; j < feature.Columns; j++)
			{
				if ((feature.AlignmentType & AlignmentType.Bottom) != 0 && !IsBorder(coord, new Coord(j, feature.Rows)))
				{
					return false;
				}
				if ((feature.AlignmentType & AlignmentType.Top) != 0 && !IsBorder(coord, new Coord(j, -1)))
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool IsBorder(Coord coord, Coord diff)
	{
		Coord coord2 = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + diff);
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
		return tile != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, tile.PrefabId) >= 0;
	}

	private bool IsEmpty(Coord c)
	{
		if (SingletonMonoBehaviour<World>.Instance.GetSection(c.Section) == null)
		{
			return false;
		}
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(c);
		if (tile != null && tile.Hermetic)
		{
			return false;
		}
		TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(c.Section);
		if (section != null && section.BackgroundMatrix[c.x, c.y] != null)
		{
			return false;
		}
		if (tile != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, tile.PrefabId) >= 0)
		{
			return false;
		}
		return true;
	}

	private void SetupWall(Coord coord)
	{
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections[coord.Section];
		if (!(tileMap.BackgroundMatrix[coord.x, coord.y] != null))
		{
			tileMap.BackgroundMatrix[coord.x, coord.y] = Setup(coord, Wall, "Wall");
		}
	}

	private GameObject Setup(Coord coord, GameObject go, string name)
	{
		if (go == null)
		{
			UnityEngine.Debug.LogError("Background.Setup called with null go");
			return null;
		}
		TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(coord.Section);
		if (section == null)
		{
			UnityEngine.Debug.LogError("Background.Setup called with invalid section");
			return null;
		}
		GameObject gameObject = SingletonMonoBehaviour<Pool>.Instance.Alloc(name);
		gameObject.SetActive(value: true);
		gameObject.transform.parent = section.transform;
		Vector3 localPosition = section.LocalPosition(coord);
		Vector3 localPosition2 = gameObject.transform.localPosition;
		localPosition.z = localPosition2.z;
		gameObject.transform.localPosition = localPosition;
		return gameObject;
	}

	public void Free(Coord coord)
	{
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections[coord.Section];
		if (tileMap == null)
		{
			UnityEngine.Debug.LogError("Trying to free background tile from deleted section");
			return;
		}
		GameObject gameObject = tileMap.BackgroundMatrix[coord.x, coord.y];
		if (gameObject != placeHolder)
		{
			SingletonMonoBehaviour<Pool>.Instance.Free(gameObject);
		}
		tileMap.BackgroundMatrix[coord.x, coord.y] = null;
	}
}
