using System;
using TMPro;
using UnityEngine;

public class MissionCounter : MonoBehaviour
{
	[Serializable]
	public class TypeIcon
	{
		public MissionType Type;

		public GameObject Icon;
	}

	public Transform IconParent;

	public TextMeshPro CountText;

	public TextMeshPro TotalText;

	private TypeIcon[] TypeIcons;

	private MissionType Type;

	private int Count = -1;

	private void Init()
	{
		if (TypeIcons == null || TypeIcons.Length <= 0)
		{
			TypeIcons = new TypeIcon[IconParent.childCount];
			for (int i = 0; i < IconParent.childCount; i++)
			{
				Transform child = IconParent.GetChild(i);
				try
				{
					MissionType type = (MissionType)Enum.Parse(typeof(MissionType), child.name);
					TypeIcons[i] = new TypeIcon
					{
						Type = type,
						Icon = child.gameObject
					};
				}
				catch
				{
					UnityEngine.Debug.LogError("No mission type for icon " + child.name);
				}
			}
		}
	}

	public bool ShouldCount(MissionStatus mission)
	{
		return SaveGame.Instance.TutorialComplete && mission.Mission.Count > 1 && !mission.ShownCompletedOnSet && Find(mission) != null;
	}

	public void Setup(MissionStatus mission)
	{
		if (Type != mission.Mission.Type)
		{
			Type = mission.Mission.Type;
			TypeIcon typeIcon = Find(mission);
			for (int i = 0; i < TypeIcons.Length; i++)
			{
				TypeIcons[i].Icon.SetActive(TypeIcons[i] == typeIcon);
			}
			TotalText.text = mission.Mission.Count.ToString();
		}
		if (Count != mission.Progress)
		{
			Count = Mathf.Min(mission.Progress, mission.Mission.Count);
			CountText.text = Count.ToString();
			TotalText.text = mission.Mission.Count.ToString();
		}
	}

	private TypeIcon Find(MissionStatus mission)
	{
		Init();
		for (int i = 0; i < TypeIcons.Length; i++)
		{
			if (TypeIcons[i].Type == mission.Mission.Type)
			{
				return TypeIcons[i];
			}
		}
		return null;
	}
}
