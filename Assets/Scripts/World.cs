using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : SingletonMonoBehaviour<World>
{
	public GameObject[] BorderPrefabs;

	private string[] borderIds;

	public GameObject[] FloorPrefabs;

	private string[] floorIds;

	public PrefabInfo[] Prefabs;

	public int Width = 10;

	public int Depth = 200;

	public Vector3 Separation = Vector3.one * 1.01f;

	public float FloorZero;

	private int levelIndex = -1;

	private bool useLoadingSection;

	private bool shouldGenerateRun = true;

	private IEnumerator loadSectionsCoroutine;

	public EnemyDef[] EnemyDefs;

	public LevelDef[] LevelDefs;

	private RunInfo runInfo;

	[HideInInspector]
	public List<TileMap> Sections = new List<TileMap>();

	private List<TileMap> nextSections = new List<TileMap>();

	public GameObject[] LoadingSections;

	public int LoadingPoolSize = 1;

	private int minActiveSection;

	private int maxActiveSection;

	private bool[] sectionLoaded;

	private GameObject[] sectionGameObject;

	[NonSerialized]
	public bool Ready;

	private List<string> usedEnemyTags = new List<string>();

	public string[] BorderIds
	{
		get
		{
			if (borderIds == null)
			{
				borderIds = Array.ConvertAll(BorderPrefabs, (GameObject c) => c.GetComponent<WorldObject>().PrefabId);
			}
			return borderIds;
		}
	}

	public string[] FloorIds
	{
		get
		{
			if (floorIds == null)
			{
				floorIds = Array.ConvertAll(FloorPrefabs, (GameObject c) => c.GetComponent<WorldObject>().PrefabId);
			}
			return floorIds;
		}
	}

	public bool IsLoading
	{
		get;
		private set;
	}

	public int LoadingLevel
	{
		get;
		private set;
	}

	private void Start()
	{
		sectionLoaded = new bool[SceneManager.sceneCountInBuildSettings];
		sectionGameObject = new GameObject[SceneManager.sceneCountInBuildSettings];
		SingletonMonoBehaviour<Pool>.Instance.WarmUp("CoinCube", Array.Find(Prefabs, (PrefabInfo p) => p.Name == "CoinCube").Prefab, 20);
		Restart();
	}

	public void Restart()
	{
		Ready = false;
		useLoadingSection = false;
		FloorZero = 0f;
		shouldGenerateRun = true;
		levelIndex = SaveGame.Instance.WorldLevel - 2;
		Background background = null;
		foreach (TileMap section in Sections)
		{
			if (section != null)
			{
				if (background == null && section.Background != null)
				{
					background = section.Background.GetComponent<Background>();
				}
				section.gameObject.SetActive(value: false);
				section.FreeBackground();
				section.FreeCoins();
				UnityEngine.Object.Destroy(section.gameObject);
			}
		}
		Sections.Clear();
		if (background != null)
		{
			Background.Feature[] features = background.Features;
			foreach (Background.Feature feature in features)
			{
				SingletonMonoBehaviour<Pool>.Instance.WarmUp(feature.Object.name, feature.Object, feature.PoolSize);
			}
			SingletonMonoBehaviour<Pool>.Instance.WarmUp(background.Wall.name, background.Wall, background.PoolSize);
		}
		minActiveSection = (maxActiveSection = 0);
		if (loadSectionsCoroutine != null)
		{
			StopCoroutine(loadSectionsCoroutine);
		}
		StartCoroutine(loadSectionsCoroutine = LoadSections());
	}

	private LevelInfo GenerateLevel(LevelDef levelDef)
	{
		int index = Array.IndexOf(LevelDefs, levelDef);
		List<string> list = (from EnemyTag v in Enum.GetValues(typeof(EnemyTag))
			select v.ToString()).ToList();
		List<string> second = (from e in EnemyDefs
			where e.MinLevelIndex > index || e.MaxLevelIndex <= index
			select e.EnemyTag.ToString()).ToList();
		List<string> list2 = list.Except(second).Except(usedEnemyTags).ToList();
		if (list2.Count < 2)
		{
			usedEnemyTags.Clear();
			list2 = list.Except(second).ToList();
		}
		System.Random rnd = new System.Random();
		List<string> list3 = (from x in list2
			orderby rnd.Next()
			select x).Take(levelDef.EnemyCount).ToList();
		if (index > 9)
		{
			list3.Add(EnemyTag.FireballThrower.ToString());
			list3.Add(EnemyTag.Thwomp.ToString());
		}
		usedEnemyTags.AddRange(list3);
		SectionList sectionList = Resources.Load<SectionList>("SectionList");
		List<SectionInfo> list4 = new List<SectionInfo>();
		SectionInfo[] sectionInfos = sectionList.SectionInfos;
		foreach (SectionInfo sectionInfo in sectionInfos)
		{
			if (sectionInfo.Width == levelDef.Width && sectionInfo.Tags.Except(list3).Intersect(list).Count() == 0 && !sectionInfo.Testing)
			{
				list4.Add(sectionInfo);
			}
		}
		if (list4.Count == 0)
		{
			UnityEngine.Debug.LogError("There are no selectable sections given the design restictions");
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (SectionInfo item2 in list4)
		{
			string[] tags = item2.Tags;
			foreach (string key in tags)
			{
				dictionary[key] = 0;
			}
		}
		List<SectionInfo> list5 = new List<SectionInfo>();
		SectionList sectionList2 = Resources.Load<SectionList>("ConnectionList");
		SectionInfo item = sectionList2.SectionInfos.First((SectionInfo c) => c.Name == levelDef.Connection);
		list5.Add(item);
		if (!SaveGame.Instance.TutorialComplete && levelDef.Width == 5)
		{
			list5.Add(sectionList.SectionInfos.First((SectionInfo c) => c.Name == "Tutorial"));
		}
		for (int k = 0; k < levelDef.SectionCount; k++)
		{
			SectionInfo sectionInfo2 = ChooseSection(list4, dictionary);
			UpdateTagCount(dictionary, sectionInfo2.Tags);
			list5.Add(sectionInfo2);
		}
		if (levelDef == LevelDefs.Last())
		{
			item = sectionList2.SectionInfos.First((SectionInfo c) => c.Name == "End");
			list5.Add(item);
		}
		LevelInfo levelInfo = new LevelInfo();
		levelInfo.SectionInfos = list5.ToArray();
		return levelInfo;
	}

	private int LoadingIndex(int index)
	{
		return (LevelDefs[index].Width - 5) / 2;
	}

	private void GenerateRun()
	{
		runInfo = new RunInfo();
		List<LevelInfo> list = new List<LevelInfo>();
		LevelDef[] levelDefs = LevelDefs;
		foreach (LevelDef levelDef in levelDefs)
		{
			LevelInfo item = GenerateLevel(levelDef);
			list.Add(item);
		}
		runInfo.LevelInfos = list.ToArray();
		shouldGenerateRun = false;
	}

	private IEnumerator LoadSections()
	{
		IsLoading = true;
		StartCoroutine(CheckLoadingPerformance());
		int nextLevelIndex = levelIndex + 1;
		LoadingLevel = nextLevelIndex + 1;
		bool lastLevel = nextLevelIndex == LevelDefs.Length - 1;
		if (Sections.Count == 0 && levelIndex != -1)
		{
			TileMap previousTileMap2 = null;
			int num = (!lastLevel) ? 2 : 14;
			for (int i = 0; i < num; i++)
			{
				TileMap tileMap = GenerateSection(LoadingSections[LoadingIndex(levelIndex)]);
				tileMap.DontDestroy = lastLevel;
				Sections.Add(tileMap);
				InitializeSection(tileMap, previousTileMap2, 1f);
				InitializeTiles(tileMap);
				previousTileMap2 = tileMap;
				if (lastLevel)
				{
					tileMap.gameObject.SetActive(value: true);
					tileMap.SetupBackground();
				}
			}
		}
		if (shouldGenerateRun)
		{
			GenerateRun();
		}
		if (nextLevelIndex < runInfo.LevelInfos.Length)
		{
			LevelInfo levelInfo = runInfo.LevelInfos[nextLevelIndex];
			float startTime = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup - startTime < 0.5f)
			{
				yield return null;
			}
			if (nextLevelIndex < LevelDefs.Length)
			{
				IEnumerator coroutine = SingletonMonoBehaviour<Pool>.Instance.WarmUpCoroutine(LoadingSections[LoadingIndex(nextLevelIndex)].name, LoadingSections[LoadingIndex(nextLevelIndex)], LoadingPoolSize);
				while (coroutine.MoveNext())
				{
					yield return null;
				}
			}
			SectionInfo[] sectionInfos = levelInfo.SectionInfos;
			foreach (SectionInfo sectionInfo in sectionInfos)
			{
				if (sectionLoaded[sectionInfo.BuildIndex])
				{
					continue;
				}
				Scene scene = SceneManager.GetSceneByBuildIndex(sectionInfo.BuildIndex);
				if (!scene.IsValid())
				{
					AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sectionInfo.BuildIndex, LoadSceneMode.Additive);
					while (!asyncOperation.isDone)
					{
						yield return null;
					}
					sectionLoaded[sectionInfo.BuildIndex] = true;
					scene = SceneManager.GetSceneByBuildIndex(sectionInfo.BuildIndex);
				}
				GameObject[] rootGos = scene.GetRootGameObjects();
				if (rootGos.Length == 0)
				{
					UnityEngine.Debug.LogError("Scene " + scene.name + " is empty");
				}
				else
				{
					sectionGameObject[sectionInfo.BuildIndex] = rootGos[0];
				}
			}
			yield return null;
			nextSections.Clear();
			SectionInfo[] sectionInfos2 = levelInfo.SectionInfos;
			foreach (SectionInfo sectionInfo2 in sectionInfos2)
			{
				TileMap section = GenerateSection(sectionGameObject[sectionInfo2.BuildIndex]);
				section.DontDestroy = lastLevel;
				nextSections.Add(section);
				yield return null;
			}
			yield return null;
			useLoadingSection = false;
			levelIndex = nextLevelIndex;
			SaveGame.Instance.WorldLevel = levelIndex + 1;
			foreach (TileMap nextSection in nextSections)
			{
				WorldObject[] componentsInChildren = nextSection.GetComponentsInChildren<WorldObject>();
				WorldObject[] array = componentsInChildren;
				foreach (WorldObject worldObject in array)
				{
					worldObject.enabled = false;
				}
			}
			float emptyWeight2 = UnityEngine.Random.Range(-2f, 2.5f);
			emptyWeight2 = ((!(emptyWeight2 <= 0f)) ? (emptyWeight2 + 1f) : (-1f / (emptyWeight2 - 1f)));
			UnityEngine.Debug.Log("Empty weight: " + emptyWeight2);
			TileMap previousTileMap = null;
			foreach (TileMap tilemap in nextSections)
			{
				if (Sections.Count > 0)
				{
					previousTileMap = Sections[Sections.Count - 1];
				}
				Sections.Add(tilemap);
				InitializeSection(tilemap, previousTileMap, emptyWeight2);
				yield return null;
			}
			InitializeTileSides(nextSections[nextSections.Count - 1]);
			foreach (TileMap nextSection2 in nextSections)
			{
				InitializeTiles(nextSection2);
			}
			SpawnCoins();
			foreach (TileMap nextSection3 in nextSections)
			{
				WorldObject[] componentsInChildren2 = nextSection3.GetComponentsInChildren<WorldObject>();
				WorldObject[] array2 = componentsInChildren2;
				foreach (WorldObject worldObject2 in array2)
				{
					worldObject2.enabled = true;
				}
			}
			Ready = true;
            Debug.Log("Ready = " + Ready);
            useLoadingSection = true;
			IsLoading = false;
		}
		else
		{
			UnityEngine.Debug.LogError("Missing level index " + nextLevelIndex);
		}
	}

	private IEnumerator CheckLoadingPerformance()
	{
		if (!SingletonMonoBehaviour<Game>.Instance.Started || !SingletonMonoBehaviour<Gui>.Instance.Ready)
		{
			yield break;
		}
		int badFrames = 0;
		while (IsLoading)
		{
			float before = Time.realtimeSinceStartup;
			yield return null;
			float delta = Time.realtimeSinceStartup - before;
			if (delta > 0.05f)
			{
				badFrames++;
			}
			if (badFrames >= 2 && !GameTime.Paused)
			{
				GameTime.Pause();
				Gui.Views.LoadingView.ShowAnimated();
				UpdateLoadingSections();
			}
		}
		if (GameTime.Paused && !Gui.Views.PauseView.Visible)
		{
			GameTime.Resume();
		}
		if (Gui.Views.LoadingView.Visible)
		{
			Gui.Views.LoadingView.HideAnimated();
		}
	}

	private SectionInfo ChooseSection(List<SectionInfo> list, Dictionary<string, int> tags)
	{
		float num = 0f;
		foreach (SectionInfo item in list)
		{
			item.ComputedWeight = item.Weight;
			string[] tags2 = item.Tags;
			foreach (string key in tags2)
			{
				int value = 0;
				float num2 = 1f;
				if (tags.TryGetValue(key, out value) && 1f / (float)value < num2)
				{
					num2 = 1f / (float)value;
				}
				item.ComputedWeight *= num2;
			}
			num += item.ComputedWeight;
		}
		float num3 = UnityEngine.Random.Range(0f, num);
		foreach (SectionInfo item2 in list)
		{
			num3 -= item2.ComputedWeight;
			if (num3 <= 0f)
			{
				return item2;
			}
		}
		return list[list.Count - 1];
	}

	private void UpdateTagCount(Dictionary<string, int> tags, string[] lastSectionTags)
	{
		foreach (string item in tags.Keys.ToList())
		{
			int num = tags[item];
			if (num > 0)
			{
				tags[item] = num - 1;
			}
		}
		foreach (string text in lastSectionTags)
		{
			Dictionary<string, int> dictionary;
			string key;
			(dictionary = tags)[key = text] = dictionary[key] + 3;
		}
	}

	private TileMap GenerateSection(GameObject template)
	{
		string name = template.name;
		GameObject gameObject = (!SingletonMonoBehaviour<Pool>.Instance.ContainsKey(name)) ? UnityEngine.Object.Instantiate(template) : SingletonMonoBehaviour<Pool>.Instance.Alloc(name);
		gameObject.gameObject.SetActive(value: false);
		gameObject.transform.parent = base.transform;
		TileMap component = gameObject.GetComponent<TileMap>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("Section with no tilemap " + gameObject, gameObject);
			return null;
		}
		return component;
	}

	private void InitializeSection(TileMap tilemap, TileMap previousTileMap, float emptyWeight)
	{
		int index = 0;
		if (previousTileMap != null)
		{
			index = previousTileMap.Index + 1;
			previousTileMap.NextMap = tilemap;
		}
		tilemap.Level = levelIndex;
		tilemap.Index = index;
		tilemap.PreviousMap = previousTileMap;
		tilemap.NextMap = null;
		tilemap.transform.position = new Vector3((float)(-tilemap.Columns) * tilemap.TileWidth / 2f, FloorZero, 0f);
		FloorZero -= tilemap.Rows;
		tilemap.InitTileCoords();
		tilemap.CreateMatrix();
		CubeRandomizer[] componentsInChildren = tilemap.GetComponentsInChildren<CubeRandomizer>(includeInactive: true);
		foreach (CubeRandomizer cubeRandomizer in componentsInChildren)
		{
			cubeRandomizer.Randomize(tilemap);
		}
		SectionRandomize component = tilemap.GetComponent<SectionRandomize>();
		if (component != null)
		{
			component.Randomize(emptyWeight);
		}
		if (UnityEngine.Random.Range(0, 2) > 0 && tilemap.Flippable)
		{
			FlipSection(tilemap);
		}
		List<StoreCube> cubes = tilemap.GetComponentsInChildren<StoreCube>().ToList();
		StoreCube.ChooseItems(cubes);
		tilemap.CreateMatrix();
		if ((bool)tilemap.PreviousMap)
		{
			InitializeTileSides(tilemap.PreviousMap);
		}
	}

	private void SpawnCoins()
	{
		List<List<Coord>> list = new List<List<Coord>>();
		foreach (TileMap nextSection in nextSections)
		{
			if (nextSection.Index != 0 && !nextSection.IsTransition && !nextSection.DisableCoins && !nextSection.IsTutorial)
			{
				for (int i = 0; i < nextSection.Rows; i++)
				{
					int j;
					for (j = 0; j < nextSection.Columns && nextSection.TileMatrix[j, i] == null; j++)
					{
					}
					for (; j < nextSection.Columns && nextSection.TileMatrix[j, i] != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, nextSection.TileMatrix[j, i].PrefabId) != -1; j++)
					{
					}
					int num = nextSection.Columns - 1;
					while (num >= 0 && nextSection.TileMatrix[num, i] == null)
					{
						num--;
					}
					while (num >= 0 && nextSection.TileMatrix[num, i] != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.BorderIds, nextSection.TileMatrix[num, i].PrefabId) != -1)
					{
						num--;
					}
					List<Coord> list2 = new List<Coord>();
					for (int k = j; k <= num; k++)
					{
						Coord coord = new Coord(nextSection.Index, k, i);
						WorldObject tile = nextSection.GetTile(coord);
						if (tile is ArrowSource || tile is ThwompCube)
						{
							list2.Clear();
							break;
						}
						if (!(tile != null) && !UnbreakableSides(coord) && !UnbreakableSides(coord + Coord.Up) && !UnbreakableSides(coord + Coord.Down))
						{
							list2.Add(coord);
						}
					}
					if (list2.Count > 0)
					{
						list.Add(list2);
					}
				}
			}
		}
		int num2 = GameVars.CoinsPerLevel;
		if (list.Count > 0)
		{
			int num3 = 0;
			while (num2 > 0 && num3 < 100)
			{
				List<Coord> list3 = list[UnityEngine.Random.Range(0, list.Count)];
				if (list3.Count != 0)
				{
					Coord coord2 = list3[UnityEngine.Random.Range(0, list3.Count)];
					TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(coord2.Section);
					if (!(section == null))
					{
						list3.Remove(coord2);
						Coin component = SingletonMonoBehaviour<Pool>.Instance.Alloc("CoinCube").GetComponent<Coin>();
						if (component.Broken)
						{
							component.Breakable.Unbreak();
						}
						component.transform.parent = section.transform;
						component.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord2);
						component.MoveCoord(coord2);
						component.gameObject.SetActive(value: true);
						num2 -= component.Coins;
					}
				}
				num3++;
			}
		}
		if (num2 > 0)
		{
			UnityEngine.Debug.LogWarning("Not enought coins in level to fulfill coins per level minimum (remainingCoins: " + num2 + ").");
		}
	}

	private bool UnbreakableSides(Coord coord)
	{
		coord = coord.Normalize();
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
		WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(coord + Coord.Left);
		WorldObject tile3 = SingletonMonoBehaviour<World>.Instance.GetTile(coord + Coord.Right);
		return (tile == null || (bool)tile.Breakable) && IsUnbreakableOrCoin(tile2) && IsUnbreakableOrCoin(tile3);
	}

	private bool IsUnbreakableOrCoin(WorldObject tile)
	{
		return (tile != null && tile.Breakable == null) || tile is Coin;
	}

	private void UpdateLoadingSections()
	{
		if (!useLoadingSection || levelIndex + 1 >= LevelDefs.Length)
		{
			return;
		}
		Camera camera = SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
		Camera camera2 = camera;
		Vector3 position = camera.transform.position;
		Vector3 vector = camera2.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f - position.z));
		for (int i = 0; i < 10; i++)
		{
			TileMap tileMap = Sections[Sections.Count - 1];
			Vector3 vector2 = tileMap.transform.position + tileMap.TileHeight * (float)tileMap.Rows * Vector3.down;
			bool flag = LoadingLevel == LevelDefs.Length;
			int num = (!flag) ? 5 : 100;
			if (vector.y - (float)num > vector2.y)
			{
				break;
			}
			TileMap tileMap2 = GenerateSection(LoadingSections[LoadingIndex(levelIndex)]);
			Sections.Add(tileMap2);
			InitializeSection(tileMap2, tileMap, 1f);
			InitializeTiles(tileMap2);
			tileMap2.DontDestroy = flag;
			tileMap2.gameObject.SetActive(value: true);
			tileMap2.SetupBackground();
			InitializeTileSides(tileMap2);
		}
	}

	private void FixedUpdate()
	{
		if (!Ready)
		{
			return;
		}
		Camera camera = SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
		Camera camera2 = camera;
		Vector3 position = camera.transform.position;
		Vector3 vector = camera2.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f - position.z));
		Camera camera3 = camera;
		Vector3 position2 = camera.transform.position;
		Vector3 vector2 = camera3.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f - position2.z));
		if (SingletonMonoBehaviour<Game>.Instance.Digger != null)
		{
			WorldObject component = SingletonMonoBehaviour<Game>.Instance.Digger.GetComponent<WorldObject>();
			TileMap section = GetSection(component.Coord.Section);
			if (section != null && !IsLoading && section.IsLoadingSection && section.Level == levelIndex)
			{
				StartCoroutine(loadSectionsCoroutine = LoadSections());
				SingletonMonoBehaviour<Game>.Instance.OnLevelComplete();
			}
		}
		UpdateLoadingSections();
		minActiveSection = Sections.Count;
		maxActiveSection = 0;
		int num = Sections.Count;
		if (SingletonMonoBehaviour<Game>.Instance.Chicken != null && !SingletonMonoBehaviour<Game>.Instance.Chicken.Broken)
		{
			num = SingletonMonoBehaviour<Game>.Instance.Chicken.Coord.Section;
		}
		for (int num2 = Sections.Count - 1; num2 >= 0; num2--)
		{
			TileMap tileMap = Sections[num2];
			if (!(tileMap == null))
			{
				Vector3 vector3 = tileMap.transform.position + tileMap.TileHeight * Vector3.up;
				Vector3 vector4 = tileMap.transform.position + (tileMap.TileHeight + 1f) * (float)tileMap.Rows * Vector3.down;
				if (vector4.y > vector.y || vector3.y < vector2.y)
				{
					if (vector4.y > vector.y && !tileMap.DontDestroy && num2 < num)
					{
						Enemy[] componentsInChildren = tileMap.GetComponentsInChildren<Enemy>();
						foreach (Enemy enemy in componentsInChildren)
						{
							if (!enemy.Broken)
							{
								SingletonMonoBehaviour<Game>.Instance.EnemiesLeftAliveInLevel++;
							}
						}
						if (tileMap.gameObject.activeSelf)
						{
							tileMap.gameObject.SetActive(value: false);
						}
						tileMap.FreeBackground();
						tileMap.FreeCoins();
						if (SingletonMonoBehaviour<Pool>.Instance.ContainsKey(tileMap.gameObject.name))
						{
							SingletonMonoBehaviour<Pool>.Instance.Free(tileMap.gameObject);
						}
						else
						{
							UnityEngine.Object.Destroy(tileMap.gameObject);
						}
						Sections[num2] = null;
					}
				}
				else if (!tileMap.gameObject.activeSelf)
				{
					tileMap.gameObject.SetActive(value: true);
					tileMap.SetupBackground();
				}
				if (tileMap.gameObject.activeSelf)
				{
					if (tileMap.Index < minActiveSection)
					{
						minActiveSection = tileMap.Index;
					}
					if (tileMap.Index > maxActiveSection)
					{
						maxActiveSection = tileMap.Index;
					}
				}
			}
		}
	}

	public TileMap GetSection(int index)
	{
		if (index < 0 || index >= Sections.Count)
		{
			return null;
		}
		return Sections[index];
	}

	public Coord GetCoordFromPosition(Vector3 position)
	{
		int num = minActiveSection;
		int num2 = maxActiveSection;
		TileMap tileMap = GetSection(num);
		if (tileMap == null || GetSection(num2) == null)
		{
			return Coord.None;
		}
		while (num2 > num)
		{
			if (num2 == num + 1)
			{
				float y = position.y;
				Vector3 position2 = Sections[num2].transform.position;
				tileMap = ((!(y > position2.y)) ? Sections[num2] : GetSection(num));
				break;
			}
			int num3 = (num2 + num) / 2;
			TileMap section = GetSection(num3);
			if (section == null)
			{
				return Coord.None;
			}
			float y2 = position.y;
			Vector3 position3 = section.transform.position;
			if (y2 > position3.y)
			{
				num2 = num3;
				continue;
			}
			float y3 = position.y;
			Vector3 position4 = section.transform.position;
			if (y3 < position4.y + (float)Sections[num3].Rows)
			{
				num = num3;
				continue;
			}
			tileMap = section;
			break;
		}
		if (tileMap == null)
		{
			return Coord.None;
		}
		return tileMap.GetCoordFromLocalPosition(tileMap.transform.InverseTransformPoint(position));
	}

	public WorldObject GetTile(Coord coord)
	{
		if (coord == Coord.None)
		{
			return null;
		}
		if (coord.Section >= 0 && coord.Section < Sections.Count && Sections[coord.Section] != null)
		{
			return Sections[coord.Section].GetTile(coord);
		}
		return null;
	}

	public void Move(WorldObject wo, Coord coord)
	{
		if (IsCoordValid(wo.Coord))
		{
			TileMap section = GetSection(wo.Coord.Section);
			if (section.TileMatrix[wo.Coord.x, wo.Coord.y] == wo)
			{
				section.TileMatrix[wo.Coord.x, wo.Coord.y] = null;
			}
		}
		if (IsCoordValid(coord))
		{
			if (Sections[coord.Section].TileMatrix[coord.x, coord.y] != null)
			{
				UnityEngine.Debug.LogWarning("Replacing " + Sections[coord.Section].TileMatrix[coord.x, coord.y] + " with " + wo, wo);
			}
			Sections[coord.Section].TileMatrix[coord.x, coord.y] = wo;
		}
		else
		{
			UnityEngine.Debug.LogWarning("Trying to move to invalid coord " + coord, wo);
		}
	}

	public Coord NormalizeCoord(Coord coord)
	{
		if (coord.Section >= 0 && coord.Section < Sections.Count && Sections[coord.Section] != null)
		{
			return Sections[coord.Section].NormalizeCoord(coord);
		}
		return coord;
	}

	public Vector3 GetPositionFromCoord(Coord coord)
	{
		TileMap section = GetSection(coord.Section);
		if ((bool)section)
		{
			return section.transform.position + section.LocalPosition(coord);
		}
		UnityEngine.Debug.LogWarning("GetPositionFromCoord called with invalid coord " + coord);
		return Vector3.zero;
	}

	public bool IsCoordValid(Coord coord)
	{
		return coord.Section >= 0 && coord.Section < Sections.Count && Sections[coord.Section] != null && coord.x >= 0 && coord.x < Sections[coord.Section].Columns && coord.y >= 0 && coord.y < Sections[coord.Section].Rows;
	}

	private void InitializeTileSides(TileMap tilemap)
	{
		WorldObject[] componentsInChildren = tilemap.GetComponentsInChildren<WorldObject>();
		WorldObject[] array = componentsInChildren;
		foreach (WorldObject tile in array)
		{
			tilemap.UpdateSides(tile);
		}
	}

	private void InitializeTiles(TileMap tilemap)
	{
		if (!tilemap.Initialized)
		{
			WorldObject[] componentsInChildren = tilemap.GetComponentsInChildren<WorldObject>();
			WorldObject[] array = componentsInChildren;
			foreach (WorldObject worldObject in array)
			{
				worldObject.Initialize();
			}
			tilemap.Initialized = true;
		}
	}

	private void FlipSection(TileMap tilemap)
	{
		tilemap.Flipped = true;
		Transform tilesParent = tilemap.TilesParent;
		TileMap componentInChildren = tilemap.GetComponentInChildren<TileMap>();
		for (int i = 0; i < tilesParent.childCount; i++)
		{
			WorldObject component = tilesParent.GetChild(i).GetComponent<WorldObject>();
			if (!(component == null))
			{
				component.Coord = new Coord(tilemap.Index, componentInChildren.Columns - component.Coord.x - 1, component.Coord.y);
				bool outleft = component.Outleft;
				component.Outleft = component.Outright;
				component.Outright = outleft;
				component.transform.position = componentInChildren.transform.position + componentInChildren.LocalPosition(component.Coord);
				if ((bool)component.FlipContent)
				{
					component.FlipContent.localRotation = Quaternion.AngleAxis(180f, Vector3.up) * component.FlipContent.localRotation;
				}
				component.Flip();
			}
		}
	}
}
