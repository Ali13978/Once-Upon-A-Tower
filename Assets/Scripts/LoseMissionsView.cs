using System.Collections;
using TMPro;
using UnityEngine;

public class LoseMissionsView : GuiView
{
	public GameObject RankContainer;

	public TextMeshPro RankText;

	public GuiButton BackButton;

	public GuiButton NextButton;

	public float MaxFontSize = 5.5f;

	public Transform[] MissionContainer;

	public Transform[] NextMissionContainer;

	public GameObject AllCompleteMessage;

	public AudioSource[] CompleteClip;

	private MissionText[] missionsTexts;

	private MissionText[] nextMissionsTexts;

	private bool missionsDismissed;

	protected override void Start()
	{
		base.Start();
		BackButton.Click = delegate
		{
			missionsDismissed = true;
		};
		StartCoroutine(InitCoroutine());
	}

	private IEnumerator InitCoroutine()
	{
		while (Gui.Views.MissionTextTemplate == null)
		{
			yield return null;
		}
		missionsTexts = new MissionText[3];
		nextMissionsTexts = new MissionText[3];
		for (int i = 0; i < 3; i++)
		{
			Transform parent = MissionContainer[i];
			GameObject gameObject = Object.Instantiate(Gui.Views.MissionTextTemplate.gameObject, parent);
			MissionText component = gameObject.GetComponent<MissionText>();
			missionsTexts[i] = component;
		}
		for (int j = 0; j < 3; j++)
		{
			Transform parent2 = NextMissionContainer[j];
			GameObject gameObject2 = Object.Instantiate(Gui.Views.MissionTextTemplate.gameObject, parent2);
			MissionText component2 = gameObject2.GetComponent<MissionText>();
			nextMissionsTexts[j] = component2;
		}
	}

	private void ClearTexts()
	{
		for (int i = 0; i < 3; i++)
		{
			missionsTexts[i].Clear();
			nextMissionsTexts[i].Clear();
		}
	}

	public bool ShouldShow()
	{
		MissionSet currentMissionSet = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		if (currentMissionSet == null)
		{
			return false;
		}
		MissionStatus[] missions = currentMissionSet.Missions;
		if (currentMissionSet.Completed)
		{
			return true;
		}
		foreach (MissionStatus missionStatus in missions)
		{
			if (missionStatus.Completed && !missionStatus.ShownCompletedOnSet)
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerator ShowAnimatedCoroutine()
	{
		UpdateChallengeRank();
		NextButton.gameObject.SetActive(value: false);
		ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(NextButton);
		ClearTexts();
		MissionSet missionSet = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		MissionSet nextMissionSet = SingletonMonoBehaviour<MissionManager>.Instance.NextMissionSet;
		MissionStatus[] missions = missionSet.Missions;
		AllCompleteMessage.SetActive(nextMissionSet == null);
		for (int j = 0; j < missions.Length; j++)
		{
			MissionText missionText = missionsTexts[j];
			missionText.MissionStatus = missions[j];
			missionText.ShowIncomplete();
			missionText.transform.localPosition = Vector3.one * 1000f;
			if (nextMissionSet != null)
			{
				nextMissionsTexts[j].MissionStatus = nextMissionSet.Missions[j];
				nextMissionsTexts[j].ShowIncomplete();
			}
		}
		yield return null;
		MissionText.CenterAndSize(missionsTexts, MaxFontSize);
		if (nextMissionSet != null)
		{
			MissionText.CenterAndSize(nextMissionsTexts, MaxFontSize);
		}
		yield return null;
		for (int k = 0; k < missions.Length; k++)
		{
			MissionText missionText2 = missionsTexts[k];
			MissionStatus missionStatus = missions[k];
			if (missionStatus.Completed && missionStatus.ShownCompletedOnSet)
			{
				missionText2.ShowComplete();
			}
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		missionsDismissed = false;
		int completeCount = 0;
		for (int l = 0; l < missions.Length; l++)
		{
			if (missions[l].Completed && missions[l].ShownCompletedOnSet)
			{
				completeCount++;
			}
		}
		for (int i = 0; i < missions.Length; i++)
		{
			MissionStatus mission = missions[i];
			if (mission.Completed && !mission.ShownCompletedOnSet)
			{
				mission.ShownCompletedOnSet = true;
				CompleteClip[Mathf.Clamp(completeCount, 0, CompleteClip.Length - 1)].Play();
				completeCount++;
				missionsTexts[i].ShowCompleteAnimated();
				yield return WaitForSecondsOrDismissed(0.4f);
			}
		}
		SingletonMonoBehaviour<MissionManager>.Instance.CheckLevelUpdate();
		if (missionSet.Completed)
		{
			Play("MissionsChange");
		}
		else
		{
			Play("ShowButton");
		}
		bool done = false;
		NextButton.Click = delegate
		{
			done = true;
		};
		while (!done)
		{
			yield return null;
		}
		HideAnimated();
	}

	private IEnumerator WaitForSecondsOrDismissed(float seconds)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < seconds && !missionsDismissed)
		{
			yield return null;
		}
	}

	public void UpdateChallengeRank()
	{
		RankContainer.SetActive(SaveGame.Instance.MissionLevel > 1);
		RankText.text = "x" + SaveGame.Instance.MissionLevel;
	}

	public override bool OnMenuButton()
	{
		if (NextButton.Click != null)
		{
			NextButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
