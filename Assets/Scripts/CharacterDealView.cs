using System;
using System.Collections;
using TMPro;

public class CharacterDealView : GuiView
{
	private enum ActionType
	{
		Wait,
		Reset,
		Buy
	}

	public TMP_Text Text;

	public GuiButton ResetButton;

	public GuiButton BuyButton;

	public TMP_Text BuyText;

	private void ResetButtons()
	{
		ResetButton.Click = (BuyButton.Click = null);
	}

	public IEnumerator ShowCoroutine(string characterName, Action onSaved, Action onNotSaved)
	{
		Character character = Characters.ByName(characterName);
		string price = Purchaser.Instance.ProductPrice(character.ProductId);
		Text.text = Text.sceneText.Replace("{Name}", character.DisplayName).Replace("{L}", GameVars.CharacterDealSaveMeCoins.ToString());
		BuyText.text = BuyText.sceneText.Replace("{P}", price);
		Gui.Views.CharacterDealMessage.HideAnimated();
		ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(BuyButton);
		ActionType actionType = ActionType.Wait;
		ResetButton.Click = delegate
		{
			ResetButtons();
			actionType = ActionType.Reset;
		};
		BuyButton.Click = delegate
		{
			Purchaser.Instance.BuyProduct(character.ProductId, delegate
			{
				ResetButtons();
				actionType = ActionType.Buy;
			});
		};
		while (actionType == ActionType.Wait)
		{
			yield return null;
		}
		if (actionType == ActionType.Buy)
		{
			SaveGame.Instance.SaveMeCoins += GameVars.CharacterDealSaveMeCoins;
			SaveGame.Instance.Save();
			onSaved();
		}
		else
		{
			onNotSaved();
		}
		HideAnimated();
	}

	public override bool OnMenuButton()
	{
		if (ResetButton.Click != null)
		{
			ResetButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
