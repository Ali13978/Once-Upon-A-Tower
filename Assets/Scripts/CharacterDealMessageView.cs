using I2.Loc;
using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterDealMessageView : GuiView
{
	public TMP_Text Name;

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		Character character = Characters.ByName(SaveGame.Instance.CurrentCharacter);
		Name.text = ScriptLocalization.TryOneEscapeWith.Replace("{Name}", character.DisplayName);
		StartCoroutine(HideOnStarted());
	}

	private IEnumerator HideOnStarted()
	{
		yield return new WaitForSeconds(4f);
		while (!SingletonMonoBehaviour<Game>.Instance.Started || SingletonMonoBehaviour<Game>.Instance.Digger.Coord.Section == 0)
		{
			yield return null;
		}
		HideAnimated();
	}
}
