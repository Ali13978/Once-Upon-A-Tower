using TMPro;
using UnityEngine;

public class CharacterBuyView : GuiView
{
	public GameObject BackCharacters;

	public CharacterSelectItem SelectAda;

	public GuiButton BackButton;

	public GuiButton BuyButton;

	public TMP_Text BuyText;

	public TMP_Text DescriptionText;

	private bool backToCharacterSelect;

	private Character character;

	protected override void Start()
	{
		base.Start();
		BackCharacters.SetActive(value: false);
		SelectAda.gameObject.SetActive(value: false);
		BackButton.Click = ((GuiView)this).HideAnimated;
		BuyButton.Click = delegate
		{
			Purchaser.Instance.BuyProduct(character.ProductId, delegate
			{
				backToCharacterSelect = false;
				HideAnimated();
				Gui.Views.CharacterGet.ShowAnimated(character.Name);
			});
		};
	}

	public void ShowAnimated(string characterName)
	{
		base.ShowAnimated();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(BuyButton);
		character = Characters.ByName(characterName);
		BuyButton.gameObject.SetActive(Purchaser.Instance.CanBuyProduct(character.ProductId));
		string newValue = Purchaser.Instance.ProductPrice(character.ProductId);
		BuyText.text = BuyText.sceneText.Replace("{price}", newValue);
		CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
		characterSelectItem.SetupBuy();
		DescriptionText.text = character.Description;
		backToCharacterSelect = true;
		Gui.Views.CharacterSelect.MoveAnimated(characterSelectItem.SelectButton.gameObject, SelectAda.SelectButton.gameObject);
		Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: false);
		Gui.Views.CharacterSelect.GuiScroll.Stop();
		Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = false;
	}

	public override void HideAnimated()
	{
		if (base.Visible && !base.Hiding)
		{
			base.HideAnimated();
			CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(character.Name);
			characterSelectItem.Reset();
			if (backToCharacterSelect)
			{
				SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(characterSelectItem.SelectButton, scrollEnabled: false);
				Gui.Views.CharacterSelect.ResetMoveAnimated();
			}
			Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: true);
			Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = true;
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
