using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class CodeResult
{
	public int MissionLevel;

	public int BestScore;

	public int SaveMe;

	public List<string> Characters = new List<string>();

	public CodeResult(string jsontext)
	{
		JSONNode jSONNode = JSON.Parse(jsontext);
		if (jSONNode == null)
		{
			UnityEngine.Debug.Log("Empty code");
			return;
		}
		MissionLevel = jSONNode["mission_level"].AsInt;
		BestScore = jSONNode["top_score"].AsInt;
		SaveMe = jSONNode["save_me"].AsInt;
		JSONArray asArray = jSONNode["characters"].AsArray;
		if (asArray != null)
		{
			for (int i = 0; i < asArray.Count; i++)
			{
				Characters.Add(asArray[i].Value);
			}
		}
	}

	private int Int(Dictionary<string, object> values, string s)
	{
		object value;
		if (values.TryGetValue(s, out value))
		{
			// Code block where the value is used
		}

		{
			if (value is long)
			{
				return (int)(long)value;
			}
			if (value is int)
			{
				return (int)value;
			}
		}
		return 0;
	}

	public void Apply()
	{
		if (MissionLevel > SaveGame.Instance.MissionLevel)
		{
			SaveGame.Instance.MissionLevel = MissionLevel;
			bool[] completedMissions = new bool[3];
			SaveGame.Instance.CompletedMissions = completedMissions;
		}
		if (BestScore > SaveGame.Instance.BestScore)
		{
			SaveGame.Instance.BestScore = BestScore;
			//GameServices.PostScore(BestScore);
		}
		SaveGame.Instance.SaveMeCoins += SaveMe;
		for (int i = 0; i < Characters.Count; i++)
		{
			SaveGame.Instance.SetCharacterOwned(Characters[i], value: true);
		}
		SaveGame.Instance.Save();
	}
}
