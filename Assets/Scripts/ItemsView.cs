using UnityEngine;

public class ItemsView : GuiView
{
	public GameObject BombsButton;

	public GameObject Counter;

	protected override void Update()
	{
		base.Update();
		if (!SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready)
		{
			return;
		}
		bool flag = SingletonMonoBehaviour<Game>.Instance.Digger.Bombs > 0 && !Gui.Views.InGameStoreItemView.Visible;
		if (flag && !BombsButton.activeSelf)
		{
			BombsButton.SetActive(value: true);
			if (base.Visible)
			{
				Play("Intro");
			}
		}
		else if (!flag && BombsButton.activeSelf && !IsPlayingSequence("Outro"))
		{
			if (base.Visible)
			{
				Play("Outro", delegate
				{
					BombsButton.SetActive(value: false);
				});
			}
			else
			{
				BombsButton.SetActive(value: false);
			}
		}
		Counter.SetActive(SingletonMonoBehaviour<Game>.Instance.Digger.Bombs > 1);
	}
}
