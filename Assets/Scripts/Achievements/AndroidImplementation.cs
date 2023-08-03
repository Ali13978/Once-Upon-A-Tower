using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
	public class AndroidImplementation : ImplementationBase
	{
		public override void Init(Dictionary<AchievementType, bool> achieved)
		{
			if (Social.localUser.authenticated)
			{
				foreach (KeyValuePair<AchievementType, bool> item in achieved)
				{
					if (item.Value && AndroidAchievements.achievementIds.ContainsKey(item.Key))
					{
						Social.ReportProgress(AndroidAchievements.achievementIds[item.Key], 100.0, delegate
						{
						});
					}
				}
			}
		}

		public override IEnumerator ShowAchievement(AchievementType achievement)
		{
			if (Social.localUser.authenticated)
			{
				UnityEngine.Debug.Log("Android Report Achievement " + achievement.ToString());
				if (AndroidAchievements.achievementIds.ContainsKey(achievement))
				{
					Social.ReportProgress(AndroidAchievements.achievementIds[achievement], 100.0, delegate
					{
					});
				}
			}
			yield break;
		}

		internal override void ShowAchievementUI(Action callback)
		{
			Social.ShowAchievementsUI();
		}
	}
}
