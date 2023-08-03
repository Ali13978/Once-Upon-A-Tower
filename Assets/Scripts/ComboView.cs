using System.Collections;
using TMPro;
using UnityEngine;

public class ComboView : GuiView
{
	public GuiTrack ComboTextParent;

	public TextMeshPro ComboText;

	public void NotifyCombo(int count, Vector3 position)
	{
		Show();
		ComboTextParent.Point = position;
		ComboText.text = ComboText.sceneText.Replace("{n}", count.ToString());
		StartCoroutine(PlayCoroutine());
	}

	private IEnumerator PlayCoroutine()
	{
		yield return PlayInCoroutine("Combo");
		ComboTextParent.Point = Vector3.zero;
	}
}
