using System.Collections;
using UnityEngine;

public class SupersonicEditorFallback : AdManager
{
	private int videosAmount = 3;

	private int interstitialAmount = 3;

	private bool showingVR;

	private bool showingIS;

	private bool gotReward;

	public override bool ISAvailable => true;

	public override bool VRAvailable => videosAmount > 0;

	public override bool GotReward => gotReward;

	public override IEnumerator ShowInterstitial()
	{
		base.ShowedIS = false;
		showingIS = true;
		interstitialAmount--;
		while (showingIS)
		{
			yield return null;
		}
	}

	public override IEnumerator ShowVideo()
	{
		showingVR = true;
		base.ShowedVR = false;
		gotReward = false;
		videosAmount--;
		while (showingVR)
		{
			yield return null;
		}
	}

	public override void RunGui()
	{
		if (showingVR || showingIS)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixels(new Color[1]
			{
				Color.magenta
			});
			texture2D.Apply();
			GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty, new GUIStyle
			{
				normal = new GUIStyleState
				{
					background = texture2D
				}
			});
			GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.fontSize = 40;
			GUILayout.Label("Estas viendo un video ad", gUIStyle);
			gUIStyle = new GUIStyle(GUI.skin.button);
			gUIStyle.fontSize = 40;
			if (GUILayout.Button("Visto", gUIStyle))
			{
				bool showedIS = base.ShowedVR = true;
				base.ShowedIS = showedIS;
				showingIS = (showingVR = false);
				gotReward = true;
			}
			if (GUILayout.Button("No visto", gUIStyle))
			{
				showingIS = (showingVR = false);
			}
			if (GUILayout.Button("Error", gUIStyle))
			{
				showingIS = (showingVR = false);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}
}
