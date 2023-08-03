using UnityEngine;

public class WorldEnabler : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.SetActive(SingletonMonoBehaviour<Game>.Instance.CurrentWorld == base.name);
	}
}
