public class BombDef : ItemDef
{
	public int Count;

	public override void Activate(Digger digger)
	{
		SingletonMonoBehaviour<Game>.Instance.Digger.Bombs += Count;
	}

	public override bool Equals(ItemDef other)
	{
		return base.Equals(other) && Count == ((BombDef)other).Count;
	}
}
