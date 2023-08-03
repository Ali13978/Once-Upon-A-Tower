public class HardBootsDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		digger.HardBoots = true;
	}

	public override bool IsActive(Digger digger)
	{
		return digger.HardBoots;
	}
}
