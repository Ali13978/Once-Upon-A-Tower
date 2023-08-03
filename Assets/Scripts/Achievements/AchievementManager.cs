using System;
using System.Collections.Generic;

namespace Achievements
{
	public class AchievementManager : SingletonMonoBehaviour<AchievementManager>
	{
		private ImplementationBase implementation = new AndroidImplementation();

		private Dictionary<AchievementType, bool> achieved = new Dictionary<AchievementType, bool>();

		private Dictionary<AchievementType, int> scoreAchievemnts;

		private Dictionary<AchievementType, int> missionLevelAchievemnts;

		public virtual int AchievedCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 20; i++)
				{
					if (achieved[(AchievementType)i])
					{
						num++;
					}
				}
				return num;
			}
		}

		public virtual int Total => 20;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (achieved.Count == 0)
			{
				ReportAllAchieved();
			}
			if (scoreAchievemnts == null)
			{
				scoreAchievemnts = new Dictionary<AchievementType, int>
				{
					{
						AchievementType.BestScore120000,
						120000
					},
					{
						AchievementType.BestScore90000,
						90000
					},
					{
						AchievementType.BestScore60000,
						60000
					},
					{
						AchievementType.BestScore30000,
						30000
					},
					{
						AchievementType.BestScore24000,
						24000
					},
					{
						AchievementType.BestScore18000,
						18000
					},
					{
						AchievementType.BestScore12000,
						12000
					},
					{
						AchievementType.BestScore6000,
						6000
					},
					{
						AchievementType.BestScore1000,
						1000
					}
				};
				missionLevelAchievemnts = new Dictionary<AchievementType, int>
				{
					{
						AchievementType.Queen,
						20
					},
					{
						AchievementType.Princess,
						15
					},
					{
						AchievementType.Duchess,
						10
					},
					{
						AchievementType.Marchioness,
						7
					},
					{
						AchievementType.Countess,
						5
					},
					{
						AchievementType.Viscountess,
						4
					},
					{
						AchievementType.Baroness,
						3
					},
					{
						AchievementType.Dame,
						2
					}
				};
			}
		}

		public void ReportAllAchieved()
		{
			for (int i = 0; i < 20; i++)
			{
				AchievementType key = (AchievementType)i;
				achieved[key] = SaveGame.Instance.HasAchieved(key.ToString());
			}
			implementation.Init(achieved);
		}

		public void ReportSucceded(string identifier)
		{
			implementation.ReportSucceded(identifier);
		}

		internal void ShowAchievementUI(Action callback)
		{
			implementation.ShowAchievementUI(callback);
		}

		private void CheckBooleanAchievement(AchievementType achievement)
		{
			if (!achieved[achievement])
			{
				achieved[achievement] = true;
				SaveGame.Instance.SetAchievement(achievement.ToString());
				StartCoroutine(implementation.ShowAchievement(achievement));
			}
		}

		public void NotifyScore(int score)
		{
			Init();
			foreach (KeyValuePair<AchievementType, int> scoreAchievemnt in scoreAchievemnts)
			{
				if (scoreAchievemnt.Value < score)
				{
					CheckBooleanAchievement(scoreAchievemnt.Key);
				}
			}
		}

		public void NotifyMissionLevel(int missionLevel)
		{
			Init();
			foreach (KeyValuePair<AchievementType, int> missionLevelAchievemnt in missionLevelAchievemnts)
			{
				if (missionLevelAchievemnt.Value <= missionLevel)
				{
					CheckBooleanAchievement(missionLevelAchievemnt.Key);
				}
			}
		}

		public void NotifyEscapeTower()
		{
			Init();
			if (Characters.RescuedCount == 1)
			{
				CheckBooleanAchievement(AchievementType.HappilyEverAfter);
			}
			else if (Characters.RescuedCount == Characters.Total)
			{
				CheckBooleanAchievement(AchievementType.NoOneLeftBehind);
			}
		}

		public void NotifyEscapeTowerWithChicken()
		{
			Init();
			CheckBooleanAchievement(AchievementType.ChickenRun);
		}
	}
}
