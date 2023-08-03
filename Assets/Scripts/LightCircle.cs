using System.Collections.Generic;
using UnityEngine;

public class LightCircle : MonoBehaviour
{
	private Mesh mesh;

	public LayerMask staticMask;

	public float maxRange = 6f;

	private float CornerAngle = 0.1f;

	private float range;

	public bool AfterFill = true;

	private Collider[] colliders;

	private SortedListNA<float, Vector2> points;

	private Vector3[] vertices;

	private Vector2[] uvs;

	private int[] triangles;

	private int raysCasted;

	private const int MAX_COLLIDERS = 50;

	private const int TOTAL_POINTS = 300;

	private const int TOTAL_TRIANGLES = 900;

	private int UpdateRate = 10;

	private int updateFrame;

	public bool DebugLog;

	private void Start()
	{
		mesh = new Mesh();
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;
		colliders = new Collider[50];
		points = new SortedListNA<float, Vector2>(300);
		vertices = new Vector3[301];
		uvs = new Vector2[301];
		triangles = new int[900];
		updateFrame = UnityEngine.Random.Range(0, UpdateRate);
	}

	private void GenerateDynamicPoints()
	{
		points.Clear();
		int num = 0;
		if ((int)staticMask != 0)
		{
			num = Physics.OverlapSphereNonAlloc(base.transform.position, range - 0.75f, colliders, staticMask);
		}
		if (num == 0)
		{
			int num2 = 16;
			for (int i = 0; i < num2; i++)
			{
				AddPoint(Quaternion.Euler(0f, 0f, (float)i * (365f / (float)num2)) * Vector2.up);
			}
			return;
		}
		int num3 = 8;
		for (int j = 0; j < num3; j++)
		{
			CastRay(base.transform.position, Quaternion.Euler(0f, 0f, (float)j * (365f / (float)num3)) * Vector2.up, points, staticMask);
		}
		if (num == 50)
		{
			UnityEngine.Debug.LogWarning("Physics.OverlapSphereNonAlloc filled colliders array");
		}
		if (DebugLog)
		{
			UnityEngine.Debug.Log("Colliders " + num, this);
		}
		for (int k = 0; k < num; k++)
		{
			Transform transform = colliders[k].transform;
			AddPoints(transform, points, staticMask);
		}
		points = RemoveAligned(points);
	}

	private void AddPoint(Vector2 direction)
	{
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		Vector2 a = new Vector2(x, position2.y);
		Vector2 vector = a + direction.normalized * range;
		float key = Angle(vector, base.transform.position);
		points.Add(key, vector);
	}

	private SortedListNA<float, Vector2> RemoveAligned(SortedListNA<float, Vector2> points)
	{
		if (points.Count > 2)
		{
			int num = 2;
			while (num < points.Count)
			{
				if (Aligned(points.ValueAt(num - 2), points.ValueAt(num - 1), points.ValueAt(num)))
				{
					points.RemoveAt(num - 1);
				}
				else
				{
					num++;
				}
			}
		}
		return points;
	}

	private void LateUpdate()
	{
		if (updateFrame == Time.frameCount % UpdateRate)
		{
			raysCasted = 0;
			range = maxRange;
			GenerateDynamicPoints();
			GenerateMesh();
			if (DebugLog)
			{
				UnityEngine.Debug.Log("rays " + raysCasted);
			}
		}
	}

