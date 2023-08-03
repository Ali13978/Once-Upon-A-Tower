using UnityEngine;

public class DayCycleAlpha : MonoBehaviour
{
	public Renderer Renderer;

	public AnimationCurve Curve;

	public int Keyframe;

	private void Update()
	{
		float num = SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.KeyframeFactor(Keyframe);
		Renderer.enabled = (num > 0f);
		Renderer.sharedMaterial.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f * num));
	}
}
