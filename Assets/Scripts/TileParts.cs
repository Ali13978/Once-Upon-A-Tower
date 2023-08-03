using System;
using UnityEngine;

public class TileParts : MonoBehaviour
{
	[Serializable]
	public class PartsPerSide
	{
		public GameObject[] Sides;

		public GameObject[] Ornaments;

		public GameObject[] Joints;
	}

	public float OrnametChance = 1f;

	public float JointChance = 1f;

	public WorldObject Tile;

	public GameObject[] Mid;

	public GameObject[] MidOutleft;

	public GameObject[] MidOutright;

	public PartsPerSide[] Parts;

	public bool OnlyOut;

	public bool OnlyIn;

	public bool OnlyOutLeft;

	public bool OnlyOutRight;

	private TileSide initialEmptySides = TileSide.All;

	public void Initialize()
	{
		Mid = new GameObject[0];
		MidOutleft = new GameObject[0];
		MidOutright = new GameObject[0];
		Parts = new PartsPerSide[16];
		for (int i = 0; i < Parts.Length; i++)
		{
			Parts[i] = new PartsPerSide
			{
				Sides = new GameObject[0],
				Ornaments = new GameObject[0],
				Joints = new GameObject[0]
			};
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			Transform child = base.transform.GetChild(j);
			if (child.name.Contains("Middle"))
			{
				if (child.name.Contains("Outleft"))
				{
					ArrayAdd(ref MidOutleft, child.gameObject);
				}
				else if (child.name.Contains("Outright"))
				{
					ArrayAdd(ref MidOutright, child.gameObject);
				}
				else
				{
					ArrayAdd(ref Mid, child.gameObject);
				}
				continue;
			}
			TileSide sides = GetSides(child.name);
			bool flag = child.name.Contains("Ornament");
			if (flag || sides != 0)
			{
				PartsPerSide partsPerSide = Parts[(int)sides];
				child.gameObject.SetActive(value: false);
				if (flag)
				{
					ArrayAdd(ref partsPerSide.Ornaments, child.gameObject);
				}
				else if (child.name.Contains("Joint"))
				{
					ArrayAdd(ref partsPerSide.Joints, child.gameObject);
				}
				else
				{
					ArrayAdd(ref partsPerSide.Sides, child.gameObject);
				}
			}
		}
	}

	private void ArrayAdd(ref GameObject[] array, GameObject element)
	{
		Array.Resize(ref array, array.Length + 1);
		array[array.Length - 1] = element;
	}

	public void UpdateSides(TileSide sides, TileSide emptySides)
	{
		if (Tile != null)
		{
			if (!Tile.Outleft && !Tile.Outright && OnlyOut)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			if (!Tile.Outleft && OnlyOutLeft)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			if (!Tile.Outright && OnlyOutRight)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			if ((Tile.Outright || Tile.Outleft) && OnlyIn)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			base.gameObject.SetActive(value: true);
		}
		if (initialEmptySides == TileSide.All)
		{
			initialEmptySides = emptySides;
		}
		for (int i = 0; i < Mid.Length; i++)
		{
			Mid[i].SetActive(value: false);
		}
		for (int j = 0; j < MidOutleft.Length; j++)
		{
			MidOutleft[j].SetActive(value: false);
		}
		for (int k = 0; k < MidOutright.Length; k++)
		{
			MidOutright[k].SetActive(value: false);
		}
		if (Mid != null)
		{
			UnityEngine.Random.InitState(GetInstanceID());
			GameObject[] array = Mid;
			if (Tile != null && Tile.Outleft && MidOutleft.Length > 0)
			{
				array = MidOutleft;
			}
			if (Tile != null && Tile.Outright && MidOutright.Length > 0)
			{
				array = MidOutright;
			}
			if (array.Length > 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				array[num].SetActive(value: true);
			}
		}
		if (Parts == null)
		{
			return;
		}
		for (int l = 0; l < Parts.Length; l++)
		{
			PartsPerSide partsPerSide = Parts[l];
			for (int m = 0; m < partsPerSide.Sides.Length; m++)
			{
				partsPerSide.Sides[m].SetActive(value: false);
			}
			for (int n = 0; n < partsPerSide.Ornaments.Length; n++)
			{
				partsPerSide.Ornaments[n].SetActive(value: false);
			}
			for (int num2 = 0; num2 < partsPerSide.Joints.Length; num2++)
			{
				partsPerSide.Joints[num2].SetActive(value: false);
			}
		}
		UnityEngine.Random.InitState(base.gameObject.GetInstanceID());
		bool flag = UnityEngine.Random.Range(0, 6) == 0;
		for (int num3 = 0; num3 < Parts.Length; num3++)
		{
			if (((int)sides & num3) != 0)
			{
				EnableRandom(Parts[num3].Sides, num3);
				if (flag && ((int)initialEmptySides & num3) == 0)
				{
					EnableRandom(Parts[num3].Ornaments, num3, OrnametChance);
				}
			}
			if ((num3 & (int)(~sides)) == num3)
			{
				EnableRandom(Parts[num3].Joints, num3, JointChance);
			}
		}
	}

	private void EnableRandom(GameObject[] list, int side, float chance = 1f)
	{
		if (list != null && list.Length != 0)
		{
			UnityEngine.Random.InitState(GetInstanceID() + side);
			if (!(UnityEngine.Random.Range(0f, 1f) < 1f - chance))
			{
				list[UnityEngine.Random.Range(0, list.Length)].SetActive(value: true);
			}
		}
	}

	private TileSide GetSides(string name)
	{
		TileSide tileSide = TileSide.None;
		if (name.Contains("Left"))
		{
			tileSide |= TileSide.Left;
		}
		if (name.Contains("Right"))
		{
			tileSide |= TileSide.Right;
		}
		if (name.Contains("Top"))
		{
			tileSide |= TileSide.Top;
		}
		if (name.Contains("Down"))
		{
			tileSide |= TileSide.Down;
		}
		return tileSide;
	}
}
