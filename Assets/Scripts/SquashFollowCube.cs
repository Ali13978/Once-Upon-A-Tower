using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquashFollowCube : WorldObject
{
	public float AccelerationMagnitude = 25f;

	public float MoveDelay = 1f;

	public float MaxSpeed = 5f;

	private Vector3 velocity;

	private Vector3 acceleration;

	private Vector3 targetPosition;

	private List<Vector3> stuckDirections;

	private bool awake;

	private bool shaking;

	private bool moving;

	private bool smashing;

	private bool pushing;

	private void Start()
	{
		stuckDirections = new List<Vector3>();
	}

	private void FixedUpdate()
	{
		if (base.Broken)
		{
			return;
		}
		if (!awake && ShouldBeAwake())
		{
			awake = true;
			shaking = true;
			StartCoroutine(ShakeCoroutine());
		}
		if (awake && !shaking && !moving && ShouldBeMoving(out Vector3 direction))
		{
			moving = true;
			if (velocity == Vector3.zero || direction != velocity.normalized)
			{
				velocity = direction;
				acceleration = direction * AccelerationMagnitude;
			}
		}
		if (!moving)
		{
			return;
		}
		velocity += acceleration * Time.fixedDeltaTime;
		if (velocity.magnitude > MaxSpeed)
		{
			velocity = velocity.normalized * MaxSpeed;
		}
		float num = Vector3.Distance(base.transform.position, targetPosition);
		bool flag = Vector3.Dot(targetPosition - base.transform.position, velocity) > 0f;
		if (num > 0f && flag)
		{
			if (num < 0.01f)
			{
				base.transform.position = targetPosition;
				moving = false;
			}
			else
			{
				base.transform.position += velocity * Time.fixedDeltaTime;
			}
		}
		else
		{
			moving = false;
		}
	}

	private bool ShouldBeAwake()
	{
		IEnumerable<int> enumerable = Enumerable.Range(-2, 5);
		foreach (int item in enumerable)
		{
			foreach (int item2 in enumerable)
			{
				if (item == 0 && (item2 == 0 || item2 == -1))
				{
					break;
				}
				Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + new Coord(item, item2));
				WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
				if (tile is Digger)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool ShouldBeMoving(out Vector3 direction)
	{
		Vector3 targetDirection = SingletonMonoBehaviour<Game>.Instance.Digger.transform.position - base.transform.position;
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.up);
		list.Add(Vector3.right);
		list.Add(Vector3.down);
		list.Add(Vector3.left);
		List<Vector3> list2 = list;
		list2.RemoveAll((Vector3 d) => stuckDirections.Contains(d));
		direction = (from d in list2
			orderby Cosine(d, targetDirection)
			select d).Last();
		Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction);
		if (coord != Coord)
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			if (tile == null)
			{
				MoveCoord(coord);
				stuckDirections.Clear();
				targetPosition = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Coord);
				return true;
			}
			if (tile is Digger || tile is Enemy)
			{
				if (direction == Vector3.up)
				{
					if (tile is Digger)
					{
						awake = false;
					}
					return false;
				}
				Coord coord2 = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + direction);
				WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
				if (tile2 != null)
				{
					if (!smashing)
					{
						smashing = true;
					}
					else
					{
						tile.OnHit(direction, this);
						if (tile is Digger)
						{
							awake = false;
						}
						if (tile.Broken)
						{
							tile.RemoveCoord();
							MoveCoord(coord);
							stuckDirections.Clear();
						}
					}
				}
				else if (!pushing)
				{
					pushing = true;
				}
				else
				{
					tile.MoveCoord(coord2);
					((Digger)tile).LookDirection = direction;
					MoveCoord(coord);
					stuckDirections.Clear();
				}
				if (smashing || pushing)
				{
					Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Coord);
					Vector3 positionFromCoord2 = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
					targetPosition = positionFromCoord + (positionFromCoord2 - positionFromCoord) / 10f;
				}
				else
				{
					targetPosition = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Coord);
				}
				return true;
			}
			stuckDirections.Add(direction);
		}
		return false;
	}

	private IEnumerator ShakeCoroutine()
	{
		yield return new WaitForSeconds(MoveDelay);
		shaking = false;
	}

	private float Cosine(Vector3 v1, Vector3 v2)
	{
		return Vector3.Dot(v1, v2) / (v1.magnitude * v2.magnitude);
	}
}
