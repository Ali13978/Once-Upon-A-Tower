using UnityEngine;

public class DebugLightMap : MonoBehaviour
{
	private void Update()
	{
		if (SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer != null)
		{
			GetComponent<Renderer>().material.mainTexture = SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.LightMap;
		}
	}
}
