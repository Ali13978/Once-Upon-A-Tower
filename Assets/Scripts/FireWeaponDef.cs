public class FireWeaponDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		digger.HitDistance = 1;
	}

	public override bool IsActive(Digger digger)
	{
		return digger.HitDistance > 0;
	}
}
