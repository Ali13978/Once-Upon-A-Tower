using UnityEngine;

public class ThwompRandomizer : CubeRandomizer
{
	public int Beats = 4;

	public override void Randomize(TileMap section)
	{
		ThwompCube component = GetComponent<ThwompCube>();
		if ((bool)component)
		{
			component.Offset = (float)Random.Range(0, Beats) * component.Period / (float)Beats;
		}
	}
}
