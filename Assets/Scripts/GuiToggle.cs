using UnityEngine;

public class GuiToggle : GuiButton
{
	public GameObject[] ActiveParts;

	public GameObject[] InactiveParts;

	public bool Toggle = true;

	protected override void Start()
	{
		base.Start();
		UpdateToggle();
	}

	public void SetToggle(bool value)
	{
		Toggle = value;
		UpdateToggle();
	}

	private void UpdateToggle()
	{
		for (int i = 0; i < ActiveParts.Length; i++)
		{
			ActiveParts[i].SetActive(Toggle);
		}
		for (int j = 0; j < InactiveParts.Length; j++)
		{
			InactiveParts[j].SetActive(!Toggle);
		}
	}
}
