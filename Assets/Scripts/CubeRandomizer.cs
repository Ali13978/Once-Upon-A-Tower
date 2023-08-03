using UnityEngine;

public class CubeRandomizer : MonoBehaviour
{
	public virtual void Randomize(TileMap section)
	{
	}

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Vector3 position = base.transform.position;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, Quaternion.FromToRotation(new Vector3(1f, 1f, 1f), Vector3.forward), Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);
	}
}
