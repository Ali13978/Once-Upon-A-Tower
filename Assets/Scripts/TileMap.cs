using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileMap : MonoBehaviour
{
	public bool Initialized;

	public int Level;

	public int Rows;

	public int Columns;

	public float TileWidth = 1f;

	public float TileHeight = 1f;

	public Transform StartPosition;

	public bool Flippable = true;

	[HideInInspector]
	public bool Flipped;

	public bool ShowGizmos = true;

	[HideInInspector]
	public int Index = -1;

	[HideInInspector]
	public Vector3 MarkerPosition;

	public WorldObject[,] TileMatrix;

	public GameObject[,] BackgroundMatrix;

	public GameObject Background;

	[HideInInspector]
	public TileMap PreviousMap;

	[HideInInspector]
	public TileMap NextMap;

	public bool IsTutorial;

	public bool IsLoadingSection;

	public bool IsTransition;

	public bool DisableCoins;

	public Coord LastFeature;

	public bool HasBackground;

	public bool DontDestroy;

	public bool IsEnd;

	public Transform TilesParent;

	private Dictionary<string, List<string>> sideMatch = new Dictionary<string, List<string>>
	{
		{
			"95a58654e82064870a525b458a9cb360",
			new List<string>
			{
				"181258e44f8384f19b56428f16ded3bf"
			}
		},
		{
			"181258e44f8384f19b56428f16ded3bf",
			new List<string>
			{
				"95a58654e82064870a525b458a9cb360"
			}
		}
	};

	private void Awake()
	{
		if (TilesParent == null)
		{
			TilesParent = base.transform.Find("Tiles");
		}
	}

	private void Start()
	{
		if (Index < 0)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void InitTileCoords()
	{
		if (Index < 0 && Application.isPlaying)
		{
			UnityEngine.Debug.LogError("Initializing tile coords without index");
		}
		WorldObject[] componentsInChildren = GetComponentsInChildren<WorldObject>(includeInactive: true);
		foreach (WorldObject worldObject in componentsInChildren)
		{
			if (!(worldObject is Dragon))
			{
				worldObject.Coord = GetCoordFromLocalPosition(base.transform.InverseTransformPoint(worldObject.transform.position));
			}
		}
	}

	public void CreateMatrix()
	{
		TileMatrix = new WorldObject[Columns, Rows];
		WorldObject[] componentsInChildren = GetComponentsInChildren<WorldObject>();
		foreach (WorldObject worldObject in componentsInChildren)
		{
			if (!worldObject.IsTrigger)
			{
				TileMatrix[worldObject.Coord.x, worldObject.Coord.y] = worldObject;
			}
		}
		BackgroundMatrix = new GameObject[Columns, Rows];
	}

	public void SetupBackground()
	{
		if (TileMatrix == null || Background == null || HasBackground)
		{
			return;
		}
		Background component = Background.GetComponent<Background>();
		component.Init();
		HasBackground = true;
		Background.Feature[] features = component.Features;
		foreach (Background.Feature feature in features)
		{
			if (!SingletonMonoBehaviour<Pool>.Instance.ContainsKey(feature.Object.name))
			{
				SingletonMonoBehaviour<Pool>.Instance.WarmUp(feature.Object.name, feature.Object, feature.PoolSize);
			}
		}
		if (!SingletonMonoBehaviour<Pool>.Instance.ContainsKey(component.Wall.name))
		{
			SingletonMonoBehaviour<Pool>.Instance.WarmUp(component.Wall.name, component.Wall, component.PoolSize);
		}
		LastFeature = Coord.None;
		if (PreviousMap != null)
		{
			LastFeature = PreviousMap.LastFeature;
		}
		string[] borderIds = SingletonMonoBehaviour<World>.Instance.BorderIds;
		for (int j = 0; j < Rows; j++)
		{
			if (Index != 0 || j != 0)
			{
				int k;
				for (k = 0; k < Columns && TileMatrix[k, j] == null; k++)
				{
				}
				for (; k < Columns && TileMatrix[k, j] != null && borderIds.Contains(TileMatrix[k, j].PrefabId); k++)
				{
				}
				int num = Columns - 1;
				while (num >= 0 && TileMatrix[num, j] == null)
				{
					num--;
				}
				while (num >= 0 && TileMatrix[num, j] != null && borderIds.Contains(TileMatrix[num, j].PrefabId))
				{
					num--;
				}
				for (int l = k; l <= num; l++)
				{
					LastFeature = component.Setup(new Coord(Index, l, j), LastFeature);
				}
			}
		}
	}

	public void FreeBackground()
	{
		if (TileMatrix == null || Background == null)
		{
			return;
		}
		Background component = Background.GetComponent<Background>();
		HasBackground = false;
		if (base.gameObject.activeSelf)
		{
			UnityEngine.Debug.LogWarning("Releasing active object");
		}
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Columns; j++)
			{
				component.Free(new Coord(Index, j, i));
			}
		}
	}

	public void FreeCoins()
	{
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Columns; j++)
			{
				WorldObject worldObject = TileMatrix[j, i];
				if (worldObject is Coin)
				{
					SingletonMonoBehaviour<Pool>.Instance.Free(worldObject.gameObject);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!ShowGizmos)
		{
			return;
		}
		float x = (float)Columns * TileWidth;
		float num = (float)Rows * TileHeight;
		Vector3 position = base.transform.position;
		Gizmos.color = Color.white;
		Gizmos.DrawLine(position, position + new Vector3(x, 0f, 0f));
		Gizmos.DrawLine(position, position + new Vector3(0f, 0f - num, 0f));
		Gizmos.DrawLine(position + new Vector3(x, 0f, 0f), position + new Vector3(x, 0f - num, 0f));
		Gizmos.DrawLine(position + new Vector3(0f, 0f - num, 0f), position + new Vector3(x, 0f - num, 0f));
		Gizmos.color = Color.grey;
		for (float num2 = 1f; num2 < (float)Columns; num2 += 1f)
		{
			Gizmos.DrawLine(position + new Vector3(num2 * TileWidth, 0f, 0f), position + new Vector3(num2 * TileWidth, 0f - num, 0f));
		}
		for (float num3 = 1f; num3 < (float)Rows; num3 += 1f)
		{
			Gizmos.DrawLine(position + new Vector3(0f, (0f - num3) * TileHeight, 0f), position + new Vector3(x, (0f - num3) * TileHeight, 0f));
		}
		if (TileMatrix != null)
		{
			Gizmos.color = Color.green * 0.5f;
			for (int i = 0; i < TileMatrix.GetLength(1); i++)
			{
				for (int j = 0; j < TileMatrix.GetLength(0); j++)
				{
					if (TileMatrix[j, i] != null)
					{
						Gizmos.DrawWireCube(base.transform.position + LocalPosition(new Coord(j, i)), new Vector3(TileWidth, TileHeight, 1f) * 0.9f);
					}
				}
			}
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(MarkerPosition, new Vector3(TileWidth, TileHeight, 1f) * 1.1f);
	}

	public Coord GetCoordFromLocalPosition(Vector3 position)
	{
		float x = position.x / TileWidth;
		float y = (0f - position.y) / TileHeight;
		Vector3 position2 = base.transform.position;
		Vector3 vector = new Vector3(x, y, position2.z);
		return new Coord(Index, (int)Math.Round(vector.x, 5, MidpointRounding.ToEven), (int)Math.Round(vector.y, 5, MidpointRounding.ToEven));
	}

	public void Remove(Coord coord)
	{
		TileMatrix[coord.x, coord.y] = null;
		UpdateSidesAround(coord);
	}

	public void UpdateSidesAround(Coord coord)
	{
		GetTilesAround(coord, out WorldObject left, out WorldObject right, out WorldObject down, out WorldObject top);
		UpdateSides(left);
		UpdateSides(right);
		UpdateSides(down);
		UpdateSides(top);
	}

	public void GetTilesAround(Coord coord, out WorldObject left, out WorldObject right, out WorldObject down, out WorldObject top)
	{
		left = GetTile(coord + Coord.Left);
		right = GetTile(coord + Coord.Right);
		down = GetTile(coord + Coord.Down);
		top = GetTile(coord + Coord.Up);
	}

	public WorldObject GetTile(Coord coord)
	{
		if (coord.y < 0)
		{
			if ((bool)PreviousMap)
			{
				return PreviousMap.GetTile(new Coord(PreviousMap.Index, coord.x, coord.y + PreviousMap.Rows));
			}
			return null;
		}
		if (coord.y >= Rows)
		{
			if ((bool)NextMap)
			{
				return NextMap.GetTile(new Coord(NextMap.Index, coord.x, Rows - coord.y));
			}
			return null;
		}
		if (coord.x < 0 || coord.x >= Columns)
		{
			return null;
		}
		return TileMatrix[coord.x, coord.y];
	}

	public Coord NormalizeCoord(Coord coord)
	{
		if (coord.y < 0)
		{
			if ((bool)PreviousMap)
			{
				return PreviousMap.NormalizeCoord(new Coord(PreviousMap.Index, coord.x, coord.y + PreviousMap.Rows));
			}
			return Coord.None;
		}
		if (coord.y >= Rows)
		{
			if ((bool)NextMap)
			{
				return NextMap.NormalizeCoord(new Coord(NextMap.Index, coord.x, coord.y - Rows));
			}
			return Coord.None;
		}
		if (coord.x < 0 || coord.x >= Columns)
		{
			return Coord.None;
		}
		return coord;
	}

	public void UpdateSides(WorldObject tile)
	{
		if (tile == null || tile.TileParts == null || tile.TileParts.Length == 0)
		{
			return;
		}
		TileMap tileMap = null;
		tileMap = ((tile.Coord.Section < 0) ? GetComponentInParent<TileMap>() : SingletonMonoBehaviour<World>.Instance.Sections[tile.Coord.Section]);
		if (tileMap == null)
		{
			return;
		}
		if (tileMap != this)
		{
			tileMap.UpdateSides(tile);
			return;
		}
		GetTilesAround(tile.Coord, out WorldObject left, out WorldObject right, out WorldObject down, out WorldObject top);
		TileSide tileSide = TileSide.All;
		TileSide tileSide2 = TileSide.All;
		if (left != null && Matches(left.PrefabId, tile.PrefabId))
		{
			tileSide &= (TileSide)(-2);
		}
		else if (left == null)
		{
			tileSide2 &= (TileSide)(-2);
		}
		if (right != null && Matches(right.PrefabId, tile.PrefabId))
		{
			tileSide &= (TileSide)(-3);
		}
		else if (right == null)
		{
			tileSide2 &= (TileSide)(-3);
		}
		if (down != null && Matches(down.PrefabId, tile.PrefabId))
		{
			tileSide &= (TileSide)(-9);
		}
		else if (down == null)
		{
			tileSide2 &= (TileSide)(-9);
		}
		if (top != null && Matches(top.PrefabId, tile.PrefabId))
		{
			tileSide &= (TileSide)(-5);
		}
		else if (top == null)
		{
			tileSide2 &= (TileSide)(-5);
		}
		TileParts[] tileParts = tile.TileParts;
		foreach (TileParts tileParts2 in tileParts)
		{
			if (tileParts2 == null)
			{
				UnityEngine.Debug.LogError("null TilePart in " + tile, tile);
			}
			tileParts2.UpdateSides(tileSide, tileSide2);
		}
	}

	private bool Matches(string id1, string id2)
	{
		if (id1 == id2)
		{
			return true;
		}
		List<string> value;
		return sideMatch.TryGetValue(id1, out value) && value.Contains(id2);
	}

	public Vector3 LocalPosition(Coord coord)
	{
		return new Vector3((float)coord.x * TileWidth + TileWidth / 2f, 0f - ((float)coord.y * TileHeight + TileHeight / 2f));
	}

	public WorldObject InstantiateFromPrefab(string prefabName, Coord tilePos)
	{
		GameObject prefab = Array.Find(SingletonMonoBehaviour<World>.Instance.Prefabs, (PrefabInfo p) => p.Name == prefabName).Prefab;
		return InstantiateFromPrefab(prefab, tilePos);
	}

	public WorldObject InstantiateFromPrefab(GameObject prefab, Coord tilePos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		gameObject.name = prefab.name;
		WorldObject worldObject = gameObject.GetComponent<WorldObject>();
		if (worldObject == null)
		{
			worldObject = gameObject.AddComponent<WorldObject>();
		}
		tilePos.Section = Index;
		if (Index < 0)
		{
			UnityEngine.Debug.LogError("Instantiating from prefab without index");
		}
		worldObject.Coord = tilePos;
		if (TileMatrix[worldObject.Coord.x, worldObject.Coord.y] != null)
		{
			UnityEngine.Debug.LogWarning("Instantiating over existing tile", worldObject);
		}
		TileMatrix[worldObject.Coord.x, worldObject.Coord.y] = worldObject;
		gameObject.transform.position = base.transform.position + LocalPosition(tilePos);
		gameObject.transform.localScale = new Vector3(TileWidth, TileHeight, 1f);
		gameObject.transform.parent = TilesParent;
		UpdateSides(worldObject);
		UpdateSidesAround(worldObject.Coord);
		return worldObject;
	}
}
