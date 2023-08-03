using System.Collections.Generic;
using UnityEngine;

public class LightsPostProcessor : MonoBehaviour
{
	public Material BlurMaterial;

	public Material BleedMaterial;

	public int BlurPass;

	public int BleedPasses;

	public List<RenderOnLights> RenderOnLights = new List<RenderOnLights>();

	private void Start()
	{
		if (Gui.AppleTV)
		{
			float value = (float)Screen.width * 1f / (float)Screen.height;
			BleedMaterial.SetFloat("_Aspect", value);
			BlurMaterial.SetFloat("_Aspect", value);
		}
	}

	private void OnPreCull()
	{
		for (int i = 0; i < RenderOnLights.Count; i++)
		{
			RenderOnLights renderOnLights = RenderOnLights[i];
			renderOnLights.OriginalMaterial = renderOnLights.Renderer.sharedMaterial;
			renderOnLights.OriginalLayer = renderOnLights.gameObject.layer;
			renderOnLights.Renderer.sharedMaterial = renderOnLights.Material;
			renderOnLights.gameObject.layer = Layer.Lights;
		}
	}

	private void OnPostRender()
	{
		for (int i = 0; i < RenderOnLights.Count; i++)
		{
			RenderOnLights renderOnLights = RenderOnLights[i];
			renderOnLights.Renderer.sharedMaterial = renderOnLights.OriginalMaterial;
			renderOnLights.gameObject.layer = renderOnLights.OriginalLayer;
		}
		if (Util.UseMultipleRenderTargets)
		{
			PostProcess(SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.LightDirMap, BleedPasses, 0);
		}
		PostProcess(SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.LightMap, BleedPasses, BlurPass);
	}

	private void PostProcess(RenderTexture target, int bleeds, int blurs)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(target.width, target.height, 0, target.format);
		for (int i = 0; i < blurs + bleeds; i++)
		{
			Material mat = (i >= bleeds) ? BlurMaterial : BleedMaterial;
			if (i % 2 == 0)
			{
				temporary.DiscardContents();
				Graphics.Blit(target, temporary, mat);
			}
			else
			{
				target.DiscardContents();
				Graphics.Blit(temporary, target, mat);
			}
		}
		if ((blurs + bleeds) % 2 == 1)
		{
			target.DiscardContents();
			Graphics.Blit(temporary, target);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}
}
