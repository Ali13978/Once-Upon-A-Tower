using System;
using UnityEngine;

public class GiftCube : WorldObject
{
	public Transform IconParent;

	public Vector3 IconScale = Vector3.one;

	public GameObject Crown;

	public Material TowerGiftMaterial;

	public Gift Gift;

	private void Start()
	{
		Gift = SingletonMonoBehaviour<GiftManager>.Instance.Choose(DateTime.Now);
		if (Gift.Type == GiftType.Character)
		{
			Crown.SetActive(value: true);
			return;
		}
		Crown.SetActive(value: false);
		ItemDef item = SingletonMonoBehaviour<Game>.Instance.GetItem(Gift.Name);
		GameObject gameObject = UnityEngine.Object.Instantiate(item.Icon);
		gameObject.transform.parent = IconParent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = IconScale;
		MeshRenderer componentInChildren = gameObject.GetComponentInChildren<MeshRenderer>();
		Material[] materials = componentInChildren.materials;
		materials[0] = TowerGiftMaterial;
		componentInChildren.materials = materials;
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (hitter is Digger)
		{
			Breakable.Break(direction, hitter);
			Gift.Redeem();
		}
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (ShouldHit(wo))
		{
			wo.OnHit(direction, this);
		}
		if (wo.Broken)
		{
			Walker.Move(direction);
		}
		return false;
	}

	private bool ShouldHit(WorldObject wo)
	{
		return wo is Enemy || wo is Coin || wo is PushCube || wo is Digger;
	}

	public override void OnGrounded()
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + Coord.Down).Normalize());
		if (ShouldHit(tile))
		{
			tile.OnHit(Vector3.down, this);
		}
	}

	public override void Disarm()
	{
		Breakable.Break(Vector3.up);
	}
}
