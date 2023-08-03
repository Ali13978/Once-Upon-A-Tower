using System;
using UnityEngine.Purchasing;

[Serializable]
public class SaveMePack : IProduct
{
	public int Size;

	public int PriceTier;

	public string ReferenceName => "SaveMePack" + Size;

	public string ProductId => "tower.savemepack" + Size;

	public ProductType ProductType => ProductType.Consumable;
}
