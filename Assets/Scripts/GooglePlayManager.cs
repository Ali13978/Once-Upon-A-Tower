using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GooglePlayManager : SingletonMonoBehaviour<GooglePlayManager>
{
	public enum PlayManagerState
	{
		Uninitialized,
		Activated,
		LoggingIn,
		LoggedIn,
		Saving,
		Loading,
		Opening,
		Size
	}

	private const string HAS_LOGGED_IN_KEY = "GooglePlayHasEverLoggedIn";

	private const string DECLINED_LOGIN_KEY = "GooglePlayHasDeclinedLogIn";

	[HideInInspector]
	private PlayManagerState state;

	private bool saveEnabled = true;

	public PlayManagerState State
	{
		get
		{
			return state;
		}
		private set
		{
			state = value;
		}
	}

	public bool LoggedIn => state >= PlayManagerState.LoggedIn;

	public TimeSpan LoadedDataPlayTime
	{
		get;
		private set;
	}

	public bool Working => state >= PlayManagerState.Saving || state == PlayManagerState.LoggingIn;

	private bool canDoSomething => state == PlayManagerState.LoggedIn;

	public static void Create()
	{
		if (!SingletonMonoBehaviour<GooglePlayManager>.HasInstance())
		{
			GameObject gameObject = new GameObject("GooglePlayManager");
			gameObject.AddComponent<GooglePlayManager>();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
		PlayGamesPlatform.InitializeInstance(configuration);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		state = PlayManagerState.Activated;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public bool HasEverLoggedIn()
	{
		return PlayerPrefs.HasKey("GooglePlayHasEverLoggedIn");
	}

	public bool HasDeclinedLogin()
	{
		return PlayerPrefs.HasKey("GooglePlayHasDeclinedLogIn");
	}

	public void Login()
	{
		state = PlayManagerState.LoggingIn;
		Social.localUser.Authenticate(delegate(bool success)
		{
			if (success)
			{
				state = PlayManagerState.LoggedIn;
				MonoBehaviour.print("logged in");
				PlayerPrefs.SetInt("GooglePlayHasEverLoggedIn", 1);
				if (HasDeclinedLogin())
				{
					PlayerPrefs.DeleteKey("GooglePlayHasDeclinedLogIn");
				}
				PlayerPrefs.Save();
			}
			else
			{
				state = PlayManagerState.Activated;
				MonoBehaviour.print("error login in");
				PlayerPrefs.SetInt("GooglePlayHasDeclinedLogIn", 1);
			}
		});
	}

	public IEnumerator LoginAndLoad(Action callback)
	{
		while (state < PlayManagerState.Activated)
		{
			yield return null;
		}
		Login();
		while (state == PlayManagerState.LoggingIn)
		{
			yield return null;
		}
		if (!LoggedIn)
		{
			callback?.Invoke();
			yield break;
		}
		Load();
		while (Working)
		{
			yield return null;
		}
		callback?.Invoke();
	}

	public void Logout()
	{
		PlayGamesPlatform.Instance.SignOut();
		state = PlayManagerState.Activated;
	}

	public void OpenSaveFile(Action<ISavedGameMetadata> OnOpen = null)
	{
		if (canDoSomething)
		{
			state = PlayManagerState.Opening;
			ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
			MonoBehaviour.print("opening save file from internet");
			savedGame.OpenWithAutomaticConflictResolution("gamedata.sav", DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, delegate(SavedGameRequestStatus status, ISavedGameMetadata save)
			{
				MonoBehaviour.print("callbac from opening savefile");
				state = PlayManagerState.LoggedIn;
				if (status == SavedGameRequestStatus.Success)
				{
					if (OnOpen != null)
					{
						OnOpen(save);
					}
				}
				else
				{
					UnityEngine.Debug.Log("Error opening " + status);
				}
			});
		}
	}

	public void Save(string data, TimeSpan playTime, bool force = false)
	{
		if (saveEnabled && canDoSomething)
		{
			OpenSaveFile(delegate(ISavedGameMetadata save)
			{
				MonoBehaviour.print("total time played " + save.TotalTimePlayed + " " + playTime);
				if (!(save.TotalTimePlayed > playTime) || force)
				{
					state = PlayManagerState.Saving;
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(Application.productName + " Save Game").WithUpdatedPlayedTime(playTime).Build();
					ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
					savedGame.CommitUpdate(save, updateForMetadata, Encoding.Unicode.GetBytes(data), delegate(SavedGameRequestStatus status, ISavedGameMetadata newSave)
					{
						state = PlayManagerState.LoggedIn;
						MonoBehaviour.print("returned from saving a game " + status);
						if (status == SavedGameRequestStatus.Success)
						{
							MonoBehaviour.print("successfuly saved a game");
						}
					});
				}
			});
		}
	}

	private IEnumerator LoadAfterGameReady(string loadedData, TimeSpan playTime)
	{
		SaveGame.Instance.NewerGame(loadedData, playTime);
		if (!SaveGame.Instance.ThereIsANewerSave)
		{
			yield break;
		}
		if (!SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready)
		{
			while (!SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready)
			{
				yield return null;
			}
			SaveGame.Instance.LoadNewerSave();
			if (SingletonMonoBehaviour<Gui>.HasInstance())
			{
				SingletonMonoBehaviour<Gui>.Instance.HideAll();
			}
			SceneManager.LoadScene(0);
		}
		else
		{
			string currentCharacter = SaveGame.Instance.CurrentCharacter;
			int worldLevel = SaveGame.Instance.WorldLevel;
			SaveGame.Instance.LoadNewerSave();
			SaveGame.Instance.CurrentCharacter = currentCharacter;
			SaveGame.Instance.WorldLevel = worldLevel;
		}
	}

	public void Load()
	{
		if (saveEnabled && canDoSomething)
		{
			OpenSaveFile(delegate(ISavedGameMetadata save)
			{
				GooglePlayManager googlePlayManager = this;
				state = PlayManagerState.Loading;
				ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
				savedGame.ReadBinaryData(save, delegate(SavedGameRequestStatus status, byte[] data)
				{
					googlePlayManager.state = PlayManagerState.LoggedIn;
					if (status == SavedGameRequestStatus.Success)
					{
						MonoBehaviour.print("save played time " + save.TotalTimePlayed);
						string @string = Encoding.Unicode.GetString(data);
						googlePlayManager.StartCoroutine(googlePlayManager.LoadAfterGameReady(@string, save.TotalTimePlayed));
					}
				});
			});
		}
	}
}
