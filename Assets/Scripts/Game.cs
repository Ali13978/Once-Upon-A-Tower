using Fabric.Crashlytics;
using Flux;
using GameAnalyticsSDK;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : SingletonMonoBehaviour<Game>
{
	public Digger Digger;

	public Dragon Dragon;

	public GameCamera GameCamera;

	public AudioMixer AudioMixer;

	public ParticleSystem ObjectOnFireParticles;

	public Action<Noise> OnNoise;

	public string CurrentWorld;

	public WorldCustomizer CurrentCustomizer;

	public LightsPostProcessor LightsPostProcessor;

	public List<ItemDef> Items;

	public bool Started;

	public int LevelKillCount;

	public int EnemiesLeftAliveInLevel;

	public int CoinsGrabbed;

	public GameObject ChickenPrefab;

	public Chicken Chicken;

	public bool LowPerformance;

	private List<GuiView> FullscreenViews = new List<GuiView>();

	private bool ready;

	private float lastKilledTime;

	[HideInInspector]
	public int Combo;

	private bool introShownThisSession;

	private bool lastVisibleFullscreenView;

	private List<Character> choosableCharacters;

	[CompilerGenerated]
	private static UnityAction<Scene, LoadSceneMode> _003C_003Ef__mg_0024cache0;

	public bool Ready
	{
		get
		{
			return ready && SingletonMonoBehaviour<World>.Instance.Ready;
		}
		set
		{
			ready = value;
		}
	}

	private bool NeverScheduledRateUs => SaveGame.Instance.NextRateUsTime == DateTime.MaxValue;

	public bool CanShowAd => SingletonMonoBehaviour<SupersonicManager>.Instance.VRAvailable;

	protected override void Awake()
	{
		base.Awake();
		LowPerformance = Util.IsLowPerformanceDevice;
		Shader.EnableKeyword((!LowPerformance) ? "ENABLE_LIGHTMAP" : "DISABLE_LIGHTMAP");
	}

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error)
		{
			Crashlytics.RecordCustomException("LogError", condition, stackTrace);
		}
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		Time.fixedDeltaTime = 1f / (float)Application.targetFrameRate;
		if (ResourceManager.pInstance == null)
		{
			UnityEngine.Debug.LogError("Can't disable I2 ResourceManager sceneLoaded");
		}
		SceneManager.sceneLoaded -= ResourceManager.MyOnLevelWasLoaded;
		Purchaser.Instance.InitializePurchasing();
        //GameServices.Initialize();
        StartCoroutine(LoadGame());
		SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
		{
			FullscreenViews = new List<GuiView>
			{
				Gui.Views.CharacterSelect,
				Gui.Views.LoseView,
				Gui.Views.LoseMissionsView,
				Gui.Views.SettingsView,
				Gui.Views.CharacterGet,
				Gui.Views.WheelView
			};
		});
	}

	public bool FullscreenView()
	{
		for (int i = 0; i < FullscreenViews.Count; i++)
		{
			GuiView guiView = FullscreenViews[i];
			if (guiView.Visible && !guiView.IsPlayingSequence("Intro") && !guiView.Hiding)
			{
				return true;
			}
		}
		return false;
	}

	public bool FullscreenViewOrTransition()
	{
		for (int i = 0; i < FullscreenViews.Count; i++)
		{
			GuiView guiView = FullscreenViews[i];
			if (guiView.Visible && !guiView.Hiding)
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		bool flag = FullscreenViewOrTransition() || !Ready;
		if (lastVisibleFullscreenView != flag)
		{
			lastVisibleFullscreenView = flag;
			int num = (!SaveGame.Instance.SFX || flag) ? (-80) : 0;
			StartCoroutine(FadeMixer("worldVolume", num, 0.4f));
		}
		SaveGame.Instance.Update();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
		{
		}
	}

	public Character ChooseRandomCharacter()
	{
		int num = Characters.List.Count((Character c) => SaveGame.Instance.CharacterOwned(c.Name));
		if (choosableCharacters == null)
		{
			choosableCharacters = new List<Character>();
		}
		choosableCharacters.Clear();
		Character[] list = Characters.List;
		foreach (Character character in list)
		{
			if (!Characters.IsSpecial(character.Name) && character.Name != Characters.Instance.DefaultCharacter && character.MinPreviousCharacters <= num)
			{
				choosableCharacters.Add(character);
			}
		}
		bool flag = false;
		for (int j = 0; j < choosableCharacters.Count * 10; j++)
		{
			Character character2 = choosableCharacters[UnityEngine.Random.Range(0, choosableCharacters.Count)];
			if (character2.SceneName != Digger.gameObject.scene.name && (flag || !SaveGame.Instance.CharacterOwned(character2.Name)))
			{
				return character2;
			}
		}
		return choosableCharacters[UnityEngine.Random.Range(0, choosableCharacters.Count)];
	}

	public void ScheduleRateUs(TimeSpan timeSpan)
	{
		SaveGame.Instance.NextRateUsTime = DateTime.Now + timeSpan;
	}

	private IEnumerator LoadGame()
	{
		Ready = false;
		UpdateMusicAndSFX();
		AudioListener.pause = false;
		LevelKillCount = 0;
		EnemiesLeftAliveInLevel = 0;
		CoinsGrabbed = 0;
		ResetFadeTime();
		SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
		{
			foreach (GuiView item in Gui.Views.All)
			{
				if (!(item is Splash))
				{
					item.HideAnimated();
				}
			}
		});
		CurrentCustomizer.DayCycle = (float)DateTime.Now.TimeOfDay.TotalSeconds * CurrentCustomizer.DayCycleSpeed;
		FixCurrentCharacter();
		yield return LoadCharacter();
		yield return LoadDragon();
		Digger.Walker.enabled = false;
		while (!SingletonMonoBehaviour<World>.Instance.Ready)
		{
			yield return null;
		}
		Digger.Initialize();
		SingletonMonoBehaviour<GiftManager>.Instance.Refresh();
		if (SaveGame.Instance.WorldLevel == 1 && SaveGame.Instance.TutorialComplete && GameCamera.TargetFrame != null)
		{
			Ready = true;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
			yield return new WaitForSeconds(0.5f);
			GameCamera.TargetFrame = null;
			GameCamera.SmoothTime *= 2f;
			GameCamera.MaxSpeed /= 2f;
			float startTime = Time.time;
			while ((!GameCamera.Stable || Time.time - startTime < 0.5f) && Time.time - startTime < 5f)
			{
				yield return null;
			}
			GameCamera.SmoothTime /= 2f;
			GameCamera.MaxSpeed *= 2f;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
		}
		else
		{
			GameCamera.TargetFrame = null;
			GameCamera.Focus();
		}
		if (SaveGame.Instance.TutorialComplete)
		{
			if (SingletonMonoBehaviour<GiftManager>.Instance.NeverScheduled)
			{
				SingletonMonoBehaviour<GiftManager>.Instance.Schedule(GameVars.FirstTimeBetweenGifts);
			}
			if (NeverScheduledRateUs)
			{
				ScheduleRateUs(GameVars.FirstTimeBetweenRateUs);
			}
			bool guiready = false;
			SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
			{
				guiready = true;
				Gui.Views.StartView.ShowAnimated();
				Gui.Views.MissionsView.ShowAnimated();
			});
			while (!guiready)
			{
				yield return null;
			}
		}
		if (SaveGame.Instance.WorldLevel > 1)
		{
			StartRun();
		}
		Digger.Walker.enabled = true;
		GameCamera.enabled = true;
		Ready = true;
		if (!SaveGame.Instance.TutorialComplete && !introShownThisSession)
		{
			introShownThisSession = true;
			StartCoroutine(IntroCutscene());
		}
	}

	private IEnumerator IntroCutscene()
	{
		TileMap firstSection = SingletonMonoBehaviour<World>.Instance.GetSection(0);
		if (firstSection == null)
		{
			yield break;
		}
		FSequence introSequence = firstSection.GetComponentInChildren<FSequence>();
		if (!(introSequence == null))
		{
			Dragon.gameObject.SetActive(value: false);
			Digger.gameObject.SetActive(value: false);
			bool skip = false;
			SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
			{
				Gui.Views.CutsceneFrame.Show();
				Gui.Views.CutsceneFrame.OnSkip = delegate
				{
					skip = true;
				};
			});
			introSequence.Play();
			while (introSequence.IsPlaying && !skip)
			{
				yield return null;
			}
			if (skip)
			{
				introSequence.Stop();
				GameCamera.Focus();
				introSequence.GetComponentInChildren<FPlayAudioSourceEvent>().Owner.GetComponent<AudioSource>().Stop();
			}
			Gui.Views.CutsceneFrame.HideAnimated();
			Digger.gameObject.SetActive(value: true);
			Dragon.gameObject.SetActive(value: true);
			Digger.Walker.InstantGround();
			GameCamera.SmoothTime *= 4f;
			GameCamera.MaxSpeed /= 4f;
			float startTime = Time.time;
			while ((!GameCamera.Stable || Time.time - startTime < 0.5f) && Time.time - startTime < 5f)
			{
				yield return null;
			}
			GameCamera.SmoothTime /= 4f;
			GameCamera.MaxSpeed *= 4f;
		}
	}

	public void OnLevelComplete()
	{
		if (SingletonMonoBehaviour<Game>.Instance.Chicken != null && !SingletonMonoBehaviour<Game>.Instance.Chicken.Broken)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.ChickenPassLevel);
		}
		if (SingletonMonoBehaviour<Game>.Instance.LevelKillCount == 0)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.Pacifist);
		}
		if (SingletonMonoBehaviour<Game>.Instance.CoinsGrabbed == 0)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.PassLevelNoCoins);
		}
		if (SingletonMonoBehaviour<Game>.Instance.EnemiesLeftAliveInLevel == 0)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillAllEnemiesInLevel);
		}
		SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.PassLevel);
		SingletonMonoBehaviour<Game>.Instance.LevelKillCount = 0;
		SingletonMonoBehaviour<Game>.Instance.EnemiesLeftAliveInLevel = 0;
		SingletonMonoBehaviour<Game>.Instance.CoinsGrabbed = 0;
		SingletonMonoBehaviour<Game>.Instance.Digger.SaveProgress();
		if (Gui.Views.RateUs.ShouldShow)
		{
			StartCoroutine(Gui.Views.RateUs.ShowCoroutine());
		}
		GameAnalytics.NewDesignEvent("gameplay:level" + SaveGame.Instance.WorldLevel + "duration", SingletonMonoBehaviour<Game>.Instance.Digger.SecondsSinceRunStart);
		GameAnalytics.NewDesignEvent("gameplay:level" + SaveGame.Instance.WorldLevel + "coins", SingletonMonoBehaviour<Game>.Instance.Digger.Coins);
	}

	private IEnumerator LoadDragon()
	{
		string sceneName = "Dragon";
		if (CurrentWorld == "Gothic")
		{
			sceneName += "Halloween";
		}
		if (SceneManager.GetSceneByName(sceneName).IsValid())
		{
			yield return SceneManager.UnloadSceneAsync(sceneName);
		}
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		Dragon = SceneManager.GetSceneByName(sceneName).GetRootGameObjects()[0].GetComponentInChildren<Dragon>();
	}

	private IEnumerator LoadCharacter()
	{
		Vector3 position = Vector3.zero;
		Coord coord = Coord.None;
		if (Digger != null)
		{
			position = Digger.transform.position;
			coord = Digger.Coord;
			Digger.RemoveCoord();
			yield return SceneManager.UnloadSceneAsync(Digger.gameObject.scene);
		}
		string name = SaveGame.Instance.CurrentCharacter;
		if (Characters.ByName(name) == null)
		{
			name = Characters.Instance.DefaultCharacter;
		}
		Scene scene = SceneManager.GetSceneByName("Character" + name);
		if (!scene.IsValid())
		{
			yield return SceneManager.LoadSceneAsync("Character" + name, LoadSceneMode.Additive);
			scene = SceneManager.GetSceneByName("Character" + name);
			if (!scene.IsValid())
			{
				UnityEngine.Debug.LogError("Failed loading character " + name);
				SaveGame.Instance.CurrentCharacter = Characters.Instance.DefaultCharacter;
				yield return SceneManager.LoadSceneAsync("Character" + Characters.Instance.DefaultCharacter, LoadSceneMode.Additive);
				yield return null;
				scene = SceneManager.GetSceneByName("Character" + Characters.Instance.DefaultCharacter);
				if (!scene.IsValid())
				{
					UnityEngine.Debug.LogError("Failed loading default character!");
				}
			}
		}
		if (scene.GetRootGameObjects().Length == 0)
		{
			UnityEngine.Debug.LogError("Empty scene while loading character " + name);
			yield return SceneManager.LoadSceneAsync("Character" + Characters.Instance.DefaultCharacter, LoadSceneMode.Additive);
			yield return null;
			scene = SceneManager.GetSceneByName("Character" + Characters.Instance.DefaultCharacter);
		}
		Digger = scene.GetRootGameObjects()[0].GetComponent<Digger>();
		if (Digger == null)
		{
			UnityEngine.Debug.LogError("Unable to load character " + name);
		}
		if (Ready)
		{
			Digger.Initialize(position, coord);
		}
	}

	public void FixCurrentCharacter()
	{
		if (!SaveGame.Instance.CharacterOwned(SaveGame.Instance.CurrentCharacter))
		{
			if (SaveGame.Instance.CharacterOwned(SaveGame.Instance.PreviousCharacter))
			{
				SaveGame.Instance.CurrentCharacter = SaveGame.Instance.PreviousCharacter;
			}
			else
			{
				SaveGame.Instance.CurrentCharacter = Characters.Instance.DefaultCharacter;
			}
		}
	}

	private bool SaveCharacter(string name)
	{
		if (SaveGame.Instance.CurrentCharacter == name)
		{
			return false;
		}
		SaveGame.Instance.PreviousCharacter = SaveGame.Instance.CurrentCharacter;
		SaveGame.Instance.CurrentCharacter = name;
		return true;
	}

	public void LoadCharacter(string name)
	{
		if (SaveCharacter(name))
		{
			Restart();
		}
	}

	public void LoadGiftCharacter(string name)
	{
		if (SaveCharacter(name))
		{
			StartCoroutine(LoadCharacter());
		}
	}

	public ItemDef GetItem(string itemName)
	{
		if (string.IsNullOrEmpty(itemName))
		{
			return null;
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].name == itemName)
			{
				return Items[i];
			}
		}
		UnityEngine.Debug.LogError("Item " + itemName + " is not defined");
		return null;
	}

	public void Restart()
	{
		StartCoroutine(RestartCoroutine());
	}

	private IEnumerator RestartCoroutine()
	{
		AudioListener.pause = false;
		GameTime.Resume();
		Started = false;
		SaveGame.Instance.WorldLevel = 1;
		if (Chicken != null)
		{
			UnityEngine.Object.Destroy(Chicken.gameObject);
			Chicken = null;
		}
		SingletonMonoBehaviour<MissionManager>.Instance.CheckLevelUpdate();
		Ready = false;
		yield return new WaitForSeconds(0.5f);
		GameCamera.enabled = false;
		Transform transform = GameCamera.transform;
		Vector3 position = GameCamera.transform.position;
		transform.position = new Vector3(0f, 0f, position.z);
		SingletonMonoBehaviour<World>.Instance.Restart();
		StartCoroutine(LoadGame());
	}

	public IEnumerator FadeTime(float targetTimeScale, float targetPitch, float length)
	{
		float startTime = Time.realtimeSinceStartup;
		float startPitch = 1f;
		AudioMixer.GetFloat("worldPitch", out startPitch);
		AudioMixer.SetFloat("duckSlowMoVolume", (targetPitch != 1f) ? (-20) : 0);
		float startTimeScale = GameTime.GetTimeScale(SlowMoPriority.Game);
		while (Time.realtimeSinceStartup - startTime < length)
		{
			float a2 = (Time.realtimeSinceStartup - startTime) / length;
			a2 = Tween.CubicEaseInOut(a2, 0f, 1f, 1f);
			GameTime.Slow(Mathf.Lerp(startTimeScale, targetTimeScale, a2), SlowMoPriority.Game);
			AudioMixer.SetFloat("worldPitch", Mathf.Lerp(startPitch, targetPitch, a2));
			yield return null;
		}
		GameTime.Slow(targetTimeScale, SlowMoPriority.Game);
		AudioMixer.SetFloat("worldPitch", targetPitch);
	}

	public IEnumerator FadeMixer(string name, float targetVolume, float time)
	{
		float startTime = Time.realtimeSinceStartup;
		float startVolume = 0f;
		AudioMixer.GetFloat(name, out startVolume);
		while (Time.realtimeSinceStartup - startTime < time)
		{
			float a2 = (Time.realtimeSinceStartup - startTime) / time;
			a2 = Tween.CubicEaseInOut(a2, 0f, 1f, 1f);
			AudioMixer.SetFloat(name, Mathf.Lerp(startVolume, targetVolume, a2));
			yield return null;
		}
		AudioMixer.SetFloat(name, targetVolume);
	}

	public IEnumerator FadeAudio(AudioSource audio, float targetVolume, float time)
	{
		float startTime = Time.realtimeSinceStartup;
		float startVolume = audio.volume;
		while (Time.realtimeSinceStartup - startTime < time)
		{
			yield return null;
			float a2 = (Time.realtimeSinceStartup - startTime) / time;
			a2 = Tween.CubicEaseInOut(a2, 0f, 1f, 1f);
			audio.volume = Mathf.Lerp(startVolume, targetVolume, a2);
		}
		audio.volume = targetVolume;
	}

	public void ResetFadeTime()
	{
		GameTime.Slow(1f, SlowMoPriority.Game);
		AudioMixer.SetFloat("worldPitch", 1f);
		SingletonMonoBehaviour<Game>.Instance.AudioMixer.SetFloat("duckSlowMoVolume", 0f);
	}

	public void MakeNoise(Noise noise)
	{
		if (OnNoise != null)
		{
			OnNoise(noise);
		}
		StartRun();
	}

	public void StartRun()
	{
		if (Started)
		{
			return;
		}
		Started = true;
		if (SaveGame.Instance.WorldLevel == 1)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				SaveGame.Instance.SetItemActive(Items[i].name, value: false);
			}
			SaveGame.Instance.SaveMeCount = 0;
			SaveGame.Instance.DiggerBombs = 0;
			SaveGame.Instance.DiggerCoins = 0;
			SaveGame.Instance.Mission1Progress = 0;
			SaveGame.Instance.Mission2Progress = 0;
			SaveGame.Instance.Mission3Progress = 0;
		}
		else
		{
			for (int j = 0; j < Items.Count; j++)
			{
				if (SaveGame.Instance.ItemActive(Items[j].name))
				{
					Items[j].Activate(Digger);
				}
			}
			Digger.Coins = SaveGame.Instance.DiggerCoins;
			Digger.Bombs = SaveGame.Instance.DiggerBombs;
		}
		MissionSet currentMissionSet = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		if (currentMissionSet != null)
		{
			currentMissionSet.Missions[0].Progress = SaveGame.Instance.Mission1Progress;
			currentMissionSet.Missions[1].Progress = SaveGame.Instance.Mission2Progress;
			currentMissionSet.Missions[2].Progress = SaveGame.Instance.Mission3Progress;
		}
		SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
		{
			ShowInGameButtons();
			Gui.Views.InGameStatsView.ShowAnimated();
			Gui.Views.StartView.HideAnimated();
			Gui.Views.MissionsView.HideAnimated();
		});
	}

	private void OnApplicationPause(bool paused)
	{
		SaveGame.Instance.Save();
		if (paused && Started && !GameTime.Paused && Gui.Views.PauseButtonView.Visible && !Gui.Views.PauseButtonView.Hiding && !FullscreenViewOrTransition())
		{
			Gui.Views.PauseButtonView.OnPause();
		}
	}

	public void ActivateChicken()
	{
		if (!(Chicken != null) || Chicken.Broken)
		{
			Chicken = UnityEngine.Object.Instantiate(ChickenPrefab, base.transform).GetComponent<Chicken>();
			Chicken.Initialize();
		}
	}

	public void ShowAd(Action onSuccess, Action onFail)
	{
		SaveGame.Instance.LastAdDateTime = DateTime.Now;
		StartCoroutine(ShowAdCoroutine(onSuccess, onFail));
	}

	public IEnumerator ShowAdCoroutine(Action onSuccess, Action onFail)
	{
		IEnumerator video = SingletonMonoBehaviour<SupersonicManager>.Instance.ShowVideo();
		while (video.MoveNext())
		{
			yield return null;
		}
		if (SingletonMonoBehaviour<SupersonicManager>.Instance.GotReward)
		{
			onSuccess?.Invoke();
		}
		else
		{
			onFail?.Invoke();
		}
	}

	public void DiggerKilledEnemy(Enemy enemy)
	{
		LevelKillCount++;
		if (Time.time - lastKilledTime < 0.8f)
		{
			Combo++;
			Gui.Views.ComboView.NotifyCombo(Combo, SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(enemy.Coord));
			if (Combo == 2)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.MakeTwoCombo);
			}
			else if (Combo == 3)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.MakeThreeCombo);
			}
		}
		else
		{
			Combo = 1;
		}
		lastKilledTime = Time.time;
	}

	public void ShowInGameButtons()
	{
		Gui.Views.ItemsView.ShowAnimated();
		if (SaveGame.Instance.TutorialComplete)
		{
			Gui.Views.PauseButtonView.ShowAnimated();
		}
	}

	public void HideInGameButtons()
	{
		Gui.Views.ItemsView.HideAnimated();
		Gui.Views.PauseButtonView.HideAnimated();
	}

	public void UpdateMusicAndSFX()
	{
		AudioMixer.SetFloat("musicVolume", (!SaveGame.Instance.Music || GameTime.Paused) ? (-80) : 0);
		AudioMixer.SetFloat("uisfxVolume", (!SaveGame.Instance.SFX) ? (-80) : 0);
		AudioMixer.SetFloat("worldVolume", (!SaveGame.Instance.SFX || GameTime.Paused) ? (-80) : 0);
	}
}
