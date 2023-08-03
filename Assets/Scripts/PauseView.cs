using UnityEngine;

public class PauseView : GuiView
{
	public GuiButton Restart;

	public GuiButton Continue;

	public GuiButton Options;

	public MeshRenderer Background;

	public RenderTexture BackTexture;

	public Material BlurMaterial;

	public int BlurPasses = 30;

	private int blurPasses;

	protected override void Start()
	{
		base.Start();
		BlurMaterial = new Material(BlurMaterial);
		Restart.Click = delegate
		{
			SingletonMonoBehaviour<Game>.Instance.Restart();
		};
		Continue.Click = OnContinue;
		Options.Click = delegate
		{
			Gui.Views.SettingsView.ShowAnimated();
		};
	}

	private void CreateRenderTexture()
	{
		if (!(BackTexture != null))
		{
			BackTexture = new RenderTexture(Screen.width, Screen.height, 24);
		}
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Continue);
		CreateRenderTexture();
		BackTexture.DiscardContents();
		SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera.targetTexture = BackTexture;
		SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera.Render();
		RenderTexture.active = BackTexture;
		SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera.targetTexture = null;
		RenderTexture.active = null;
		Background.sharedMaterial.mainTexture = BackTexture;
		blurPasses = BlurPasses;
	}

	protected override void Update()
	{
		base.Update();
		if (blurPasses > 0)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(BackTexture.width, BackTexture.height, 0, BackTexture.format);
			BlurMaterial.SetFloat("_Aspect", SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera.aspect);
			BlurMaterial.SetFloat("_Size", 1f / (float)Screen.height);
			temporary.DiscardContents();
			Graphics.Blit(BackTexture, temporary, BlurMaterial);
			BackTexture.DiscardContents();
			Graphics.Blit(temporary, BackTexture, BlurMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			blurPasses--;
		}
	}

	private void OnContinue()
	{
		SingletonMonoBehaviour<Game>.Instance.ShowInGameButtons();
		Gui.Views.PauseView.HideAnimated();
		Gui.Views.MissionsView.HideAnimated();
		GameTime.Resume();
		SingletonMonoBehaviour<Game>.Instance.UpdateMusicAndSFX();
	}

	public override bool OnMenuButton()
	{
		if (base.Visible && !base.Hiding)
		{
			OnContinue();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
