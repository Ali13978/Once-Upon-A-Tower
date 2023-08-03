using System.Collections.Generic;
using UnityEngine;

public class RandomizeArrowPattern : CubeRandomizer
{
	public float SecondsPerBeat = 0.4f;

	public string[] Patterns;

	public GameObject[] DisableOnDoublePattern;

	public override void Randomize(TileMap section)
	{
		ArrowSource component = GetComponent<ArrowSource>();
		if (!component)
		{
			return;
		}
		string text = Patterns[Random.Range(0, Patterns.Length)];
		bool flag = text.StartsWith("101");
		for (int i = 0; i < DisableOnDoublePattern.Length; i++)
		{
			if (DisableOnDoublePattern[i] != null)
			{
				DisableOnDoublePattern[i].SetActive(!flag);
			}
		}
		float num = 0f;
		bool flag2 = true;
		List<float> list = new List<float>();
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] == '1')
			{
				if (flag2)
				{
					component.Offset = num;
				}
				else
				{
					list.Add(num);
				}
				flag2 = false;
				num = 0f;
			}
			num += SecondsPerBeat;
		}
		list.Add(num + component.Offset);
		component.Offset += (float)Random.Range(0, text.Length) * SecondsPerBeat;
		component.Period = list.ToArray();
	}
}
