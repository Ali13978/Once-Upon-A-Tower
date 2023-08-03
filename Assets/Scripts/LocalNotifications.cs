using I2.Loc;
using Prime31;
using System;

public class LocalNotifications : SingletonMonoBehaviour<LocalNotifications>
{
	public struct NotificationConfig
	{
		public DateTime Time;

		public string Message;
	}

	private void Start()
	{
	}

	public void ReScheduleAll()
	{
		CancelAllNotifications();
		int cancelId = -1;
		for (int i = 0; i < GameVars.Reminders.Length + 1; i++)
		{
			TimeSpan t = (i != 0) ? GameVars.Reminders[i - 1] : TimeSpan.FromDays(0.0);
			DateTime dateTime = SaveGame.Instance.NextGiftTime + t;
			if (DateTime.Now < dateTime)
			{
				Gift gift = SingletonMonoBehaviour<GiftManager>.Instance.Choose(dateTime);
				string message = ScriptLocalization.Get(gift.NotificationKey).Replace("{name}", gift.DisplayName);
				NotificationConfig notificationConfig = default(NotificationConfig);
				notificationConfig.Time = dateTime;
				notificationConfig.Message = message;
				NotificationConfig config = notificationConfig;
				cancelId = ScheduleNotification(config, config.Time, cancelId);
			}
		}
		SaveGame.Instance.Save();
	}

	private bool NotificationSound(DateTime dateTime)
	{
		return dateTime.Hour >= GameVars.NotificationSoundStart && dateTime.Hour < GameVars.NotificationSoundEnd;
	}

	public void Schedule(NotificationConfig config, bool reminders)
	{
		int cancelId = ScheduleNotification(config, config.Time);
		if (reminders)
		{
			TimeSpan[] reminders2 = GameVars.Reminders;
			foreach (TimeSpan t in reminders2)
			{
				cancelId = ScheduleNotification(config, DateTime.Now + t, cancelId);
			}
		}
	}

	private int ScheduleNotification(NotificationConfig config, DateTime time, int cancelId = -1)
	{
		AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration((long)(time - DateTime.Now).TotalSeconds, "Tower", config.Message, config.Message);
		androidNotificationConfiguration.color = -12303292;
		androidNotificationConfiguration.largeIcon = "ic_large_default";
		androidNotificationConfiguration.smallIcon = "ic_stat_default";
		androidNotificationConfiguration.sound = (androidNotificationConfiguration.vibrate = NotificationSound(config.Time));
		androidNotificationConfiguration.cancelsNotificationId = cancelId;
		int num = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration);
		SaveGame instance = SaveGame.Instance;
		instance.AndroidNotifications = instance.AndroidNotifications + num + ",";
		return num;
	}

	public void CancelAllNotifications()
	{
		string[] array = SaveGame.Instance.AndroidNotifications.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			CancelNotification(array[i]);
		}
		SaveGame.Instance.AndroidNotifications = string.Empty;
		EtceteraAndroid.cancelAllNotifications();
	}

	public void CancelNotification(string id)
	{
		int result = 0;
		if (int.TryParse(id, out result))
		{
			EtceteraAndroid.cancelNotification(result);
		}
	}
}
