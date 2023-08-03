public class PauseButtonView : GuiView
{
	public GuiButton PauseButton;

	protected override void Start()
	{
		base.Start();
		PauseButton.Click = OnPause;
	}

	public void OnPause()
	{
		SingletonMonoBehaviour<Game>.Instance.HideInGameButtons();
		Gui.Views.PauseView.ShowAnimated();
		Gui.Views.MissionsView.ShowAnimated();
		GameTime.Pause();
		SingletonMonoBehaviour<Game>.Instance.UpdateMusicAndSFX();
	}

	public override bool OnMenuButton()
	{
		if (base.Visible && !base.Hiding)
		{
			OnPause();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
