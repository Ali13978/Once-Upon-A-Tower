using GooglePlayGames;
using System;
using UnityEngine;

public class GameServices
{
	public static bool Disabled
	{
		get;
		private set;
	}

	public static void Initialize()
	{
		Disabled = false;
		LoadGooglePlayServices(force: false);
	}

	public static void PostScore(int score)
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportScore(score, "CgkIyurnu4kPEAIQAg", delegate(bool success)
			{
				if (!success && SaveGame.Instance.BestPendingReportScore < score)
				{
					SaveGame.Instance.BestPendingReportScore = score;
				}
			});
		}
	}

	public static void ShowLeaderboard(Action afterShow = null)
	{
		SingletonMonoBehaviour<Gui>.Instance.Enabled = false;
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		Action callback = delegate
		{
			SingletonMonoBehaviour<Gui>.Instance.Enabled = true;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
			if (afterShow != null)
			{
				afterShow();
			}
		};
		Action action = delegate
		{
			if (SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn)
			{
				PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIyurnu4kPEAIQAg");
			}
			callback();
		};
		if (!SingletonMonoBehaviour<GooglePlayManager>.HasInstance() || !SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn)
		{
			LoadGooglePlayServices(true, action);
		}
		else
		{
			action();
		}
	}

	public static void ShowAchievements(Action afterShow = null)
	{
		SingletonMonoBehaviour<Gui>.Instance.Enabled = false;
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		Action callback = delegate
		{
			SingletonMonoBehaviour<Gui>.Instance.Enabled = true;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
			if (afterShow != null)
			{
				afterShow();
			}
		};
		Action action = delegate
		{
			if (SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn)
			{
				PlayGamesPlatform.Instance.ShowAchievementsUI();
			}
			callback();
		};
		if (!SingletonMonoBehaviour<GooglePlayManager>.HasInstance() || !SingletonMonoBehaviour<GooglePlayManager>.Instance.LoggedIn)
		{
			LoadGooglePlayServices(true, action);
		}
		else
		{
			action();
		}
	}

	public static void LoadGooglePlayServices(bool force, Action afterLoad = null)
	{
		SingletonMonoBehaviour<Gui>.Instance.Enabled = false;
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		Action action = delegate
		{
			SingletonMonoBehaviour<Gui>.Instance.Enabled = true;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
			if (afterLoad != null)
			{
				afterLoad();
			}
		};
		UnityEngine.Debug.Log("GPG Loading");
		GooglePlayManager.Create();
		GooglePlayManager instance = SingletonMonoBehaviour<GooglePlayManager>.Instance;
		if (force || !SingletonMonoBehaviour<GooglePlayManager>.Instance.HasDeclinedLogin())
		{
			UnityEngine.Debug.Log("GPG LoginAndLoad");
			instance.StartCoroutine(instance.LoginAndLoad(action));
		}
		else
		{
			UnityEngine.Debug.Log("GPG HasDeclinedLogin");
			action();
		}
	}
}
