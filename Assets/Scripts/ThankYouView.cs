using System.Collections;

public class ThankYouView : GuiView
{
	public override void ShowAnimated()
	{
		base.ShowAnimated();
		StartCoroutine(WaitUntilMove());
	}

	private IEnumerator WaitUntilMove()
	{
		Coord coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
		while (coord == SingletonMonoBehaviour<Game>.Instance.Digger.Coord)
		{
			yield return null;
		}
		HideAnimated();
	}
}
