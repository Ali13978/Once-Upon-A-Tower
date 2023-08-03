using System;
using System.Collections;
using System.Collections.Generic;

namespace Achievements
{
	public class ImplementationBase
	{
		public virtual void Init(Dictionary<AchievementType, bool> achieved)
		{
		}

		public virtual IEnumerator ShowAchievement(AchievementType achievement)
		{
			yield break;
		}

		public virtual void OnGUI()
		{
		}

		public virtual void ReportSucceded(string id)
		{
		}

		internal virtual void ShowAchievementUI(Action callback)
		{
			callback();
		}
	}
}
