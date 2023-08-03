using UnityEngine;

public class ActiveOnlyOnUnbreakable : CubeRandomizer
{
	public override void Randomize(TileMap section)
	{
		WorldObject component = GetComponent<WorldObject>();
		if (component.Coord.y >= section.Rows - 1)
		{
			UnityEngine.Debug.LogError("Last row active on unbreakable");
			base.gameObject.SetActive(value: false);
		}
		else
		{
			WorldObject worldObject = section.TileMatrix[component.Coord.x, component.Coord.y + 1];
			base.gameObject.SetActive(worldObject != null && worldObject.GetComponent<Breakable>() == null);
		}
	}
}
