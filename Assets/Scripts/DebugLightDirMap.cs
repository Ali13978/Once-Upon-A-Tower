using UnityEngine;

public class DebugLightDirMap : MonoBehaviour
{
	private void Update()
	{
		if (SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer != null)
		{
			GetComponent<Renderer>().material.mainTexture = SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.LightDirMap;
		}
	}
}
