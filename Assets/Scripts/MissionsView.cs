using System.Collections;
using TMPro;
using UnityEngine;

public class MissionsView : GuiView
{
	public GameObject LevelParent;

	public TextMeshPro LevelText;

	public Transform[] MissionTextParents;

	public MissionText[] MissionTexts;

	public float MaxFontSize = 5.5f;

	public GameObject SetCompleteMessage;

	public GameObject AllCompleteMessage;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(InitCoroutine());
	}

	private IEnumerator InitCoroutine()
	{
		while (Gui.Views.MissionTextTemplate == null)
		{
			yield return null;
		}
		MissionTexts = new MissionText[3];
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject = Object.Instantiate(Gui.Views.MissionTextTemplate.gameObject, MissionTextParents[i]);
			MissionTexts[i] = gameObject.GetComponent<MissionText>();
		}
	}

	public override void ShowAnimated()
	{
		ClearTexts();
		LevelParent.SetActive(SaveGame.Instance.MissionLevel > 1);
		LevelText.text = "x" + SaveGame.Instance.MissionLevel;
		base.ShowAnimated();
		StartCoroutine(ShowAnimatedCoroutine());
	}

	private IEnumerator ShowAnimatedCoroutine()
	{
		yield return ShowSet(SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet);
	}

	private void ClearTexts()
	{
		for (int i = 0; i < MissionTexts.Length; i++)
		{
			MissionTexts[i].Clear();
		}
	}

	private IEnumerator ShowSet(MissionSet missionSet)
	{
		SetCompleteMessage.SetActive(missionSet?.Completed ?? false);
		AllCompleteMessage.SetActive(missionSet == null);
		if (missionSet == null)
		{
			yield break;
		}
		MissionStatus[] missions = missionSet.Missions;
		for (int j = 0; j < missions.Length; j++)
		{
			MissionText missionText = MissionTexts[j];
			missionText.MissionStatus = missions[j];
			missionText.Show();
			missionText.transform.localPosition = Vector3.one * 1000f;
		}
		yield return null;
		MissionText.CenterAndSize(MissionTexts, MaxFontSize);
		yield return null;
		for (int i = 0; i < missions.Length; i++)
		{
			MissionText text = MissionTexts[i];
			MissionStatus mission = missions[i];
			if (mission.Completed)
			{
				if (mission.ShownCompleted)
				{
					text.ShowComplete();
				}
				else
				{
					mission.ShownCompleted = true;
					text.ShowCompleteAnimated();
					yield return new WaitForSeconds(0.5f);
				}
			}
			else
			{
				text.ShowIncomplete();
			}
			yield return new WaitForSecondsRealtime(0.1f);
		}
	}
}
