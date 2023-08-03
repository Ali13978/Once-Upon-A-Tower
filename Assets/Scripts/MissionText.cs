using TMPro;
using UnityEngine;

public class MissionText : GuiView
{
	private MissionStatus missionStatus;

	public TextMeshPro Text;

	public Renderer TextRenderer;

	public Strikethrough Strikethrough;

	public Material PendingMaterial;

	public Material CompletedMaterial;

	public GameObject Tick;

	public GameObject TickParent;

	public float CenterOffset = 1f;

	public MissionStatus MissionStatus
	{
		get
		{
			return missionStatus;
		}
		set
		{
			if (value == null)
			{
				Text.text = " ";
			}
			else
			{
				TextRenderer.material.SetColor("_FaceColor", PendingMaterial.GetColor("_FaceColor"));
				Text.text = value.Text;
			}
			missionStatus = value;
		}
	}

	public void ShowIncomplete()
	{
		Show();
		Strikethrough.Clear();
		TickParent.SetActive(value: true);
		Tick.SetActive(value: false);
		StopCurrentSequence();
		TextRenderer.material.SetColor("_FaceColor", PendingMaterial.GetColor("_FaceColor"));
	}

	public void ShowComplete()
	{
		Show();
		TickParent.SetActive(value: true);
		Tick.SetActive(value: true);
		Strikethrough.Show();
		StopCurrentSequence();
		TextRenderer.material.SetColor("_FaceColor", CompletedMaterial.GetColor("_FaceColor"));
	}

	public void ShowCompleteAnimated()
	{
		Show();
		TickParent.SetActive(value: true);
		Tick.SetActive(value: true);
		Strikethrough.Show();
		Play("Strikethrough");
	}

	public void Clear()
	{
		MissionStatus = null;
		TickParent.SetActive(value: false);
	}

	public void Center(float size = 0f)
	{
		if (size == 0f)
		{
			Vector3 size2 = Text.bounds.size;
			size = size2.x;
		}
		base.transform.localPosition = Vector3.right * (0f - size / 2f + CenterOffset);
	}

	public static void CenterAndSize(MissionText[] missionTexts, float maxFontSize)
	{
		float num = 0f;
		float num2 = maxFontSize;
		foreach (MissionText missionText in missionTexts)
		{
			Vector3 size = missionText.Text.bounds.size;
			num = Mathf.Max(size.x, num);
			num2 = Mathf.Min(missionText.Text.fontSize, num2);
		}
		foreach (MissionText missionText2 in missionTexts)
		{
			missionText2.Center(num);
			missionText2.Text.fontSizeMax = num2;
		}
	}
}
