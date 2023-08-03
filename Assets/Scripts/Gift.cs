using I2.Loc;

public class Gift
{
	public GiftType Type;

	public string Name;

	public string DisplayName
	{
		get
		{
			if (Type == GiftType.Character)
			{
				return Characters.ByName(Name).DisplayName;
			}
			return ScriptLocalization.Get("Item" + Name + "Name");
		}
	}

	public string NotificationKey
	{
		get
		{
			if (Type == GiftType.Character)
			{
				return "NotificationCharacter";
			}
			string text = "Item" + Name + "Notification";
			if (!string.IsNullOrEmpty(ScriptLocalization.Get(text)))
			{
				return text;
			}
			return "NotificationItem";
		}
	}

	public void Redeem()
	{
		if (Type == GiftType.Character)
		{
			SingletonMonoBehaviour<Game>.Instance.LoadGiftCharacter(Name);
			Gui.Views.CharacterDealMessage.ShowAnimated();
		}
		else
		{
			SingletonMonoBehaviour<Game>.Instance.GetItem(Name).Activate(SingletonMonoBehaviour<Game>.Instance.Digger);
			SingletonMonoBehaviour<Game>.Instance.StartRun();
			Gui.Views.ItemGet.ShowAnimated(Name);
		}
		SaveGame.Instance.PlaceGift = false;
		SaveGame.Instance.Save();
		SingletonMonoBehaviour<LocalNotifications>.Instance.ReScheduleAll();
		SingletonMonoBehaviour<GiftManager>.Instance.ForceCharacter = false;
	}
}
