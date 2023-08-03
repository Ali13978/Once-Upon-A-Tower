using TMPro;
using UnityEngine;

public class SaveMePackButton : GuiButton
{
	public TextMeshPro SaveMeCount;

	public TextMeshPro Price;

	public SaveMePack Pack;

	public void Setup(SaveMePack pack)
	{
		Pack = pack;
		SaveMeCount.text = pack.Size.ToString();
		if (!Application.isEditor)
		{
			Price.text = Purchaser.Instance.ProductPrice(pack.ProductId);
		}
		else
		{
			Price.text = "$" + ((float)Pack.PriceTier - 0.01f);
		}
	}
}
