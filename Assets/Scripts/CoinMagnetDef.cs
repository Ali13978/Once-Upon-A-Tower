public class CoinMagnetDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		if (digger.CoinMagnet == null)
		{
			digger.CoinMagnet = digger.gameObject.AddComponent<CoinMagnet>();
		}
	}

	public override bool IsActive(Digger digger)
	{
		return digger.CoinMagnet != null;
	}
}
