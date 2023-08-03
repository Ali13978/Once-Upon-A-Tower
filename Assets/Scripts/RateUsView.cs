using System;
using System.Collections;
using UnityEngine;

public class RateUsView : GuiView
{
	private enum ActionType
	{
		Wait,
		Later,
		Never,
		Yes
	}

	public GuiButton LaterButton;

	public GuiButton NeverButton;

	public GuiButton YesButton;

	private bool thankyou;

	public bool ShouldShow => !SaveGame.Instance.RateUsDisabled && (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.isEditor) && Characters.OwnedCount > 1 && SaveGame.Instance.NextRateUsTime < DateTime.Now;

	public IEnumerator ShowCoroutine()
	{
		while (SingletonMonoBehaviour<World>.Instance.IsLoading)
		{
			yield return null;
		}
		if (!ShouldShow || SaveGame.Instance.WorldLevel <= 1)
		{
			yield break;
		}
		if (SingletonMonoBehaviour<GameInput>.HasInstance())
		{
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		}
		SingletonMonoBehaviour<Game>.Instance.HideInGameButtons();
		Show();
		Play("Intro", delegate
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(YesButton);
		});
		ActionType actionType = ActionType.Wait;
		LaterButton.Click = delegate
		{
			ResetButtons();
			actionType = ActionType.Later;
		};
		NeverButton.Click = delegate
		{
			ResetButtons();
			actionType = ActionType.Never;
		};
		YesButton.Click = delegate
		{
			ResetButtons();
			actionType = ActionType.Yes;
		};
		while (actionType == ActionType.Wait)
		{
			yield return null;
		}
		if (actionType != ActionType.Later)
		{
			if (actionType != ActionType.Never)
			{
				if (actionType == ActionType.Yes)
				{
					Yes();
				}
			}
			else
			{
				Never();
			}
		}
		else
		{
			Later();
		}
		if (SingletonMonoBehaviour<GameInput>.HasInstance())
		{
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
		}
		SingletonMonoBehaviour<Game>.Instance.ShowInGameButtons();
	}

	private void ResetButtons()
	{
		LaterButton.Click = (NeverButton.Click = (YesButton.Click = null));
	}

	private void Later()
	{
		SingletonMonoBehaviour<Game>.Instance.ScheduleRateUs(GameVars.TimeBetweenRateUs);
		HideAnimated();
	}

	private void Never()
	{
		SaveGame.Instance.RateUsDisabled = true;
		HideAnimated();
	}

	private void Yes()
	{
		SaveGame.Instance.RateUsDisabled = true;
		try
		{
			Application.OpenURL("market://details?id=com.pomelogames.TowerGame");
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		StartCoroutine(ThankYou());
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused && thankyou)
		{
			thankyou = false;
			Gui.Views.ThankYou.ShowAnimated();
		}
	}

	private IEnumerator ThankYou()
	{
		thankyou = true;
		HideAnimated();
		yield return new WaitForSeconds(0.5f);
		if (thankyou)
		{
			thankyou = false;
			Gui.Views.ThankYou.ShowAnimated();
		}
	}

	public override bool OnMenuButton()
	{
		if (LaterButton.Click != null)
		{
			LaterButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
