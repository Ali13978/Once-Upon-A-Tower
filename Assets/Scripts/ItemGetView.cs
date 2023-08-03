using UnityEngine;

public class ItemGetView : GuiView
{
	public Transform IconTemplate;

	public Material GuiMaterial;

	private GameObject icon;

	protected override void Start()
	{
		base.Start();
		IconTemplate.gameObject.SetActive(value: false);
	}

	public void ShowAnimated(string name)
	{
		ItemDef item = SingletonMonoBehaviour<Game>.Instance.GetItem(name);
		if (icon != null)
		{
			UnityEngine.Object.Destroy(icon);
		}
		if (!(item.Icon == null))
		{
			icon = Object.Instantiate(item.Icon);
			icon.transform.parent = IconTemplate.parent;
			icon.transform.localPosition = IconTemplate.localPosition;
			icon.transform.localScale = IconTemplate.localScale;
			MeshRenderer componentInChildren = icon.GetComponentInChildren<MeshRenderer>();
			Material[] materials = componentInChildren.materials;
			materials[0] = GuiMaterial;
			componentInChildren.materials = materials;
			if (base.Hiding)
			{
				StopCurrentSequence();
			}
			Show();
			Play((!(item is BombDef)) ? "Intro" : "IntroBomb", delegate
			{
				Hide();
			});
		}
	}
}
