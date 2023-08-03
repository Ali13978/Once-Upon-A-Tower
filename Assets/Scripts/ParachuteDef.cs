public class ParachuteDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		digger.Walker.AirControl = true;
	}

	public override bool IsActive(Digger digger)
	{
		return digger.Walker.AirControl;
	}
}
