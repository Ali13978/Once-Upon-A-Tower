using System;
using UnityEngine;

public class GiftManager : SingletonMonoBehaviour<GiftManager>
{
	private bool forceCharacter;

	public bool ForceCharacter
	{
		get
		{
			return Application.isEditor && forceCharacter;
		}
		set
		{
			forceCharacter = value;
		}
	}

	public bool NeverScheduled => SaveGame.Instance.NextGiftTime == DateTime.MaxValue;

	public Gift Choose(DateTime dateTime)
	{
		UnityEngine.Random.State state = UnityEngine.Random.state;
		Gift[] array = null;
		Gift gift = null;
		UnityEngine.Random.InitState(SaveGame.Instance.UserId.GetHashCode() + dateTime.DayOfYear);
		float num = UnityEngine.Random.Range(0f, 1f);
		Gift[] array2 = CharacterArray();
		Gift[] array3 = ChickenArray();
		Gift[] array4 = ItemArray();
		if (!ForceCharacter)
		{
			array = ((Purchaser.Instance.UseIAPs && array2.Length > 0 && Characters.OwnedCount > 2) ? ((num <= GameVars.CharacterGiftChance) ? array2 : ((!(num <= GameVars.CharacterGiftChance + GameVars.ChickenGiftChance)) ? array4 : array3)) : ((!(num <= GameVars.CharacterGiftChance / 2f + GameVars.ChickenGiftChance)) ? array4 : array3));
		}
		else
		{
			array = array2;
			if (array2.Length == 0)
			{
				gift = new Gift();
				gift.Type = GiftType.Character;
				gift.Name = Characters.Instance.DefaultCharacter;
				UnityEngine.Debug.LogWarning("Forced to gift a character but none are available, using " + gift.Name + " as default");
			}
		}
		if (gift == null)
		{
			if (array.Length == 0)
			{
				gift = new Gift();
				gift.Type = GiftType.Item;
				gift.Name = "Chicken";
				UnityEngine.Debug.LogError("No gift available to give, this should not happen, using " + gift.Name + " as default");
			}
			else
			{
				int num2 = UnityEngine.Random.Range(0, array.Length);
				gift = array[num2];
			}
		}
		UnityEngine.Random.state = state;
		return gift;
	}

	private Gift[] CharacterArray()
	{
		return Array.ConvertAll(Characters.PendingList, (Character c) => new Gift
		{
			Type = GiftType.Character,
			Name = c.Name
		});
	}

	private Gift[] ChickenArray()
	{
		return SingletonMonoBehaviour<Game>.Instance.Items.FindAll((ItemDef i) => i.name == "Chicken").ConvertAll((ItemDef i) => new Gift
		{
			Type = GiftType.Item,
			Name = i.name
		}).ToArray();
	}

	private Gift[] ItemArray()
	{
		return SingletonMonoBehaviour<Game>.Instance.Items.FindAll((ItemDef i) => i.name != "Chicken" && i.name != "BombX1" && i.name != "Fortune").ConvertAll((ItemDef i) => new Gift
		{
			Type = GiftType.Item,
			Name = i.name
		}).ToArray();
	}

	private void Place()
	{
		if (!SingletonMonoBehaviour<Game>.Instance.Started && SingletonMonoBehaviour<World>.Instance.Ready && SaveGame.Instance.WorldLevel == 1)
		{
			Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(new Coord(0, 5, 3));
			WorldObject worldObject = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			if (worldObject != null && worldObject is GiftCube)
			{
				Gift gift = ((GiftCube)worldObject).Gift;
				Gift gift2 = Choose(DateTime.Now);
				if (gift == null || gift2 == null || gift.Name != gift2.Name)
				{
					UnityEngine.Object.Destroy(worldObject.gameObject);
					worldObject = null;
				}
			}
			if (worldObject == null)
			{
				TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(0);
				if (section != null)
				{
					worldObject = section.InstantiateFromPrefab("GiftCube", coord);
					worldObject.Initialize();
				}
			}
		}
		SaveGame.Instance.PlaceGift = true;
		SaveGame.Instance.Save();
		SingletonMonoBehaviour<LocalNotifications>.Instance.ReScheduleAll();
	}

	public void Schedule(TimeSpan timeSpan)
	{
		SaveGame.Instance.NextGiftTime = DateTime.Now + timeSpan;
		SaveGame.Instance.Save();
		SingletonMonoBehaviour<LocalNotifications>.Instance.ReScheduleAll();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			SingletonMonoBehaviour<GiftManager>.Instance.Refresh();
		}
	}

	public void Refresh()
	{
		if (SaveGame.Instance.NextGiftTime < DateTime.Now)
		{
			Schedule(GameVars.TimeBetweenGifts);
			Place();
		}
		else if (SaveGame.Instance.PlaceGift)
		{
			Place();
		}
	}
}
