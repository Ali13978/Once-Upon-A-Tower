using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertyCustomizer : MonoBehaviour
{
	protected WorldCustomizer customizer;

	private static List<PropertyCustomizer> customizers = new List<PropertyCustomizer>();

	protected virtual void Start()
	{
		Reset();
	}

	private void Customize()
	{
		if (!(customizer == SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer) || !Application.isPlaying)
		{
			customizer = SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer;
			if (Application.isEditor && customizer == null)
			{
				customizer = UnityEngine.Object.FindObjectOfType<WorldCustomizer>();
			}
			Apply();
		}
	}

	public static void CustomizeAll()
	{
		foreach (PropertyCustomizer item in customizers.ToList())
		{
			item.Customize();
		}
	}

	private void OnEnable()
	{
		customizers.Add(this);
		Reset();
	}

	private void OnDisable()
	{
		customizers.Remove(this);
	}

	protected virtual void Apply()
	{
	}

	public void Reset()
	{
		customizer = null;
		Customize();
	}
}
