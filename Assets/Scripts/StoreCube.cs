using Flux;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreCube : WorldObject
{
	public Transform IconParent;

	public Vector3 IconScale = Vector3.one;

	public TextMeshPro Cost;

	public GameObject CostContainer;

	public GameObject AdContainer;

	private int cost;

	public FSequence NotEnoughUpSequence;

	public FSequence NotEnoughDownSequence;

	private static List<ItemDef> chosenItems = new List<ItemDef>();

	private static System.Random rng = new System.Random();

	public ItemDef Item
	{
		get;
		private set;
	}

	private void FixedUpdate()
	{
		Coord coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
		if (coord == Coord + Coord.Up || coord == Coord + Coord.Down)
		{
			if (!Gui.Views.InGameStoreItemView.Visible)
			{
				Gui.Views.InGameStoreItemView.ShowAnimated(this);
			}
		}
		else
		{
			Gui.Views.InGameStoreItemView.HideAnimated(this);
		}
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (Item != null && hitter is Digger)
		{
			if (hitter.Coins >= cost)
			{
				Gui.Views.InGameStoreItemView.HideAnimated(this);
				Breakable.Break(direction, hitter);
				SingletonMonoBehaviour<Game>.Instance.StartCoroutine(ActivateItem((Digger)hitter));
				hitter.Coins -= cost;
				Gui.Views.ItemGet.ShowAnimated(Item.name);
			}
			else if (direction == Vector3.up && NotEnoughUpSequence != null)
			{
				NotEnoughUpSequence.Stop();
				NotEnoughUpSequence.Play();
			}
			else if (direction == Vector3.down && NotEnoughDownSequence != null)
			{
				NotEnoughDownSequence.Stop();
				NotEnoughDownSequence.Play();
			}
		}
	}

	private IEnumerator ActivateItem(Digger digger)
	{
		yield return new WaitForSeconds(0.2f);
		Item.Activate(digger);
	}

	public override bool BreakableBy(WorldObject hitter)
	{
		return Item != null && hitter is Digger && hitter.Coins >= cost;
	}

	private void Choose(ItemDef item)
	{
		Item = item;
		cost = item.ActualCost;
		Cost.text = cost.ToString();
		while (IconParent.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(IconParent.GetChild(0).gameObject);
		}
		if (item.Icon != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(item.Icon);
			gameObject.transform.parent = IconParent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = IconScale;
		}
		CostContainer.SetActive(item.Cost > 0);
		AdContainer.SetActive(item.Cost == 0);
	}

	public static void ChooseItems(List<StoreCube> cubes)
	{
		if (cubes.Count == 0)
		{
			return;
		}
		chosenItems.Clear();
		Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
		List<ItemDef> items = SingletonMonoBehaviour<Game>.Instance.Items;
		if (items.Count == 0)
		{
			UnityEngine.Debug.LogError("ChooseItems: no items to choose from");
			return;
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].StoreItem && items[i].ActualCost <= GameVars.CoinsPerLevel * (SaveGame.Instance.WorldLevel - 1) && !items[i].Redundant(digger, chosenItems))
			{
				chosenItems.Add(items[i]);
			}
		}
		for (int j = 0; j < 100; j++)
		{
			if (chosenItems.Count >= cubes.Count)
			{
				break;
			}
			ItemDef itemDef = items[UnityEngine.Random.Range(0, items.Count)];
			if (itemDef.StoreItem && !itemDef.Redundant(digger, chosenItems))
			{
				chosenItems.Add(itemDef);
			}
		}
		if (chosenItems.Count < cubes.Count)
		{
			UnityEngine.Debug.LogError("ChooseItems: not enough chosenItems");
			return;
		}
		Shuffle(chosenItems);
		if (SingletonMonoBehaviour<World>.Instance.LoadingLevel == 2 && SingletonMonoBehaviour<Gui>.Instance.Ready && Gui.Views.StartView.ShouldEnableWheel)
		{
			chosenItems[1] = SingletonMonoBehaviour<Game>.Instance.GetItem("Fortune");
		}
		for (int k = 0; k < cubes.Count; k++)
		{
			cubes[k].Choose(chosenItems[k]);
		}
	}

	public static void Shuffle<T>(IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = rng.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}
}
