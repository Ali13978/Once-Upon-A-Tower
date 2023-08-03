public class StoreProcessingView : GuiView
{
	public GuiButton BackButton;

	private bool showingError;

	protected override void Start()
	{
		base.Start();
		BackButton.Click = Back;
	}

	private void Back()
	{
		if (showingError && base.Visible && !base.Hiding)
		{
			HideAnimated();
		}
	}

	public override void ShowAnimated()
	{
		StopCurrentSequence();
		base.Show();
		Play("Intro", delegate
		{
			Play("WaitLoop");
		});
		showingError = false;
	}

	public void ShowError()
	{
		StopCurrentSequence();
		base.Show();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(BackButton);
		Play("Error");
		showingError = true;
	}

	public override void HideAnimated()
	{
		base.HideAnimated();
		if (SingletonMonoBehaviour<Gui>.Instance.Ready && Gui.Views.SettingsView.Visible)
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Gui.Views.SettingsView.RestorePurchases);
		}
	}

	public override bool OnMenuButton()
	{
		BackButton.Click();
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
