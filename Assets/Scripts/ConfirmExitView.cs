using UnityEngine;

public class ConfirmExitView : GuiView
{
	public GuiButton YesButton;

	public GuiButton NoButton;

	protected override void Start()
	{
		YesButton.Click = delegate
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.Quit();
			}
			HideAnimated();
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
		SaveGame.Instance.Save();
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
