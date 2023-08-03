using System.Collections;
using UnityEngine;

public class Coin : WorldObject
{
	private void OnTriggerEnter(Collider other)
	{
		Digger component = other.GetComponent<Digger>();
		if ((bool)component)
		{
			OnHit(Vector3.zero, component);
		}
	}

	public override void OnSteppedOn(WorldObject stepper)
	{
		OnHit(Vector3.down, stepper);
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		base.OnHit(direction, hitter, medium);
		if (hitter is Digger)
		{
			SingletonMonoBehaviour<Game>.Instance.CoinsGrabbed++;
		}
		TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(Coord.Section);
		if ((bool)section && section.gameObject.activeInHierarchy)
		{
			section.StartCoroutine(Free());
		}
	}

	private IEnumerator Free()
	{
		yield return new WaitForSeconds(0.5f);
		if (base.gameObject != null)
		{
			SingletonMonoBehaviour<Pool>.Instance.Free(base.gameObject);
		}
	}
}
