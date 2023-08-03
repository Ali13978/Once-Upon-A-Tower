using I2.Loc;
using TMPro;
using UnityEngine;

public class CharacterFreeView : GuiView
{
	public GameObject BackCharacters;

	public TMP_Text CharacterName;

	public CharacterSelectItem SelectAda;

	public GuiButton PlayButton;

	private string characterName;

	protected override void Start()
	{
		base.Start();
		BackCharacters.SetActive(value: false);
		SelectAda.gameObject.SetActive(value: false);
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		characterName = SaveGame.Instance.CurrentCharacter;
		Character character = Characters.ByName(characterName);
		CharacterName.text = ScriptLocalization.PrincessHasEscapedTheTower.Replace("{Character}", character.DisplayName);
		SaveGame.Instance.SetCharacterRescued(characterName, value: false);
		CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(character.Name);
		characterSelectItem.SetupGet();
		characterSelectItem.SetupFrame();
		Gui.Views.CharacterSelect.Show();
		Gui.Views.CharacterSelect.Move(characterSelectItem.SelectButton.gameObject, SelectAda.SelectButton.gameObject);
		Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: false);
		Gui.Views.CharacterSelect.GuiScroll.Stop();
		Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = false;
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(PlayButton);
	}

	public void SwapPictureToFree()
	{
		SaveGame.Instance.SetCharacterRescued(characterName, value: true);
		CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
		characterSelectItem.SetupFrame();
	}

	public override void HideAnimated()
	{
		if (base.Visible && !base.Hiding)
		{
			base.HideAnimated();
			CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
			characterSelectItem.Reset();
			Gui.Views.CharacterSelect.Hide();
			Gui.Views.CharacterSelect.ResetMove();
			Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: true);
			Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = true;
		}
	}

	public override bool OnMenuButton()
	{
		if (PlayButton.Click != null)
		{
			PlayButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
