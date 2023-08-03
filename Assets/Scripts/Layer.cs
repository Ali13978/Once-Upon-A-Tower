using UnityEngine;

public class Layer
{
	public static int Digger;

	public static int Cube;

	public static int Enemy;

	public static int Coin;

	public static int Lights;

	public static int DiggerMask;

	public static int CubeMask;

	public static int EnemyMask;

	public static int CoinMask;

	static Layer()
	{
		Digger = LayerMask.NameToLayer("Digger");
		DiggerMask = 1 << Digger;
		Cube = LayerMask.NameToLayer("Cube");
		CubeMask = 1 << Cube;
		Enemy = LayerMask.NameToLayer("Enemy");
		EnemyMask = 1 << Enemy;
		Coin = LayerMask.NameToLayer("Coin");
		CoinMask = 1 << Coin;
		Lights = LayerMask.NameToLayer("Lights");
	}
}
