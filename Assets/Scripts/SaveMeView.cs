using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SaveMeView : GuiView
{
	private enum ActionType
	{
		Wait,
		Dismiss,
		SaveMeWithAd,
		SaveMe
	}

	public TMP_Text TimerText;

	public GuiButton DismissButton;

	public TMP_Text SaveMeCoinsText;

	public GameObject SaveMeCoinsContainer;

	public SaveMeButton SaveMeLeft;

	public SaveMeButton SaveMeRight;

	public SaveMeButton SaveMeCenter;

	public Renderer Timer;

	public int SaveMeCost
	{
		get
		{
			int num = (int)Mathf.Pow(2f, SaveGame.Instance.SaveMeCount);
			if (num == 16)
			{
				num = 15;
			}
			return num;
		}
	}

	public bool CanSave => SaveGame.Instance.TutorialComplete && (CanSaveWithCoins || CanSaveWithAds);

	private bool CanSaveWithCoins => SaveGame.Instance.SaveMeCoins >= SaveMeCost || Purchaser.Instance.CanBuyProduct(SaveMePacks.List[0].ProductId);

	private bool CanSaveWithAds => SaveGame.Instance.SaveMeCount == 0 && SingletonMonoBehaviour<Game>.Instance.CanShowAd;

	private void ResetButtons()
	{
		DismissButton.Click = (SaveMeLeft.Click = (SaveMeRight.Click = (SaveMeCenter.Click = null)));
	}

	public IEnumerator ShowCoroutine(Action onSaved, Action onNotSaved)
	{
		SaveMeButton adButton = null;
		SaveMeButton coinsButton = null;
		if (CanSaveWithAds && CanSaveWithCoins)
		{
			adButton = SaveMeLeft;
			coinsButton = SaveMeRight;
		}
		else if (CanSaveWithAds)
		{
			adButton = SaveMeCenter;
		}
		else
		{
			if (!CanSaveWithCoins)
			{
				onNotSaved();
				yield break;
			}
			coinsButton = SaveMeCenter;
		}
		SaveMeLeft.gameObject.SetActive(adButton != null && coinsButton != null);
		SaveMeRight.gameObject.SetActive(adButton != null && coinsButton != null);
		SaveMeCenter.gameObject.SetActive(adButton == null || coinsButton == null);
		SaveMeCoinsText.text = SaveGame.Instance.SaveMeCoins.ToString();
		SaveMeCoinsContainer.SetActive(CanSaveWithCoins);
		if (TimerText != null)
		{
			TimerText.text = string.Empty;
		}
		ActionType actionType = ActionType.Wait;
		if ((bool)adButton)
		{
			adButton.ConfigureAd();
			adButton.Click = delegate
			{
				ResetButtons();
				actionType = ActionType.SaveMeWithAd;
			};
		}
		if ((bool)coinsButton)
		{
			coinsButton.ConfigureHearts(SaveMeCost);
			coinsButton.Click = delegate
			{
				ResetButtons();
				actionType = ActionType.SaveMe;
			};
		}
		DismissButton.Click = delegate
		{
			ResetButtons();
			actionType = ActionType.Dismiss;
		};
		ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus((!(adButton != null)) ? coinsButton : adButton);
		if (Timer != null)
		{
			Timer.material.SetFloat("_CutVal", 1f);
		}
		while (base.IsPlaying)
		{
			yield return null;
		}
		DateTime endTime = DateTime.Now + GameVars.SaveMeMaxDuration;
		while (DateTime.Now < endTime && actionType == ActionType.Wait)
		{
			float remaining = (float)(endTime - DateTime.Now).TotalSeconds;
			if (TimerText != null)
			{
				int num = Mathf.CeilToInt(remaining);
				TimerText.text = ((num > 3) ? string.Empty : num.ToString());
			}
			if (Timer != null)
			{
				Timer.material.SetFloat("_CutVal", (float)((double)remaining / GameVars.SaveMeMaxDuration.TotalSeconds));
			}
			yield return null;
		}
		if (TimerText != null && DateTime.Now >= endTime)
		{
			TimerText.text = "0";
		}
		if (actionType != ActionType.SaveMeWithAd)
		{
			if (actionType == ActionType.SaveMe)
			{
				if (SaveGame.Instance.SaveMeCoins < SaveMeCost)
				{
					HideAnimated();
					yield return Gui.Views.BuySaveMePackView.ShowCoroutine();
				}
				if (SaveMeCost <= SaveGame.Instance.SaveMeCoins)
				{
					SaveGame.Instance.SaveMeCoins -= SaveMeCost;
					SaveGame.Instance.Save();
					onSaved();
				}
				else
				{
					onNotSaved();
				}
			}
			else
			{
				onNotSaved();
			}
		}
		else
		{
			yield return SingletonMonoBehaviour<Game>.Instance.ShowAdCoroutine(onSaved, onNotSaved);
		}
		HideAnimated();
	}

	public override bool OnMenuButton()
	{
		if (DismissButton.Click != null)
		{
			DismissButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
