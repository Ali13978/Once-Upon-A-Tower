public class GameCenterDisabledView : GuiView
{
	public GuiButton BackButton;

	protected override void Start()
	{
		base.Start();
		BackButton.Click = HideAnimated;
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(BackButton);
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
