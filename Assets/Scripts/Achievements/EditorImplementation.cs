using System.Collections;
using UnityEngine;

namespace Achievements
{
	public class EditorImplementation : ImplementationBase
	{
		private bool showingAchievement;

		private string currentAchievement = string.Empty;

		public override IEnumerator ShowAchievement(AchievementType achievement)
		{
			showingAchievement = true;
			currentAchievement = "Achievement: " + achievement.ToString();
			UnityEngine.Debug.Log(currentAchievement);
			yield return new WaitForSeconds(4f);
			showingAchievement = false;
		}

		public override void OnGUI()
		{
			if (showingAchievement)
			{
				GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
				gUIStyle.fontSize = 20;
				gUIStyle.fontStyle = FontStyle.Bold;
				GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUI.backgroundColor = Color.white;
				GUI.color = Color.red;
				GUILayout.Box(currentAchievement, gUIStyle);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
	}
}
