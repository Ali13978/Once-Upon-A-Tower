public class FortuneDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		if (SingletonMonoBehaviour<Game>.Instance.CanShowAd)
		{
			Game instance = SingletonMonoBehaviour<Game>.Instance;
			WheelView wheelView = Gui.Views.WheelView;
			instance.ShowAd(((GuiView)wheelView).ShowAnimated, delegate
			{
			});
		}
		else
		{
			Gui.Views.WheelView.ShowAnimated();
		}
	}
}
