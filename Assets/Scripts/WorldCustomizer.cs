using System;
using UnityEngine;

public class WorldCustomizer : MonoBehaviour
{
	public WorldKeyframe[] DayKeyframes;

	public WorldKeyframe CustomKeyframe;

	[Range(0f, 1f)]
	public float CustomKeyframeAlpha;

	public RenderTexture PaletteTarget;

	public RenderTexture SkyGradientTarget;

	public RenderTexture LightsSkyTarget;

	public RenderTexture StarsGradientTarget;

	public AnimationCurve Curve;

	public Shader BlendShader;

	private Material BlendMaterial;

	public RenderTexture LightMap;

	public RenderTexture LightDirMap;

	public Texture2D RampGame;

	[Range(0f, 4f)]
	public float DayCycle;

	public float DayCycleSpeed = 1f;

	public float LoadingCycleSpeed = 1f;

	public bool IsDay => KeyframeFactor(2) >= 0.5f;

	public void Update()
	{
		bool flag = !SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready;
		if (!SaveGame.Instance.TutorialComplete)
		{
			DayCycle = 0.2f;
		}
		else if (flag)
		{
			DayCycle += LoadingCycleSpeed * Time.unscaledDeltaTime;
		}
		else
		{
			DayCycle += DayCycleSpeed * Time.deltaTime;
		}
		if (Time.frameCount % 3 == 0 || !Application.isPlaying)
		{
			Blend(DayCycle, (WorldKeyframe k) => k.Palette, PaletteTarget);
		}
		if (Time.frameCount % 3 == 1 || !Application.isPlaying || flag)
		{
			Blend(DayCycle, (WorldKeyframe k) => k.SkyGradient, SkyGradientTarget);
			Blend(DayCycle, (WorldKeyframe k) => k.StarsGradient, StarsGradientTarget);
		}
		if (Time.frameCount % 3 == 2 || !Application.isPlaying)
		{
			Blend(DayCycle, (WorldKeyframe k) => k.LightsSky, LightsSkyTarget);
		}
		if (SingletonMonoBehaviour<Game>.HasInstance() && !SingletonMonoBehaviour<Game>.Instance.LowPerformance && CustomKeyframe != null && CustomKeyframeAlpha > 0f)
		{
			if ((bool)CustomKeyframe.Palette)
			{
				CustomBlend((WorldKeyframe k) => k.Palette, CustomKeyframe.Palette, PaletteTarget);
			}
			if ((bool)CustomKeyframe.SkyGradient)
			{
				CustomBlend((WorldKeyframe k) => k.SkyGradient, CustomKeyframe.SkyGradient, SkyGradientTarget);
			}
			if ((bool)CustomKeyframe.LightsSky)
			{
				CustomBlend((WorldKeyframe k) => k.LightsSky, CustomKeyframe.LightsSky, LightsSkyTarget);
			}
			if ((bool)CustomKeyframe.StarsGradient)
			{
				CustomBlend((WorldKeyframe k) => k.StarsGradient, CustomKeyframe.StarsGradient, StarsGradientTarget);
			}
		}
	}

	public float KeyframeFactor(int keyframe)
	{
		return KeyframeFactor(DayCycle, keyframe);
	}

	private float KeyframeFactor(float t, int keyframe)
	{
		int num = Mathf.FloorToInt(t) % DayKeyframes.Length;
		int num2 = (num + 1) % DayKeyframes.Length;
		float num3 = Curve.Evaluate(t - Mathf.Floor(t));
		if (num == keyframe)
		{
			return 1f - num3;
		}
		if (num2 == keyframe)
		{
			return num3;
		}
		return 0f;
	}

	private void CustomBlend(Func<WorldKeyframe, Texture2D> getTexture0, Texture2D texture1, RenderTexture target)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(target.width, target.height, 0, target.format);
		Blend(DayCycle, getTexture0, temporary);
		Blend(CustomKeyframeAlpha, temporary, texture1, target);
	}

	private void Blend(float t, Func<WorldKeyframe, Texture2D> getTexture, RenderTexture target)
	{
		if (!(target == null))
		{
			int num = Mathf.FloorToInt(t) % DayKeyframes.Length;
			int num2 = (num + 1) % DayKeyframes.Length;
			Blend(Curve.Evaluate(t - Mathf.Floor(t)), getTexture(DayKeyframes[num]), getTexture(DayKeyframes[num2]), target);
		}
	}

	private void Blend(float t, Texture texture0, Texture texture1, RenderTexture target)
	{
		if (BlendMaterial == null)
		{
			BlendMaterial = new Material(BlendShader);
		}
		BlendMaterial.SetTexture("_OtherTex", texture1);
		BlendMaterial.SetFloat("_Blend", t);
		target.DiscardContents();
		Graphics.Blit(texture0, target, BlendMaterial);
	}
}
