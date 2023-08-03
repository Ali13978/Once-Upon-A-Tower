public class ArmorDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		digger.Armor = true;
	}

	public override bool IsActive(Digger digger)
	{
		return digger.Armor;
	}
}
