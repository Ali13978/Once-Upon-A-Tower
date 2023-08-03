using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Characters : ScriptableObject
{
	private static Character[] list;

	private static Characters instance;

	public string DefaultCharacter;

	public Character[] SerializeList;

	public static Character[] List
	{
		get
		{
			if (list == null)
			{
				list = (from c in Instance.SerializeList
					where c.Enabled && (!Application.isPlaying || !Gui.Android || !IsSpecial(c.Name))
					select c).ToArray();
			}
			return list;
		}
	}

	public static Character[] PendingList => (from c in List
		where !SaveGame.Instance.CharacterOwned(c.Name)
		select c).ToArray();

	public static Characters Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load<Characters>("Characters");
			}
			return instance;
		}
	}

	public static int OwnedCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < List.Length; i++)
			{
				if (SaveGame.Instance.CharacterOwned(List[i].Name))
				{
					num++;
				}
			}
			return num;
		}
	}

	public static int RescuedCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < List.Length; i++)
			{
				if (SaveGame.Instance.CharacterRescued(List[i].Name))
				{
					num++;
				}
			}
			return num;
		}
	}

	public static int Total => List.Length;

	public static Character ByName(string name)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].Name == name)
			{
				return List[i];
			}
		}
		return null;
	}

	public static bool IsSpecial(string name)
	{
		return false;
	}

	public static void Reload()
	{
		list = null;
		instance = null;
	}

	public static string ChooseToUnlock()
	{
		List<Character> list = new List<Character>();
		Character[] array = List;
		foreach (Character character in array)
		{
			if (!SaveGame.Instance.CharacterOwned(character.Name))
			{
				list.Add(character);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[Random.Range(0, list.Count)].Name;
	}

	public static string ChooseNewCharacter()
	{
		List<Character> list = new List<Character>();
		Character[] array = List;
		foreach (Character character in array)
		{
			if (SaveGame.Instance.CharacterOwned(character.Name) && !SaveGame.Instance.CharacterRescued(character.Name) && character.Name != SaveGame.Instance.CurrentCharacter)
			{
				list.Add(character);
			}
		}
		if (list.Count == 0)
		{
			Character[] array2 = List;
			foreach (Character character2 in array2)
			{
				if (SaveGame.Instance.CharacterOwned(character2.Name) && character2.Name != SaveGame.Instance.CurrentCharacter)
				{
					list.Add(character2);
				}
			}
		}
		if (list.Count == 0)
		{
			return Instance.DefaultCharacter;
		}
		return list[Random.Range(0, list.Count)].Name;
	}
}
