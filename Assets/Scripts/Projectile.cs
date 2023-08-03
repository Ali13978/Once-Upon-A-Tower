using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float MaxVelocityMagnitude = 20f;

	private Vector3 acceleration = Vector3.zero;

	private Vector3 initialVelocity = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	private WorldObject Wo;

	public float MoveSpeed => initialVelocity.magnitude;

	public bool Moving => velocity != Vector3.zero;

	private void Start()
	{
		Wo = GetComponent<WorldObject>();
	}

	private void FixedUpdate()
	{
		if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(Wo.Coord))
		{
			return;
		}
		velocity += acceleration * Time.fixedDeltaTime;
		if (velocity.magnitude > MaxVelocityMagnitude)
		{
			velocity = MaxVelocityMagnitude * velocity.normalized;
		}
		Vector3 vector = base.transform.position + velocity * Time.fixedDeltaTime;
		Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Wo.Coord);
		Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + velocity.normalized);
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
		if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord))
		{
			Stop();
		}
		else if (Vector3.Dot(vector - positionFromCoord, velocity) > 0f)
		{
			bool flag = !(tile != null) || Wo.OnMoveTo(velocity.normalized, tile);
			bool flag2 = false;
			if (flag && Wo.CanMoveTo(coord, tile))
			{
				base.transform.position = vector;
				Wo.MoveCoord(coord);
				flag2 = true;
			}
			if (!flag2)
			{
				base.transform.position = positionFromCoord;
				Stop();
			}
		}
		else
		{
			base.transform.position = vector;
		}
	}

	public void Move(Vector3 acceleration, Vector3 initialVelocity)
	{
		this.acceleration = acceleration;
		this.initialVelocity = initialVelocity;
		velocity = initialVelocity;
	}

	public void Stop()
	{
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}
}
