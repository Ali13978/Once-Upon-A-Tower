using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectView : GuiView
{
	public GuiButton BackButton;

	public GuiScroll GuiScroll;

	public Transform CharactersParent;

	public Material LockedMaterial;

	public Material UnlockedMaterial;

	public Material FreeMaterial;

	private IEnumerator moveCoroutine;

	private float smoothTime = 0.15f;

	private Vector3 velocity;

	private Vector3 scaleVelocity;

	private Vector3 previousPosition;

	private Vector3 originalLocalPosition;

	private Vector3 originalLocalScale;

	public AudioSourceController Loop;

	public List<CharacterSelectItem> Items;

	protected override void Awake()
	{
		base.Awake();
		Items = GetComponentsInChildren<CharacterSelectItem>(includeInactive: true).ToList();
	}

	protected override void Start()
	{
		base.Start();
		BackButton.Click = ((GuiView)this).HideAnimated;
		originalLocalPosition = CharactersParent.localPosition;
		originalLocalScale = CharactersParent.localScale;
		for (int i = 0; i < CharactersParent.childCount; i++)
		{
			CharacterSelectItem component = CharactersParent.GetChild(i).GetComponent<CharacterSelectItem>();
			if (!(component != null))
			{
				continue;
			}
			Character character = Characters.ByName(component.CharacterName);
			if (character != null)
			{
				component.NamePlate.text = component.NamePlate.sceneText.Replace("{Character}", character.DisplayName);
				continue;
			}
			for (int j = 0; j < component.transform.childCount; j++)
			{
				component.transform.GetChild(j).gameObject.SetActive(value: false);
			}
			component.transform.Find("States").gameObject.SetActive(value: true);
			component.SetupFrame();
			UnityEngine.Object.Destroy(component);
		}
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		Loop.TargetVolume = 1f;
		if (SingletonMonoBehaviour<GameInput>.HasInstance())
		{
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		}
		CharacterSelectItem[] componentsInChildren = CharactersParent.GetComponentsInChildren<CharacterSelectItem>();
		CharacterSelectItem[] array = componentsInChildren;
		foreach (CharacterSelectItem item in array)
		{
			item.SetupFrame();
			item.SelectButton.Click = delegate
			{
				if (SaveGame.Instance.CharacterOwned(item.CharacterName))
				{
					HideAnimated();
					SingletonMonoBehaviour<Game>.Instance.LoadCharacter(item.CharacterName);
				}
				else
				{
					Gui.Views.CharacterBuy.ShowAnimated(item.CharacterName);
				}
			};
			CharacterSelectItem item_ = item;
			item.SelectButton.Down = delegate
			{
				item_.ChangePose();
			};
		}
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(componentsInChildren[0].SelectButton);
		ResetMove();
	}

	public override void HideAnimated()
	{
		base.HideAnimated();
		if (SingletonMonoBehaviour<Gui>.Instance.Ready)
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(Gui.Views.StartView.CharactersButton);
		}
		Loop.TargetVolume = 0f;
	}

	public override void Hide()
	{
		base.Hide();
		if (SingletonMonoBehaviour<GameInput>.HasInstance())
		{
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
		}
	}

	public CharacterSelectItem GetCharacterSelectItem(string characterName)
	{
		return Items.Find((CharacterSelectItem i) => i.CharacterName == characterName);
	}

	private void CalculateMove(GameObject source, GameObject target, out Vector3 targetPosition, out Vector3 targetLocalScale)
	{
		previousPosition = CharactersParent.position;
		BoxCollider2D component = target.GetComponent<BoxCollider2D>();
		Vector3 lossyScale = target.transform.lossyScale;
		float x = lossyScale.x;
		Vector2 size = component.size;
		float num = x * size.x;
		Vector3 lossyScale2 = target.transform.lossyScale;
		float y = lossyScale2.y;
		Vector2 size2 = component.size;
		float num2 = y * size2.y;
		BoxCollider2D component2 = source.GetComponent<BoxCollider2D>();
		Vector3 lossyScale3 = source.transform.lossyScale;
		float x2 = lossyScale3.x;
		Vector2 size3 = component2.size;
		float num3 = x2 * size3.x;
		Vector3 lossyScale4 = source.transform.lossyScale;
		float y2 = lossyScale4.y;
		Vector2 size4 = component2.size;
		float num4 = y2 * size4.y;
		targetLocalScale = originalLocalScale * Mathf.Min(num / num3, num2 / num4);
		CharactersParent.localScale = targetLocalScale;
		targetPosition = previousPosition + (target.transform.position - source.transform.position);
		CharactersParent.localScale = originalLocalScale;
		targetPosition.z = previousPosition.z;
		targetLocalScale.z = originalLocalScale.z;
	}

	public void Move(GameObject source, GameObject target)
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		Vector3 targetPosition;
		Vector3 targetLocalScale;
		CalculateMove(source, target, out targetPosition, out targetLocalScale);

		CharactersParent.position = targetPosition;
		CharactersParent.localScale = targetLocalScale;
	}

	public void ResetMove()
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		CharactersParent.localPosition = originalLocalPosition;
		CharactersParent.localScale = originalLocalScale;
		GuiScroll.ResetContentOrigin();
	}

	public void MoveAnimated(GameObject source, GameObject target)
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		Vector3 targetPosition;
		Vector3 targetLocalScale;
		CalculateMove(source, target, out targetPosition, out targetLocalScale);

		StartCoroutine(moveCoroutine = MoveCoroutine(targetPosition, targetLocalScale));
	}

	public void ResetMoveAnimated()
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		StartCoroutine(moveCoroutine = MoveCoroutine(previousPosition, originalLocalScale));
	}

	private IEnumerator MoveCoroutine(Vector3 targetPosition, Vector3 targetLocalScale)
	{
		Transform content = CharactersParent;
		while (Vector3.Distance(content.position, targetPosition) > 0.01f || Vector3.Distance(content.localScale, targetLocalScale) > 0.01f)
		{
			content.position = Vector3.SmoothDamp(content.position, targetPosition, ref velocity, smoothTime);
			content.localScale = Vector3.SmoothDamp(content.localScale, targetLocalScale, ref scaleVelocity, smoothTime);
			yield return null;
		}
		content.position = targetPosition;
		content.localScale = targetLocalScale;
		moveCoroutine = null;
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
