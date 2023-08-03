using I2.Loc;
using TMPro;
using UnityEngine;

public class CharacterGetView : GuiView
{
	public GameObject BackCharacters;

	public TMP_Text CharacterName;

	public TMP_Text Description;

	public CharacterSelectItem SelectAda;

	public GuiButton PlayButton;

	public GuiButton PoseButton;

	private string characterName;

	protected override void Start()
	{
		base.Start();
		BackCharacters.SetActive(value: false);
		SelectAda.gameObject.SetActive(value: false);
		PlayButton.Click = delegate
		{
			if (base.Visible && !base.Hiding)
			{
				HideAnimated();
				SingletonMonoBehaviour<Game>.Instance.LoadCharacter(characterName);
			}
		};
		PoseButton.Down = delegate
		{
			CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
			if (characterSelectItem != null)
			{
				characterSelectItem.ChangePose();
			}
		};
	}

	public void ShowAnimated(string characterName)
	{
		base.ShowAnimated();
		this.characterName = characterName;
		Character character = Characters.ByName(characterName);
		if (character == null)
		{
			UnityEngine.Debug.LogError("Can't find character " + characterName);
		}
		CharacterName.text = ScriptLocalization.YouHaveUnlockedPrincessName.Replace("{Character}", character.DisplayName);
		Description.text = character.Description;
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(PlayButton);
	}

	public void ShowCharacterPicture()
	{
		CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
		characterSelectItem.SetupGet();
		characterSelectItem.SetupFrame();
		Gui.Views.CharacterSelect.Show();
		Gui.Views.CharacterSelect.Move(characterSelectItem.SelectButton.gameObject, SelectAda.SelectButton.gameObject);
		Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: false);
		Gui.Views.CharacterSelect.GuiScroll.Stop();
		Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = false;
	}

	public override void HideAnimated()
	{
		if (base.Visible && !base.Hiding)
		{
			base.Hiding = true;
			base.TouchEnabled = false;
			Play("Outro", delegate
			{
				Hide();
				CharacterSelectItem characterSelectItem = Gui.Views.CharacterSelect.GetCharacterSelectItem(characterName);
				characterSelectItem.Reset();
				Gui.Views.CharacterSelect.Hide();
				Gui.Views.CharacterSelect.ResetMove();
				Gui.Views.CharacterSelect.BackButton.gameObject.SetActive(value: true);
				Gui.Views.CharacterSelect.GuiScroll.TouchEnabled = true;
			});
		}
	}

	public override bool OnMenuButton()
	{
		PlayButton.Click();
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
