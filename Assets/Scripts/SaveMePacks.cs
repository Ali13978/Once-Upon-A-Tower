using System.Linq;
using UnityEngine;

public class SaveMePacks : ScriptableObject
{
	private static SaveMePack[] list;

	private static SaveMePacks instance;

	public SaveMePack[] SerializeList;

	public static SaveMePack[] List
	{
		get
		{
			if (list == null)
			{
				list = (from p in Instance.SerializeList
					orderby p.Size
					select p).ToArray();
			}
			return list;
		}
	}

	public static SaveMePacks Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load<SaveMePacks>("SaveMePacks");
			}
			return instance;
		}
	}

	public static SaveMePack BySize(int size)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].Size == size)
			{
				return List[i];
			}
		}
		return null;
	}

	public static void Reload()
	{
		list = null;
		instance = null;
	}
}
