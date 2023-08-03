public class ChickenDef : ItemDef
{
	public override void Activate(Digger digger)
	{
		SingletonMonoBehaviour<Game>.Instance.ActivateChicken();
	}

	public override bool IsActive(Digger digger)
	{
		return SingletonMonoBehaviour<Game>.Instance.Chicken != null && !SingletonMonoBehaviour<Game>.Instance.Chicken.Broken;
	}
}
