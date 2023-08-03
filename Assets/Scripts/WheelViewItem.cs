using UnityEngine;

public class WheelViewItem : MonoBehaviour
{
	public string NextRunItem;

	public bool Egg;

	public GameObject[] Off;

	public GameObject[] On;

	public Transform Up;

	public void Activate()
	{
		if (Egg)
		{
			SingletonMonoBehaviour<Game>.Instance.ActivateChicken();
		}
		else
		{
			SingletonMonoBehaviour<Game>.Instance.GetItem(NextRunItem).Activate(SingletonMonoBehaviour<Game>.Instance.Digger);
		}
		SingletonMonoBehaviour<Game>.Instance.StartRun();
	}

	public void TurnOff()
	{
		for (int i = 0; i < Off.Length; i++)
		{
			if (Off[i] != null)
			{
				Off[i].SetActive(value: true);
			}
		}
		for (int j = 0; j < On.Length; j++)
		{
			if (On[j] != null)
			{
				On[j].SetActive(value: false);
			}
		}
	}

	public void TurnOn()
	{
		for (int i = 0; i < Off.Length; i++)
		{
			if (Off[i] != null)
			{
				Off[i].SetActive(value: false);
			}
		}
		for (int j = 0; j < On.Length; j++)
		{
			if (On[j] != null)
			{
				On[j].SetActive(value: true);
			}
		}
	}
}
