using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WheelView : GuiView
{
	public Transform Wheel;

	public Transform ItemsParent;

	public TextMeshPro TitleText;

	public TextMeshPro DescriptionText;

	public int WheelCount = 8;

	public float Acceleration = 4f;

	public float StartSpeed = 200f;

	public float SpinTime = 8f;

	public Vector3 ItemIconScale = Vector3.one;

	public List<WheelViewItem> templateItems;

	public List<WheelViewItem> currentItems;

	public GuiButton SpinButton;

	public GuiButton RestartButton;

	public GameObject Buttons;

	public Animator WheelAnimator;

	public AudioSource Loop;

	public AudioSource Spin;

	private IEnumerator idleCoroutine;

	private GuiButton defaultButton;

	protected override void Start()
	{
		base.Start();
		templateItems = ItemsParent.GetComponentsInChildren<WheelViewItem>(includeInactive: true).ToList();
		for (int i = 0; i < templateItems.Count; i++)
		{
			templateItems[i].gameObject.SetActive(value: false);
		}
		currentItems = new List<WheelViewItem>(templateItems.Count);
		RestartButton.Click = delegate
		{
			HideAnimated();
			SingletonMonoBehaviour<Game>.Instance.StartRun();
		};
	}

	public override void ShowAnimated()
	{
		base.ShowAnimated();
		SaveGame.Instance.NextLuckyTime = DateTime.Now + GameVars.LuckyTimeCooldown;
		SaveGame.Instance.Save();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(defaultButton = SpinButton);
		WheelAnimator.SetTrigger("start");
		Buttons.SetActive(value: false);
		PrepareWheel();
		if (Loop != null)
		{
			Loop.Play();
		}
		SpinButton.gameObject.SetActive(value: true);
		SpinButton.Click = delegate
		{
			defaultButton = null;
			SpinButton.gameObject.SetActive(value: false);
			StartCoroutine(SpinWheel());
		};
		TitleText.text = string.Empty;
		DescriptionText.text = string.Empty;
		StartCoroutine(idleCoroutine = IdleCoroutine());
	}

	private IEnumerator IdleCoroutine()
	{
		yield return new WaitForSeconds(3f);
		Play("Idle");
	}

	private void PrepareWheel()
	{
		string[] itemNames = new string[8]
		{
			"Chicken",
			"Armor",
			"Chicken",
			"HardBoots",
			"Chicken",
			"FireWeapon",
			"Chicken",
			"BombX3"
		};
		foreach (WheelViewItem currentItem in currentItems)
		{
			UnityEngine.Object.Destroy(currentItem.gameObject);
		}
		currentItems.Clear();
		int i;
		for (i = 0; i < itemNames.Length; i++)
		{
			WheelViewItem wheelViewItem = templateItems.Find((WheelViewItem it) => it.NextRunItem == itemNames[i]);
			if (wheelViewItem == null)
			{
				UnityEngine.Debug.LogError("Can't find template for " + itemNames[i]);
			}
			WheelViewItem wheelViewItem2 = UnityEngine.Object.Instantiate(wheelViewItem, wheelViewItem.transform.parent);
			wheelViewItem2.gameObject.SetActive(value: true);
			wheelViewItem2.TurnOff();
			wheelViewItem2.transform.localRotation = Quaternion.AngleAxis(45f * (float)i, Vector3.forward);
			currentItems.Add(wheelViewItem2);
		}
	}

	private IEnumerator SpinWheel()
	{
		StopCoroutine(idleCoroutine);
		if (IsPlayingSequence("Idle"))
		{
			StopCurrentSequence();
		}
		PlaySingle("Tap");
		float startTime = Time.fixedTime;
		Quaternion startRotation = Wheel.localRotation;
		SingletonMonoBehaviour<Game>.Instance.ResetFadeTime();
		UnityEngine.Random.InitState((int)(Time.realtimeSinceStartup * 1000f));
		int chosenIndex = UnityEngine.Random.Range(0, currentItems.Count);
		WheelViewItem ChosenItem = currentItems[chosenIndex];
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("ChosenItem " + chosenIndex + " " + ChosenItem.name, ChosenItem);
		}
		Vector3 itemDir = ChosenItem.transform.TransformDirection(ChosenItem.Up.localPosition);
		itemDir.z = 0f;
		float endAngle2 = Vector3.Angle(itemDir, Vector3.up);
		if (itemDir.x > 0f)
		{
			endAngle2 = 360f - endAngle2;
		}
		endAngle2 += (float)(Mathf.RoundToInt(StartSpeed * SpinTime / 360f) * 360);
		Loop.Stop();
		Spin.Play();
		while (Time.fixedTime - startTime < SpinTime)
		{
			float t2 = (Time.fixedTime - startTime) / SpinTime;
			t2 = Tween.CubicEaseOut(t2, 0f, 1f, 1f);
			Wheel.localRotation = startRotation * Quaternion.AngleAxis(endAngle2 * t2, Vector3.forward);
			yield return new WaitForFixedUpdate();
		}
		Wheel.localRotation = startRotation * Quaternion.AngleAxis(endAngle2, Vector3.forward);
		TitleText.text = ScriptLocalization.Get("Item" + ChosenItem.NextRunItem + "Name");
		DescriptionText.text = ScriptLocalization.Get("Item" + ChosenItem.NextRunItem + "Description");
		ChosenItem.TurnOn();
		ChosenItem.Activate();
		SingletonMonoBehaviour<Gui>.Instance.UpdateFocus(defaultButton = RestartButton);
		Buttons.SetActive(value: true);
		Play("Reward");
	}

	public override bool OnMenuButton()
	{
		if (defaultButton != null && defaultButton.Click != null && base.Visible && !base.Hiding)
		{
			defaultButton.Click();
		}
		return true;
	}

	public override bool HandlesMenuButton()
	{
		return base.Visible;
	}
}
