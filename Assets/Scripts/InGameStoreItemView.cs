using I2.Loc;
using TMPro;
using UnityEngine;

public class InGameStoreItemView : GuiView
{
	public TextMeshPro Title;

	public TextMeshPro Description;

	public Transform IconParent;

	public StoreCube lastCube;

	public void ShowAnimated(StoreCube storeCube)
	{
		if (storeCube != lastCube)
		{
			lastCube = storeCube;
			Title.text = ScriptLocalization.Get("Item" + storeCube.Item.name + "Name");
			Description.text = ScriptLocalization.Get("Item" + storeCube.Item.name + "Description");
			for (int i = 0; i < IconParent.childCount; i++)
			{
				Transform child = IconParent.GetChild(i);
				child.gameObject.SetActive(child.name == storeCube.Item.name);
			}
			ShowAnimated();
		}
	}

	public void HideAnimated(StoreCube storeCube)
	{
		if (storeCube == lastCube)
		{
			HideAnimated();
			lastCube = null;
		}
	}
}
