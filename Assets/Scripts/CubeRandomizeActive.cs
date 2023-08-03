using UnityEngine;

public class CubeRandomizeActive : CubeRandomizer
{
	public float Likelihood = 0.5f;

	public override void Randomize(TileMap section)
	{
		bool flag = Random.Range(0f, 1f) <= Likelihood;
		base.gameObject.SetActive(flag);
		if (!flag)
		{
			WorldObject component = GetComponent<WorldObject>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("CubeRandomizeActive without WorldObject", this);
			}
			else
			{
				component.RemoveCoord();
			}
		}
	}
}