	private void GenerateMesh()
	{
		vertices[0] = Vector3.zero;
		uvs[0] = Vector2.zero;
		for (int i = 0; i < points.Count; i++)
		{
			Vector3 a = points.ValueAt(i);
			vertices[1 + i] = a - base.transform.position;
			vertices[1 + i].z = 0f;
			uvs[1 + i] = new Vector2(vertices[1 + i].x, vertices[1 + i].y);
			if (i > 0)
			{
				triangles[(i - 1) * 3] = 0;
				triangles[(i - 1) * 3 + 1] = i;
				triangles[(i - 1) * 3 + 2] = 1 + i;
			}
		}
		int count = points.Count;
		triangles[(count - 1) * 3] = 0;
		triangles[(count - 1) * 3 + 1] = count;
		triangles[(count - 1) * 3 + 2] = 1;
		for (int j = count * 3; j < triangles.Length; j++)
		{
			triangles[j] = 0;
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
	}

	private bool Aligned(Vector2 a, Vector2 b, Vector2 c)
	{
		return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(b.x, c.x)) || (Mathf.Approximately(a.y, b.y) && Mathf.Approximately(b.y, c.y));
	}

	private float Angle(Vector2 a, Vector2 b)
	{
		Vector2 vector = b - a;
		return 0f - Mathf.Atan2(vector.y, vector.x);
	}

	private void CastRays(Vector2 direction, SortedListNA<float, Vector2> points, LayerMask mask)
	{
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		Vector2 origin = new Vector2(x, position2.y);
		Vector2 direction2 = Quaternion.Euler(0f, 0f, CornerAngle) * direction;
		CastRay(origin, direction2, points, mask);
		Vector2 direction3 = Quaternion.Euler(0f, 0f, 0f - CornerAngle) * direction;
		CastRay(origin, direction3, points, mask);
	}

	private void CastRay(Vector2 origin, Vector2 direction, SortedListNA<float, Vector2> points, LayerMask mask)
	{
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, direction, out hitInfo, range, mask);
		raysCasted++;
		Vector2 vector = (!flag) ? (origin + direction.normalized * range) : ((Vector2)hitInfo.point);
		float key = Angle(vector, base.transform.position);
		points.Add(key, vector);
	}

	private void AddPoints(Transform t, SortedListNA<float, Vector2> points, LayerMask mask)
	{
		Bounds bounds = t.GetComponent<Collider>().bounds;
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		Vector2 b = new Vector2(x, position2.y);
		Vector2 a = bounds.center;
		Vector3 extents = bounds.extents;
		float x2 = 0f - extents.x;
		Vector3 extents2 = bounds.extents;
		Vector2 vector = a + new Vector2(x2, extents2.y);
		Vector2 a2 = bounds.center;
		Vector3 extents3 = bounds.extents;
		float x3 = 0f - extents3.x;
		Vector3 extents4 = bounds.extents;
		Vector2 vector2 = a2 + new Vector2(x3, 0f - extents4.y);
		Vector2 a3 = bounds.center;
		Vector3 extents5 = bounds.extents;
		float x4 = extents5.x;
		Vector3 extents6 = bounds.extents;
		Vector2 vector3 = a3 + new Vector2(x4, extents6.y);
		Vector2 a4 = bounds.center;
		Vector3 extents7 = bounds.extents;
		float x5 = extents7.x;
		Vector3 extents8 = bounds.extents;
		Vector2 vector4 = a4 + new Vector2(x5, 0f - extents8.y);
		Vector2 a5 = vector;
		Vector2 a6 = vector3;
		if (b.x < vector.x)
		{
			if (b.y < vector2.y)
			{
				a5 = vector;
				a6 = vector4;
			}
			else if (b.y > vector.y)
			{
				a5 = vector3;
				a6 = vector2;
			}
			else
			{
				a5 = vector;
				a6 = vector2;
			}
		}
		else if (b.x > vector3.x)
		{
			if (b.y < vector4.y)
			{
				a5 = vector3;
				a6 = vector2;
			}
			else if (b.y > vector3.y)
			{
				a5 = vector;
				a6 = vector4;
			}
			else
			{
				a5 = vector3;
				a6 = vector4;
			}
		}
		else if (b.y < vector4.y)
		{
			a5 = vector4;
			a6 = vector2;
		}
		else if (b.y > vector3.y)
		{
			a5 = vector;
			a6 = vector3;
		}
		CastRays(a5 - b, points, mask);
		CastRays(a6 - b, points, mask);
	}
}
