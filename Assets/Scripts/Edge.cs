using UnityEngine;

public struct Edge
{
	public Vector3 v1;

	public Vector3 v2;

	public Edge(Vector3 v1, Vector3 v2)
	{
		if (v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z <= v2.z))))
		{
			this.v1 = v1;
			this.v2 = v2;
		}
		else
		{
			this.v1 = v2;
			this.v2 = v1;
		}
	}
}
