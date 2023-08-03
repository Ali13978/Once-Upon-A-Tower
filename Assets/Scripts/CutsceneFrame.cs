using System;
using System.Collections;
using UnityEngine;

public class CutsceneFrame : GuiView
{
	public GuiButton SkipButton;

	public GameObject SkipText;

	public float SkipWaitTime = 4f;

	public Action OnSkip;

	protected override void Start()
	{
		base.Start();
		SkipText.SetActive(value: false);
		SkipButton.Click = delegate
		{
			if (SkipText.activeSelf)
			{
				SkipText.SetActive(value: false);
				if (OnSkip != null)
				{
					OnSkip();
				}
			}
			else
			{
				StartCoroutine(SkipCoroutine());
			}
		};
	}

	public override void Show()
	{
		base.Show();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(SkipButton);
	}

	private IEnumerator SkipCoroutine()
	{
		SkipText.SetActive(value: true);
		yield return new WaitForSeconds(SkipWaitTime);
		SkipText.SetActive(value: false);
	}

	public override bool OnMenuButton()
	{
		SkipButton.Click();
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
