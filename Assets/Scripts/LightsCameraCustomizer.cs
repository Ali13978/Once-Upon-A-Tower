using UnityEngine;

public class LightsCameraCustomizer : PropertyCustomizer
{
	protected override void Apply()
	{
		if (customizer != null)
		{
			if (Util.UseMultipleRenderTargets)
			{
				GetComponent<Camera>().SetTargetBuffers(new RenderBuffer[2]
				{
					customizer.LightMap.colorBuffer,
					customizer.LightDirMap.colorBuffer
				}, customizer.LightMap.depthBuffer);
			}
			else
			{
				GetComponent<Camera>().targetTexture = customizer.LightMap;
			}
		}
	}
}
