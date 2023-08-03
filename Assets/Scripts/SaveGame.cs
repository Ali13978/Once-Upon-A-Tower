using SimpleJSON;
using System;
using System.Globalization;
using UnityEngine;

public class SaveGame
{
	private static SaveGame instance;

	public bool ThereIsANewerSave;

	private bool loaded;

	private string newerSave = string.Empty;

	private TimeSpan newerSavePlaytime;

	private JSONNode json = new JSONClass();

	private float currentPlayTime;

	public static SaveGame Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SaveGame();
			}
			return instance;
		}
	}

	public string UserId
	{
		get
		{
			string text = GetString("userId", string.Empty);
			if (string.IsNullOrEmpty(text))
			{
				text = Guid.NewGuid().ToString();
				SetString("userId", text);
			}
			return text;
		}
	}

	public string PreviousCharacter
	{
		get
		{
			return GetString("PreviousCharacter", Characters.Instance.DefaultCharacter);
		}
		set
		{
			SetString("PreviousCharacter", value);
		}
	}

	public string CurrentCharacter
	{
		get
		{
			return GetString("CurrentCharacter", Characters.Instance.DefaultCharacter);
		}
		set
		{
			SetString("CurrentCharacter", value);
		}
	}

	public float PlayTime
	{
		get
		{
			float @float = GetFloat("PlayTime", 0f);
			return currentPlayTime + @float;
		}
	}

	public int PrizeCoins
	{
		get
		{
			return GetInt("PrizeCoins", 0);
		}
		set
		{
			SetInt("PrizeCoins", value);
		}
	}

	public int PrizeCount
	{
		get
		{
			return GetInt("PrizeCount", 0);
		}
		set
		{
			SetInt("PrizeCount", value);
		}
	}

	public int SaveMeCoins
	{
		get
		{
			return GetInt("SaveMeCoins", GameVars.StartSaveMeCoins);
		}
		set
		{
			SetInt("SaveMeCoins", value);
		}
	}

	public int SaveMeCount
	{
		get
		{
			return GetInt("SaveMeCount", 0);
		}
		set
		{
			SetInt("SaveMeCount", value);
		}
	}

	public int WorldLevel
	{
		get
		{
			return GetInt("WorldLevel", 1);
		}
		set
		{
			SetInt("WorldLevel", value);
		}
	}

	public bool TutorialComplete
	{
		get
		{
			return GetBool("TutorialComplete", defaultValue: false);
		}
		set
		{
			SetBool("TutorialComplete", value);
		}
	}

	public DateTime LastAdDateTime
	{
		get
		{
			return GetDate("LastAdDateTime", default(DateTime));
		}
		set
		{
			SetDate("LastAdDateTime", value);
		}
	}

	public string AndroidNotifications
	{
		get
		{
			return GetString("AndroidNotifications", string.Empty);
		}
		set
		{
			SetString("AndroidNotifications", value);
		}
	}

	public int DiggerBombs
	{
		get
		{
			return GetInt("DiggerBombs", 0);
		}
		set
		{
			SetInt("DiggerBombs", value);
		}
	}

	public int DiggerCoins
	{
		get
		{
			return GetInt("DiggerCoins", 0);
		}
		set
		{
			SetInt("DiggerCoins", value);
		}
	}

	public bool Music
	{
		get
		{
			return GetBool("Music", defaultValue: true);
		}
		set
		{
			SetBool("Music", value);
		}
	}

	public bool SFX
	{
		get
		{
			return GetBool("SFX", defaultValue: true);
		}
		set
		{
			SetBool("SFX", value);
		}
	}

	public int MissionLevel
	{
		get
		{
			return GetInt("MissionLevel", 1);
		}
		set
		{
			SetInt("MissionLevel", value);
		}
	}

	private bool Mission1Completed
	{
		get
		{
			return GetBool("Mission1Completed", defaultValue: false);
		}
		set
		{
			SetBool("Mission1Completed", value);
		}
	}

	private bool Mission2Completed
	{
		get
		{
			return GetBool("Mission2Completed", defaultValue: false);
		}
		set
		{
			SetBool("Mission2Completed", value);
		}
	}

	private bool Mission3Completed
	{
		get
		{
			return GetBool("Mission3Completed", defaultValue: false);
		}
		set
		{
			SetBool("Mission3Completed", value);
		}
	}

	public bool[] CompletedMissions
	{
		get
		{
			return new bool[3]
			{
				Mission1Completed,
				Mission2Completed,
				Mission3Completed
			};
		}
		set
		{
			Mission1Completed = value[0];
			Mission2Completed = value[1];
			Mission3Completed = value[2];
		}
	}

	public int Mission1Progress
	{
		get
		{
			return GetInt("Mission1Progress", 0);
		}
		set
		{
			SetInt("Mission1Progress", value);
		}
	}

	public int Mission2Progress
	{
		get
		{
			return GetInt("Mission2Progress", 0);
		}
		set
		{
			SetInt("Mission2Progress", value);
		}
	}

	public int Mission3Progress
	{
		get
		{
			return GetInt("Mission3Progress", 0);
		}
		set
		{
			SetInt("Mission3Progress", value);
		}
	}

	public int BestScore
	{
		get
		{
			return GetInt("BestScore", 0);
		}
		set
		{
			SetInt("BestScore", value);
		}
	}

	public int BestPendingReportScore
	{
		get
		{
			return GetInt("BestPendingReportScore", 0);
		}
		set
		{
			SetInt("BestPendingReportScore", value);
		}
	}

	public bool PlaceGift
	{
		get
		{
			return GetBool("PlaceGift", defaultValue: false);
		}
		set
		{
			SetBool("PlaceGift", value);
		}
	}

	public DateTime NextGiftTime
	{
		get
		{
			return GetDate("NextGiftTime", DateTime.MaxValue);
		}
		set
		{
			SetDate("NextGiftTime", value);
		}
	}

	public DateTime NextLuckyTime
	{
		get
		{
			return GetDate("NextLuckyTime", DateTime.MinValue);
		}
		set
		{
			SetDate("NextLuckyTime", value);
		}
	}

	public bool RateUsDisabled
	{
		get
		{
			return GetBool("RateUsDisabled", defaultValue: false);
		}
		set
		{
			SetBool("RateUsDisabled", value);
		}
	}

	public DateTime NextRateUsTime
	{
		get
		{
			return GetDate("NextRateUsTime", DateTime.MaxValue);
		}
		set
		{
			SetDate("NextRateUsTime", value);
		}
	}

	public void Update()
	{
		currentPlayTime += Time.deltaTime;
	}

	public void SetPlayTime()
	{
		SetFloat("PlayTime", PlayTime);
		currentPlayTime = 0f;
	}

	private string ItemKey(string item)
	{
		return "Item." + item;
	}

	public bool ItemActive(string item)
	{
		return GetBool(ItemKey(item), defaultValue: false);
	}

	public void SetItemActive(string item, bool value)
	{
		SetBool(ItemKey(item), value);
	}

	private string CharacterKey(string character)
	{
		return "CharacterOwned[" + character + "]";
	}

	public bool CharacterOwned(string character)
	{
		return GetBool(CharacterKey(character), character == Characters.Instance.DefaultCharacter);
	}

	public void SetCharacterOwned(string character, bool value)
	{
		SetBool(CharacterKey(character), value);
	}

	private string CharacterRescuedKey(string character)
	{
		return "CharacterRescued[" + character + "]";
	}

	public bool CharacterRescued(string character)
	{
		return GetBool(CharacterRescuedKey(character), defaultValue: false);
	}

	public void SetCharacterRescued(string character, bool value)
	{
		SetBool(CharacterRescuedKey(character), value);
	}

	private string keyForAchievement(string achievement)
	{
		return "Achievement[" + achievement + "]";
	}

	public void SetAchievement(string key)
	{
		SetBool(keyForAchievement(key), value: true);
	}

	public void ClearAchievement(string key)
	{
		SetBool(keyForAchievement(key), value: false);
	}

	public bool HasAchieved(string key)
	{
		return GetBool(keyForAchievement(key), defaultValue: false);
	}

	public void Save(bool force = false)
	{
		SetPlayTime();
		string text = json.ToString();
		SaveLocal(text);
		if (SingletonMonoBehaviour<GooglePlayManager>.HasInstance())
		{
			SingletonMonoBehaviour<GooglePlayManager>.Instance.Save(text, TimeSpan.FromSeconds(PlayTime), force);
		}
	}

	public void SaveLocal(string serialized)
	{
		PlayerPrefs.SetString("savegame", serialized);
		PlayerPrefs.Save();
	}

	public void NewerGame(string save, TimeSpan playTime)
	{
		JSONNode jSONNode = JSON.Parse(save);
		if (!loaded)
		{
			Load();
		}
		if (jSONNode != null && PlayTime < jSONNode["PlayTime"].AsFloat)
		{
			UnityEngine.Debug.Log("greater playtime from internet " + PlayTime + " " + jSONNode["PlayTime"].AsFloat + " " + playTime.TotalSeconds);
			newerSave = save;
			newerSavePlaytime = playTime;
			ThereIsANewerSave = true;
		}
	}

	public void LoadNewerSave()
	{
		Load(newerSave, newerSavePlaytime);
		ThereIsANewerSave = false;
	}

	public void Load(string save, TimeSpan playTime)
	{
		JSONNode d = JSON.Parse(save);
		if (!loaded)
		{
			Load();
		}
		UnityEngine.Debug.Log("greater playtime from internet");
		json = d;
		SetFloat("PlayTime", (float)playTime.TotalSeconds + 100f);
		loaded = true;
		SaveLocal(d);
	}

	public void Load()
	{
		string aJSON = "{}";
		if (PlayerPrefs.HasKey("savegame"))
		{
			aJSON = PlayerPrefs.GetString("savegame");
		}
		json = JSON.Parse(aJSON);
		if (json == null)
		{
			json = new JSONClass();
		}
		loaded = true;
	}

	private int GetInt(string name, int defaultValue)
	{
		if (!loaded)
		{
			Load();
		}
		if (json.HasKey(name))
		{
			return json[name].AsInt;
		}
		return defaultValue;
	}

	private void SetInt(string name, int value)
	{
		if (!loaded)
		{
			Load();
		}
		json[name].AsInt = value;
	}

	public bool GetBool(string name, bool defaultValue)
	{
		return GetInt(name, defaultValue ? 1 : 0) != 0;
	}

	public void SetBool(string name, bool value)
	{
		SetInt(name, value ? 1 : 0);
	}

	private float GetFloat(string name, float defaultValue)
	{
		if (!loaded)
		{
			Load();
		}
		if (json.HasKey(name))
		{
			return json[name].AsFloat;
		}
		return defaultValue;
	}

	private void SetFloat(string name, float value)
	{
		if (!loaded)
		{
			Load();
		}
		json[name].AsFloat = value;
	}

	private string GetString(string name, string defaultValue)
	{
		if (!loaded)
		{
			Load();
		}
		if (json.HasKey(name))
		{
			return json[name];
		}
		return defaultValue;
	}

	private void SetString(string name, string value)
	{
		if (!loaded)
		{
			Load();
		}
		json[name] = value;
	}

	private void SetDate(string key, DateTime dateTime)
	{
		SetString(key, dateTime.ToString(CultureInfo.InvariantCulture));
	}

	private DateTime GetDate(string key, DateTime dateTime)
	{
		string @string = GetString(key, string.Empty);
		if (@string == string.Empty)
		{
			return dateTime;
		}
		return DateTime.Parse(@string, CultureInfo.InvariantCulture);
	}

	public static void ClearSaveGame()
	{
		PlayerPrefs.DeleteAll();
		Instance.Load();
		Instance.Save(force: true);
	}
}
