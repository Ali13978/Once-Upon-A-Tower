using I2.Loc;
using Rewired;
using System;
using TMPro;
using UnityEngine;

public class StartView : GuiView
{
	public GuiButton CharactersButton;

	public GuiButton WheelButton;

	public GuiButton PlayButton;

	public GuiButton OptionsButton;

	public GameObject WheelButtonContainer;

	public GameObject PlayButtonContainer;

	public Material MaterialEnabled;

	public Material MaterialDisabled;

	public TMP_Text LuckyText;

	private float elapsedTime;

	public bool ShouldEnableWheel => SaveGame.Instance.NextLuckyTime < DateTime.Now && SingletonMonoBehaviour<SupersonicManager>.Instance.VRAvailable;

	private bool WheelButtonEnabled => WheelButton.Click != null;

	protected override void Start()
	{
		base.Start();
		CharactersButton.Click = delegate
		{
			Gui.Views.CharacterSelect.ShowAnimated();
		};
		WheelButton.Click = OnWheelButtonClick;
		PlayButton.Click = delegate
		{
			SingletonMonoBehaviour<Game>.Instance.StartRun();
		};
		OptionsButton.Click = delegate
		{
			Gui.Views.SettingsView.ShowAnimated();
		};
	}

	protected override void Update()
	{
		base.Update();
		if (!base.Visible)
		{
			return;
		}
		if ((ShouldEnableWheel && !WheelButtonEnabled) || (!ShouldEnableWheel && WheelButtonEnabled))
		{
			UpdateWheelButton();
		}
		if (!WheelButtonEnabled)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1f)
			{
				elapsedTime = 0f;
				UpdateLuckyText();
			}
		}
	}

	private void UpdateWheelButton()
	{
		if (ShouldEnableWheel)
		{
			EnableWheelButton();
		}
		else
		{
			DisableWheelButton();
			elapsedTime = 0f;
		}
		UpdateLuckyText();
	}

	private void OnWheelButtonClick()
	{
		DisableWheelButton();
		if (SingletonMonoBehaviour<Game>.Instance.CanShowAd)
		{
			SingletonMonoBehaviour<Game>.Instance.ShowAd(ShowWheel, EnableWheelButton);
		}
		else
		{
			ShowWheel();
		}
	}

	private void ShowWheel()
	{
		Gui.Views.WheelView.ShowAnimated();
	}

	private void EnableWheelButton()
	{
		WheelButton.Click = OnWheelButtonClick;
		WheelButton.TouchEnabled = true;
		MeshRenderer[] componentsInChildren = WheelButton.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			Material[] materials = meshRenderer.materials;
			materials[0] = MaterialEnabled;
			meshRenderer.materials = materials;
		}
		ParticleSystem[] componentsInChildren2 = WheelButton.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren2)
		{
			particleSystem.Play();
		}
	}

	private void DisableWheelButton()
	{
		WheelButton.Click = null;
		WheelButton.TouchEnabled = false;
		MeshRenderer[] componentsInChildren = WheelButton.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			Material[] materials = meshRenderer.materials;
			materials[0] = MaterialDisabled;
			meshRenderer.materials = materials;
		}
		ParticleSystem[] componentsInChildren2 = WheelButton.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren2)
		{
			particleSystem.Stop();
		}
	}

	private void UpdateLuckyText()
	{
		if (SaveGame.Instance.NextLuckyTime < DateTime.Now)
		{
			LuckyText.text = ScriptLocalization.Fortune;
			return;
		}
		TimeSpan timeSpan = SaveGame.Instance.NextLuckyTime - DateTime.Now;
		LuckyText.text = string.Format("{1:D1}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	public override void Show()
	{
		Player systemPlayer = ReInput.players.GetSystemPlayer();
		bool flag = (systemPlayer != null && systemPlayer.controllers.joystickCount > 0) || !SingletonMonoBehaviour<Gui>.Instance.Touchable;
		WheelButtonContainer.SetActive(!flag);
		PlayButtonContainer.SetActive(flag);
		base.Show();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(PlayButton);
		UpdateWheelButton();
	}

	public void Reload()
	{
		if (base.Visible && !base.Hiding)
		{
			base.Hiding = true;
			base.TouchEnabled = false;
			Play("Outro", delegate
			{
				Hide();
				ShowAnimated();
			});
		}
	}
}
