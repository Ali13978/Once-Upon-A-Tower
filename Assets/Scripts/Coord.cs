using System;
using UnityEngine;

[Serializable]
public struct Coord
{
	public int Section;

	public int x;

	public int y;

	public static Coord None = new Coord(0, 0);

	public static Coord Right = new Coord(1, 0);

	public static Coord Left = new Coord(-1, 0);

	public static Coord Up = new Coord(0, -1);

	public static Coord Down = new Coord(0, 1);

	public Coord(int section, int x, int y)
	{
		Section = section;
		this.x = x;
		this.y = y;
	}

	public Coord(int x, int y)
	{
		Section = -1;
		this.x = x;
		this.y = y;
	}

	public static Coord operator +(Coord a, Coord b)
	{
		return new Coord(Mathf.Max(a.Section, b.Section), a.x + b.x, a.y + b.y);
	}

	public static Coord operator -(Coord a, Coord b)
	{
		int section = -1;
		if (a.Section < 0)
		{
			section = b.Section;
		}
		else if (b.Section < 0)
		{
			section = a.Section;
		}
		else if (a.Section != b.Section)
		{
			section = b.Section;
			while (a.Section < b.Section)
			{
				TileMap section2 = SingletonMonoBehaviour<World>.Instance.GetSection(a.Section);
				if (section2 == null)
				{
					return None;
				}
				a.y -= section2.Rows;
				a.Section++;
			}
			while (a.Section > b.Section)
			{
				TileMap section3 = SingletonMonoBehaviour<World>.Instance.GetSection(b.Section);
				if (section3 == null)
				{
					return None;
				}
				a.y += section3.Rows;
				a.Section--;
			}
		}
		return new Coord(section, a.x - b.x, a.y - b.y);
	}

	public static bool operator ==(Coord a, Coord b)
	{
		return a.x == b.x && a.y == b.y && a.Section == b.Section;
	}

	public static bool operator !=(Coord a, Coord b)
	{
		return !(a == b);
	}

	public static implicit operator Coord(Vector2 v)
	{
		return new Coord((int)v.x, -(int)v.y);
	}

	public static implicit operator Coord(Vector3 v)
	{
		return new Coord((int)v.x, -(int)v.y);
	}

	public Coord Normalize()
	{
		return SingletonMonoBehaviour<World>.Instance.NormalizeCoord(this);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		return this == (Coord)obj;
	}

	public override int GetHashCode()
	{
		return Section * 1000000 + x * 1000 + y;
	}

	public override string ToString()
	{
		return "(" + Section + ", " + x + ", " + y + ")";
	}
}
