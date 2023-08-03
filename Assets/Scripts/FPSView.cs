using TMPro;
using UnityEngine;

public class FPSView : GuiView
{
	public TextMeshPro Text;

	public GuiButton Button;

	private int FPS;

	private float lastTime;

	private int frameCount;

	protected override void Start()
	{
		base.Start();
		Text.gameObject.SetActive(value: false);
		Button.Click = delegate
		{
			Text.gameObject.SetActive(!Text.gameObject.activeSelf);
		};
	}

	protected override void Update()
	{
		if (!base.Visible)
		{
			return;
		}
		base.Update();
		frameCount++;
		if (frameCount > 10)
		{
			float num = Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			int num2 = Mathf.RoundToInt((float)frameCount / num);
			if (num2 != FPS)
			{
				FPS = num2;
				Text.text = FPS.ToString();
			}
			frameCount = 0;
		}
	}
}
