using UnityEngine;

public class CubeRandomizeAlt : CubeRandomizer
{
	public GameObject Alternative;

	public bool InactiveAlternative;

	public override void Randomize(TileMap section)
	{
		if (Alternative == null)
		{
			UnityEngine.Debug.LogError("CubeRandomizeAlt without alternative");
			return;
		}
		int num = (!InactiveAlternative) ? Random.Range(0, 2) : Random.Range(0, 3);
		base.gameObject.SetActive(num == 0);
		Alternative.SetActive(num == 1);
	}
}
