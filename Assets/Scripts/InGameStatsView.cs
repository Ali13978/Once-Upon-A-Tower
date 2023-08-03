using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameStatsView : GuiView
{
	public TextMeshPro CoinsText;

	public TextMeshPro IncrementCoinsText;

	public Transform PowerUps;

	public float PowerUpScale = 0.5f;

	public Vector2 PowerUpSeparation = new Vector2(0.4f, 0.4f);

	public int PowerUpCols = 4;

	public Transform MissionCounterParent;

	public MissionCounter MissionCounterTemplate;

	public float MissionCounterSeparation = 0.6f;

	private List<MissionCounter> currentCounters = new List<MissionCounter>();

	private int coins = -1;

	private int powerups;

	private IEnumerator incrementCoins;

	protected override void Start()
	{
		base.Start();
		MissionCounterTemplate.gameObject.SetActive(value: false);
	}

	protected override void Update()
	{
		base.Update();
		if (SingletonMonoBehaviour<Game>.Instance.Digger == null)
		{
			return;
		}
		int num = SingletonMonoBehaviour<Game>.Instance.Digger.Coins;
		if (num != coins)
		{
			if (incrementCoins != null)
			{
				StopCoroutine(incrementCoins);
			}
			StartCoroutine(incrementCoins = IncrementCoins(coins, num));
			coins = num;
		}
		UpdateMissionCounters();
		int num2 = 0;
		if (!SingletonMonoBehaviour<Game>.Instance.Digger.IsDead || SingletonMonoBehaviour<Game>.Instance.Digger.WaitingSave)
		{
			for (int i = 0; i < SingletonMonoBehaviour<Game>.Instance.Items.Count; i++)
			{
				if (SingletonMonoBehaviour<Game>.Instance.Items[i].IsActive(SingletonMonoBehaviour<Game>.Instance.Digger))
				{
					num2 |= 1 << i;
				}
			}
		}
		if (powerups == num2)
		{
			return;
		}
		powerups = num2;
		while (PowerUps.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(PowerUps.GetChild(0).gameObject);
		}
		if (SingletonMonoBehaviour<Game>.Instance.Digger.IsDead && !SingletonMonoBehaviour<Game>.Instance.Digger.WaitingSave)
		{
			return;
		}
		int num3 = 0;
		for (int j = 0; j < SingletonMonoBehaviour<Game>.Instance.Items.Count; j++)
		{
			ItemDef itemDef = SingletonMonoBehaviour<Game>.Instance.Items[j];
			if (itemDef.IsActive(SingletonMonoBehaviour<Game>.Instance.Digger))
			{
				GameObject gameObject = Object.Instantiate(itemDef.Icon);
				gameObject.transform.parent = PowerUps;
				gameObject.transform.localScale = new Vector3(PowerUpScale, PowerUpScale, PowerUpScale);
				gameObject.transform.localPosition = new Vector3(PowerUpSeparation.x * (float)(num3 % PowerUpCols), PowerUpSeparation.y * (float)(num3 / PowerUpCols), 0f);
				num3++;
			}
		}
		Vector3 localPosition = PowerUps.transform.localPosition;
		localPosition.x = (float)(-(num3 - 1)) * PowerUpSeparation.x / 2f;
		PowerUps.transform.localPosition = localPosition;
	}

	private IEnumerator IncrementCoins(int coins, int newCoins)
	{
		if (newCoins <= coins)
		{
			CoinsText.text = newCoins.ToString();
			yield break;
		}
		IncrementCoinsText.text = "+" + (newCoins - coins);
		Play("IncrementCoins");
		CoinsText.text = newCoins.ToString();
	}

	private void UpdateMissionCounters()
	{
		MissionSet currentMissionSet = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		if (currentMissionSet == null || (SingletonMonoBehaviour<Game>.Instance.Digger.IsDead && !SingletonMonoBehaviour<Game>.Instance.Digger.WaitingSave))
		{
			foreach (MissionCounter currentCounter in currentCounters)
			{
				UnityEngine.Object.Destroy(currentCounter.gameObject);
			}
			currentCounters.Clear();
			return;
		}
		int num = 0;
		for (int i = 0; i < currentMissionSet.Missions.Length; i++)
		{
			MissionStatus mission = currentMissionSet.Missions[i];
			if (MissionCounterTemplate.ShouldCount(mission))
			{
				while (num >= currentCounters.Count)
				{
					MissionCounter component = Object.Instantiate(MissionCounterTemplate.gameObject).GetComponent<MissionCounter>();
					component.transform.parent = MissionCounterTemplate.transform.parent;
					component.transform.localPosition = MissionCounterTemplate.transform.localPosition + Vector3.down * MissionCounterSeparation * num;
					component.gameObject.SetActive(value: true);
					currentCounters.Add(component);
				}
				currentCounters[num].Setup(mission);
				num++;
			}
		}
		for (int j = num; j < currentCounters.Count; j++)
		{
			UnityEngine.Object.Destroy(currentCounters[j].gameObject);
		}
		if (currentCounters.Count - num > 0)
		{
			currentCounters.RemoveRange(num, currentCounters.Count - num);
		}
	}
}
