using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
	private WorldObject wo;

	private Coord lastCoord = Coord.None;

	private void Start()
	{
		wo = GetComponent<WorldObject>();
	}

	private void Update()
	{
		if (wo != null && wo.Coord != lastCoord)
		{
			HitCoin(Vector3.right);
			HitCoin(Vector3.left);
			HitCoin(Vector3.up);
			lastCoord = wo.Coord;
		}
	}

	private void HitCoin(Vector3 dir)
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(wo.Coord + dir);
		if (tile != null && (bool)tile.GetComponent<Coin>())
		{
			tile.OnHit(dir, wo);
		}
	}
}
