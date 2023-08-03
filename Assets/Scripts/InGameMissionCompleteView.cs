using System.Collections;
using UnityEngine;

public class InGameMissionCompleteView : GuiView
{
	public Transform TextParent;

	public AudioSource[] CompleteClip;

	private MissionText Text;

	public void ShowMissionCompleted(MissionStatus status)
	{
		if (Text == null)
		{
			Text = Object.Instantiate(Gui.Views.MissionTextTemplate.gameObject, TextParent).GetComponent<MissionText>();
		}
		Text.Clear();
		ShowAnimated();
		StartCoroutine(ShowMissionCompletedCoroutine(status));
	}

	private IEnumerator ShowMissionCompletedCoroutine(MissionStatus mission)
	{
		yield return null;
		Text.MissionStatus = mission;
		Text.ShowIncomplete();
		mission.ShownCompleted = true;
		yield return null;
		Text.Center();
		yield return new WaitForSeconds(0.3f);
		int completeCount = 0;
		MissionSet missions = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		if (missions != null)
		{
			for (int i = 0; i < missions.Missions.Length; i++)
			{
				if (missions.Missions[i].Completed)
				{
					completeCount++;
				}
			}
		}
		CompleteClip[Mathf.Clamp(completeCount - 1, 0, CompleteClip.Length - 1)].Play();
		Text.ShowCompleteAnimated();
		yield return new WaitForSecondsRealtime(2f);
		HideAnimated();
	}
}
