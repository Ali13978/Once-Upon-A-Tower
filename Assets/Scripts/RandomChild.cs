using UnityEngine;

public class RandomChild : MonoBehaviour
{
	public Renderer[] Renderers;

	public float[] Weights;

	public float TotalWeight;

	public void Initialize()
	{
		TotalWeight = 0f;
		Weights = new float[base.transform.childCount];
		Renderers = new Renderer[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			RandomChildWeight component = base.transform.GetChild(i).GetComponent<RandomChildWeight>();
			float num = (!(component != null)) ? 1f : component.Weight;
			TotalWeight += num;
			Weights[i] = num;
			Renderers[i] = base.transform.GetChild(i).GetComponent<Renderer>();
		}
	}

	private void Start()
	{
		float num = UnityEngine.Random.Range(0f, TotalWeight);
		int num2 = Renderers.Length - 1;
		for (int i = 0; i < Renderers.Length; i++)
		{
			float num3 = (i >= Weights.Length) ? 1f : Weights[i];
			num -= num3;
			if (num <= 0f)
			{
				num2 = i;
				break;
			}
		}
		for (int j = 0; j < Renderers.Length; j++)
		{
			Renderers[j].enabled = (j == num2);
		}
	}
}
