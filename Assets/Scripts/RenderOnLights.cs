using UnityEngine;

public class RenderOnLights : MonoBehaviour
{
	public Material Material;

	[HideInInspector]
	public LayerMask OriginalLayer;

	[HideInInspector]
	public Material OriginalMaterial;

	[HideInInspector]
	public Renderer Renderer;

	private void OnEnable()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance() && !SingletonMonoBehaviour<Game>.Instance.LowPerformance)
		{
			Renderer = GetComponent<Renderer>();
			if (Renderer != null && Renderer.enabled)
			{
				SingletonMonoBehaviour<Game>.Instance.LightsPostProcessor.RenderOnLights.Add(this);
			}
		}
	}

	private void OnDisable()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance() && !SingletonMonoBehaviour<Game>.Instance.LowPerformance)
		{
			SingletonMonoBehaviour<Game>.Instance.LightsPostProcessor.RenderOnLights.Remove(this);
		}
	}
}
