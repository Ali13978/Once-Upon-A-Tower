using System.Collections.Generic;
using UnityEngine;

public class ItemDef : MonoBehaviour
{
	public GameObject Icon;

	public int Cost;

	public bool StoreItem => Cost > 0;

	public int ActualCost
	{
		get
		{
			float num = GameVars.StoreLevelMultiplier[Mathf.Clamp(SingletonMonoBehaviour<World>.Instance.LoadingLevel - 2, 0, GameVars.StoreLevelMultiplier.Length - 1)];
			return Mathf.RoundToInt((float)Cost * num);
		}
	}

	public virtual bool IsActive(Digger digger)
	{
		return false;
	}

	public virtual void Activate(Digger digger)
	{
	}

	public virtual bool Redundant(Digger digger, List<ItemDef> items)
	{
		if (IsActive(digger))
		{
			return true;
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Equals(this))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool Equals(ItemDef other)
	{
		return GetType() == other.GetType() && Cost == other.Cost;
	}
}
