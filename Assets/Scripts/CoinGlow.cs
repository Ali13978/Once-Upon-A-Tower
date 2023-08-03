using UnityEngine;

public class CoinGlow : MonoBehaviour
{
	public GameObject Activate;

	private WorldObject Wo;

	private void Start()
	{
		Wo = GetComponent<WorldObject>();
	}

	private void FixedUpdate()
	{
		Activate.SetActive(!Wo.Breakable.Broken && Wo.Coins > 20);
	}
}
