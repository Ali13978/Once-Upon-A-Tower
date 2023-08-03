using I2.Loc;
using TMPro;
using UnityEngine;

public class SettingsView : GuiView
{
	public GuiButton Leaderboards;

	public GuiButton Achievements;

	public GuiButton RestorePurchases;

	public GuiButton GooglePlay;

	public GuiButton Reset;

	public GuiButton Back;

	public GuiToggle Music;

	public GuiToggle SFX;

	public GameObject GooglePlayContainer;

	public GameObject RestorePurchasesContainer;

	public TextMeshPro GooglePlayText;

	public GameObject ResetContainer;

	public GuiButton HiddenButton;

	private int hiddenCount;

	protected override void Start()
	{
		base.Start();
		Back.Click = delegate
		{
			HideAnimated();
		};
		Leaderboards.Click = delegate
		{
			GameServices.ShowLeaderboard(UpdateGameServicesTexts);
		};
		Achievements.Click = delegate
		{
			GameServices.ShowAchievements(UpdateGameServicesTexts);
		};
		RestorePurchases.Click = delegate
		{
			Purchaser.Instance.RestorePurchases();
		};
		GooglePlayContainer.SetActive(Gui.Android || Gui.AndroidTV);
		RestorePurchasesContainer.SetActive(Gui.AppleTV || Gui.iOS);
		Music.SetToggle(SaveGame.Instance.Music);
		Music.Click = delegate
		{
			Music.SetToggle(!Music.Toggle);
			SaveGame.Instance.Music = Music.Toggle;
			SingletonMonoBehaviour<Game>.Instance.UpdateMusicAndSFX();
		};
		SFX.SetToggle(SaveGame.Instance.SFX);
		SFX.Click = delegate
		{
			SFX.SetToggle(!SFX.Toggle);
			SaveGame.Instance.SFX = SFX.Toggle;
			SingletonMonoBehaviour<Game>.Instance.UpdateMusicAndSFX();
		};
		GooglePlay.Click = delegate
		{
			if (SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn)
			{
				SingletonMonoBehaviour<GooglePlayManager>.Instance.Logout();
				UpdateGameServicesTexts();
			}
			else
			{
				GameServices.LoadGooglePlayServices(force: true, UpdateGameServicesTexts);
			}
		};
		Reset.Click = delegate
		{
			Gui.Views.ConfirmReset.ShowAnimated();
		};
		HiddenButton.Click = delegate
		{
			hiddenCount++;
			if (hiddenCount > 15)
			{
				ResetContainer.SetActive(value: true);
			}
		};
	}

	public override void Show()
	{
		base.Show();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Leaderboards);
		UpdateGameServicesTexts();
		ResetContainer.SetActive(value: false);
		hiddenCount = 0;
	}

	public override void HideAnimated()
	{
		base.HideAnimated();
		if (SingletonMonoBehaviour<Gui>.Instance.Ready)
		{
			if (Gui.Views.PauseView.Visible)
			{
				SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Gui.Views.PauseView.Options);
			}
			else if (Gui.Views.StartView.Visible)
			{
				SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Gui.Views.StartView.OptionsButton);
			}
		}
	}

	private void UpdateGameServicesTexts()
	{
		if (GooglePlayText.gameObject.activeInHierarchy)
		{
			GooglePlayText.text = ((!SingletonMonoBehaviour<GooglePlayManager>.HasInstance() || !SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn) ? ScriptLocalization.Login : ScriptLocalization.Logout);
		}
	}

	public override bool OnMenuButton()
	{
		Back.Click();
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
