public class HeavyBootsDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		digger.HitSidesWhenLanding = true;
	}

	public override bool IsActive(Digger digger)
	{
		return digger.HitSidesWhenLanding;
	}
}
