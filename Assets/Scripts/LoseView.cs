using Achievements;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoseView : GuiView
{
	public GuiButton RestartButton;

	public GuiButton CompleteButton;

	public GuiButton LeaderboardButton;

	public GuiButton SkipButton;

	public Animation PrizeFillAnimation;

	public TextMeshPro PrizePercentage;

	public TextMeshPro ScoreText;

	public TextMeshPro BestScoreText;

	public GameObject Buttons;

	public GameObject BestContainer;

	public GameObject NewBestContainer;

	public GameObject MultiplierContainer;

	public TextMeshPro SaveMesText;

	public TextMeshPro MultiplierText;

	public ParticleSystem BottleFireflies;

	public ParticleSystem FlowFireflies;

	public AnimationCurve FireflyEmission;

	public AnimationCurve FireflySize;

	public float MinFlowRate = 1f;

	public float MaxFlowRate = 20f;

	public float CoinFlowTime = 2f;

	public float CoinFlowWait = 0.75f;

	public AudioSource JarLoopAudio;

	private GuiButton defaultButton;

	private int score;

	private int CurrentPrizeCoins
	{
		get
		{
			int num = SaveGame.Instance.PrizeCount;
			if (num >= GameVars.PrizeCoins.Length)
			{
				num = GameVars.PrizeCoins.Length - 1;
			}
			return GameVars.PrizeCoins[num];
		}
	}

	protected override void Start()
	{
		base.Start();
		RestartButton.Click = delegate
		{
			HideAnimated();
			SingletonMonoBehaviour<Game>.Instance.Restart();
		};
		CompleteButton.Click = delegate
		{
			base.TouchEnabled = false;
			StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeAudio(JarLoopAudio, 0f, 0.4f));
			Play("PrizeComplete", delegate
			{
				Hide();
			});
			string text = Characters.ChooseToUnlock();
			SaveGame.Instance.SetCharacterOwned(text, value: true);
			Gui.Views.CharacterGet.ShowAnimated(text);
			SaveGame.Instance.PrizeCoins = 0;
			SaveGame.Instance.PrizeCount++;
			SaveGame.Instance.Save();
		};
		LeaderboardButton.Click = delegate
		{
			GameServices.ShowLeaderboard();
		};
	}

	public override void ShowAnimated()
	{
		ApplyBoundingBox();
		CompleteButton.gameObject.SetActive(value: false);
		UpdatePrizeCounter();
		UpdateBestScoreText();
		SetScoreText(0);
		StartCoroutine(LoseCoroutine());
		JarLoopAudio.Stop();
		JarLoopAudio.volume = 1f;
	}

	private IEnumerator LoseCoroutine()
	{
		Buttons.SetActive(value: false);
		BestContainer.SetActive(value: false);
		NewBestContainer.SetActive(value: false);
		SaveMesText.gameObject.SetActive(value: false);
		MultiplierContainer.SetActive(value: false);
		if (base.Hiding)
		{
			StopCurrentSequence();
		}
		Show();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(defaultButton = SkipButton);
		yield return PlayInCoroutine("Intro");
		ScoreText.text = "0";
		bool skip = false;
		SkipButton.Click = delegate
		{
			skip = true;
		};
		score = SingletonMonoBehaviour<Game>.Instance.Digger.Coins;
		if (score > 0)
		{
			if (!Gui.Views.InGameStatsView.Visible)
			{
				Gui.Views.InGameStatsView.ShowAnimated();
			}
            
            ParticleSystem.EmissionModule FlowFirefliesEmissionModule = FlowFireflies.emission;
            FlowFirefliesEmissionModule.rateOverTime = Mathf.Lerp(MinFlowRate, MaxFlowRate, Mathf.Clamp01((float)score * 1f / 1500f)); ;

            StopSingle("Fill");
			PlaySingle("Fill");
			yield return new WaitForSecondsRealtime(CoinFlowWait);
			int startPrizeCoins = SaveGame.Instance.PrizeCoins;
			float startTime = Time.realtimeSinceStartup;
			float delta = CoinFlowTime;
			while (Time.realtimeSinceStartup - startTime < delta && !skip)
			{
				float a = (Time.realtimeSinceStartup - startTime) / delta;
				SingletonMonoBehaviour<Game>.Instance.Digger.Coins = Mathf.RoundToInt((float)score * Mathf.Clamp01(1f - a));
				SaveGame.Instance.PrizeCoins = Mathf.RoundToInt((float)startPrizeCoins + (float)score * a);
				UpdatePrizeCounter();
				SetScoreText(Mathf.RoundToInt((float)score * a));
				yield return null;
			}
			SingletonMonoBehaviour<Game>.Instance.Digger.Coins = 0;
			SaveGame.Instance.PrizeCoins = startPrizeCoins + score;
			UpdatePrizeCounter();
			SetScoreText(score);
			if (Gui.Views.InGameStatsView.Visible)
			{
				Gui.Views.InGameStatsView.HideAnimated();
			}
			while (IsPlayingSingle("Fill") && !skip)
			{
				yield return null;
			}
			if (skip)
			{
				GoToLastFrameSingle("Fill");
			}
		}
		if (SaveGame.Instance.MissionLevel > 1)
		{
			MultiplierText.text = "x" + SaveGame.Instance.MissionLevel;
			MultiplierContainer.SetActive(value: true);
			score *= SaveGame.Instance.MissionLevel;
			if (skip)
			{
				UpdateScoreText();
			}
			else
			{
				yield return PlayInCoroutine("MultiplyScore");
			}
			MultiplierContainer.SetActive(value: false);
		}
		bool newBest = score > SaveGame.Instance.BestScore;
		if (newBest)
		{
			SaveGame.Instance.BestScore = score;
			UpdateBestScoreText();
			GameServices.PostScore(SaveGame.Instance.BestScore);
			SingletonMonoBehaviour<AchievementManager>.Instance.NotifyScore(SaveGame.Instance.BestScore);
		}
		if (SaveGame.Instance.PrizeCoins >= CurrentPrizeCoins)
		{
			if (Characters.ChooseToUnlock() != null)
			{
				Play("CompleteLoop");
				SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(defaultButton = CompleteButton);
				CompleteButton.gameObject.SetActive(value: true);
				yield break;
			}
			SaveMesText.gameObject.SetActive(value: true);
			SaveMesText.text = SaveMesText.text.Replace("{S}", GameVars.SaveMesPerFlask.ToString());
			SaveGame.Instance.SaveMeCoins += GameVars.SaveMesPerFlask;
			SaveGame.Instance.PrizeCoins = 0;
			yield return PlayInCoroutine("ShowSaveMes");
			ShowScoreAndButtons(newBest);
		}
		else
		{
			ShowScoreAndButtons(newBest);
		}
	}

	public void UpdateScoreText()
	{
		SetScoreText(score);
	}

	private void ShowScoreAndButtons(bool newBest)
	{
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(defaultButton = RestartButton);
		Buttons.SetActive(value: true);
		BestContainer.SetActive(!newBest);
		NewBestContainer.SetActive(newBest);
		Play((!newBest) ? "ShowScore" : "ShowBestScore");
	}

	private void SetScoreText(int coins)
	{
		ScoreText.text = coins.ToString();
	}

	private void UpdateBestScoreText()
	{
		BestScoreText.text = SaveGame.Instance.BestScore.ToString();
	}

	private void UpdatePrizeCounter()
	{
		float num = (float)SaveGame.Instance.PrizeCoins * 1f / (float)CurrentPrizeCoins;
		if (PrizeFillAnimation != null)
		{
			PrizeFillAnimation.clip.SampleAnimation(PrizeFillAnimation.gameObject, (1f - Mathf.Clamp01(num)) * PrizeFillAnimation.clip.length);
		}
		if (PrizePercentage != null)
		{
			int num2 = Mathf.Min(100, Mathf.RoundToInt(num * 100f));
			if (num2 == 100 && SaveGame.Instance.PrizeCoins < CurrentPrizeCoins)
			{
				num2 = 99;
			}
			PrizePercentage.text = num2 + "%";
		}

        ParticleSystem.EmissionModule bottleFirefliesEmission = BottleFireflies.emission;
		bottleFirefliesEmission.rateOverTime = FireflyEmission.Evaluate(num);

        ParticleSystem.MainModule bootleFirefliesMainModule = BottleFireflies.main;
        bootleFirefliesMainModule.startSize = FireflySize.Evaluate(num);
	}

	public override bool OnMenuButton()
	{
		if (base.TouchEnabled && defaultButton != null && defaultButton.Click != null)
		{
			defaultButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
