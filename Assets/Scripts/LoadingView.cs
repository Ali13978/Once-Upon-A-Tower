public class LoadingView : GuiView
{
	public override void ShowAnimated()
	{
		base.ShowAnimated();
		StopSingle("Loading");
		PlaySingle("Loading");
	}
}
