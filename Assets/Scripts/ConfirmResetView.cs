public class ConfirmResetView : GuiView
{
	public GuiButton YesButton;

	public GuiButton NoButton;

	protected override void Start()
	{
		YesButton.Click = delegate
		{
			SaveGame.ClearSaveGame();
			if (SingletonMonoBehaviour<Gui>.HasInstance())
			{
				SingletonMonoBehaviour<Gui>.Instance.HideAll();
			}
			SingletonMonoBehaviour<Game>.Instance.Restart();
		};
		NoButton.Click = delegate
		{
			HideAnimated();
		};
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(NoButton);
	}

	public override void HideAnimated()
	{
		base.HideAnimated();
		if (SingletonMonoBehaviour<Gui>.Instance.Ready)
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Gui.Views.SettingsView.Reset);
		}
	}

	public override bool OnMenuButton()
	{
		NoButton.Click();
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
