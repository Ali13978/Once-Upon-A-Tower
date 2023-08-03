using UnityEngine;

public class PlatformEnable : MonoBehaviour
{
	public bool Android = true;

	public bool iOS = true;

	public bool Web = true;

	public bool AppleTV = true;

	private void Start()
	{
		base.gameObject.SetActive(Android);
	}
}
