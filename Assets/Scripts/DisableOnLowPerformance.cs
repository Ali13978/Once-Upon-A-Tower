using UnityEngine;

public class DisableOnLowPerformance : MonoBehaviour
{
	private void Start()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance() && SingletonMonoBehaviour<Game>.Instance.LowPerformance)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance() && SingletonMonoBehaviour<Game>.Instance.LowPerformance)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
