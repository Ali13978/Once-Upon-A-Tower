using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BuySaveMePackView : GuiView
{
	private enum ActionType
	{
		Wait,
		WaitBuy,
		Cancel,
		Buy
	}

	public TMP_Text Body;

	public GuiButton CancelButton;

	public SaveMePackButton[] PackButtons;

	private List<SaveMePack> Packs = new List<SaveMePack>(3);

	private void ResetButtons()
	{
		CancelButton.Click = null;
		for (int i = 0; i < PackButtons.Length; i++)
		{
			PackButtons[i].Click = null;
		}
	}

	public IEnumerator ShowCoroutine()
	{
		Packs.Clear();
		int missing = Gui.Views.SaveMeView.SaveMeCost - SaveGame.Instance.SaveMeCoins;
		for (int i = 0; i < SaveMePacks.List.Length; i++)
		{
			if (missing <= SaveMePacks.List[i].Size)
			{
				Packs.Add(SaveMePacks.List[i]);
			}
		}
		if (Packs.Count == 0)
		{
			Packs.Add(SaveMePacks.List.Last());
		}
		ActionType actionType2 = ActionType.Wait;
		ActionType actionType;
		for (int j = 0; j < PackButtons.Length; j++)
		{
			SaveMePackButton button = PackButtons[j];
			button.gameObject.SetActive(value: true);
			button.Pack = null;
			button.Click = delegate
			{
				if (button.Pack != null && actionType2 == ActionType.Wait)
				{
					actionType = ActionType.WaitBuy;
					Purchaser.Instance.BuyProduct(button.Pack.ProductId, delegate
					{
						SaveGame.Instance.SaveMeCoins += button.Pack.Size;
						SaveGame.Instance.Save();
						actionType = ActionType.Buy;
					}, delegate
					{
						actionType = ActionType.Wait;
					});
				}
			};
		}
		if (Packs.Count == 1)
		{
			PackButtons[0].gameObject.SetActive(value: false);
			PackButtons[1].Setup(Packs[0]);
			PackButtons[2].gameObject.SetActive(value: false);
		}
		else if (Packs.Count == 2)
		{
			PackButtons[0].gameObject.SetActive(value: false);
			PackButtons[1].Setup(Packs[0]);
			PackButtons[2].Setup(Packs[1]);
		}
		else
		{
			for (int k = 0; k < PackButtons.Length; k++)
			{
				PackButtons[k].Setup(Packs[k]);
			}
		}
		UpdateBodyText();
		ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(PackButtons[(Packs.Count < 3) ? 1 : 0]);
		CancelButton.Click = delegate
		{
			if (actionType2 == ActionType.Wait)
			{
				ResetButtons();
				actionType2 = ActionType.Cancel;
			}
		};
		while (true)
		{
			if (actionType2 == ActionType.Wait)
			{
				yield return null;
				continue;
			}
			if (actionType2 == ActionType.Buy)
			{
				if (SaveGame.Instance.SaveMeCoins >= Gui.Views.SaveMeView.SaveMeCost)
				{
					break;
				}
				actionType2 = ActionType.Wait;
				UpdateBodyText();
			}
			if (actionType2 == ActionType.Cancel)
			{
				break;
			}
			yield return null;
		}
		HideAnimated();
	}

	private void UpdateBodyText()
	{
		Body.text = Body.sceneText.Replace("{missing}", (Gui.Views.SaveMeView.SaveMeCost - SaveGame.Instance.SaveMeCoins).ToString());
	}

	public override bool OnMenuButton()
	{
		if (CancelButton.Click != null)
		{
			CancelButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
