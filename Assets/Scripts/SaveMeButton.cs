using TMPro;
using UnityEngine;

public class SaveMeButton : GuiButton
{
	public GameObject AdContainer;

	public GameObject HeartsContainer;

	public TextMeshPro HeartsText;

	public void ConfigureAd()
	{
		if (AdContainer != null)
		{
			AdContainer.SetActive(value: true);
		}
		if (HeartsContainer != null)
		{
			HeartsContainer.SetActive(value: false);
		}
	}

	public void ConfigureHearts(int cost)
	{
		if (AdContainer != null)
		{
			AdContainer.SetActive(value: false);
		}
		if (HeartsContainer != null)
		{
			HeartsContainer.SetActive(value: true);
		}
		if (HeartsText != null)
		{
			HeartsText.text = HeartsText.sceneText.Replace("{C}", cost.ToString());
		}
	}
}
