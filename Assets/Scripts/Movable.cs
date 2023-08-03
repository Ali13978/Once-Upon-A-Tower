using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public abstract class Movable : MonoBehaviour
{
	public Vector3 Velocity = Vector3.zero;

	public float MoveSpeed = 5f;

	public float Acceleration = 20f;

	public bool DebugLog;

	[HideInInspector]
	public Vector3 Direction = Vector3.zero;

	public Trajectory trajectory = default(Trajectory);

	protected WorldObject Wo;

	[HideInInspector]
	public WorldObject moveTo;

	protected Vector3 moveToDirection;

	protected bool initialized;

	public bool Moving => Velocity != Vector3.zero;

	public virtual void Initialize()
	{
		Wo = GetComponent<WorldObject>();
		initialized = true;
	}

	public abstract void Move(Vector3 direction);
}
