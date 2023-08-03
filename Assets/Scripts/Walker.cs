using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class Walker : Movable
{
	[HideInInspector]
	public bool IsGrounded;

	public float MinFallVelocity = -4f;

	public float MaxFallVelocity = -40f;

	public bool StopBeforeFall;

	public bool Decelerate = true;

	public bool AirControl;

	public float AirControlFallVelocity = -8f;

	public Animator Animator;

	private bool stoppedBeforeFall;

	private Vector3 GroundedPosition(WorldObject ground)
	{
		Bounds bounds = ground.Bounds;
		Bounds bounds2 = Wo.Bounds;
		Vector3 center = bounds.center;
		Vector3 extents = bounds.extents;
		float y = extents.y;
		Vector3 extents2 = bounds2.extents;
		float num = y + extents2.y;
		Vector3 center2 = bounds2.center;
		float num2 = num - center2.y;
		Vector3 position = base.transform.position;
		return center + new Vector3(0f, num2 + position.y, 0f);
	}

	public override void Initialize()
	{
		if (initialized)
		{
			return;
		}
		base.Initialize();
		for (int i = 0; i < 20; i++)
		{
			Coord coord = (Wo.Coord + Coord.Down).Normalize();
			if (!SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord))
			{
				break;
			}
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			if (tile != null)
			{
				if (tile is Enemy)
				{
					tile.RemoveCoord();
					UnityEngine.Object.DestroyImmediate(tile.gameObject);
				}
				else
				{
					if (!(tile is Coin))
					{
						break;
					}
					Wo.Coins += tile.Coins;
					tile.RemoveCoord();
					UnityEngine.Object.DestroyImmediate(tile.gameObject);
				}
			}
			Wo.MoveCoord(coord);
			base.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
		}
	}

	public void InstantGround()
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile((Wo.Coord + Coord.Down).Normalize());
		if ((bool)tile)
		{
			base.transform.position = GroundedPosition(tile);
			IsGrounded = true;
		}
	}

	private void FixedUpdate()
	{
		if (!initialized || !SingletonMonoBehaviour<World>.Instance.IsCoordValid(Wo.Coord))
		{
			return;
		}
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + Coord.Down));
		if (!IsGrounded || (Velocity.x == 0f && tile == null))
		{
			IsGrounded = false;
			trajectory.Cancel(this);
			Velocity.x = 0f;
			Velocity += Physics.gravity * Time.fixedDeltaTime;
			Velocity.y = Mathf.Clamp(Velocity.y, MaxFallVelocity, MinFallVelocity);
			if (AirControl && Velocity.y < AirControlFallVelocity)
			{
				Velocity.y = AirControlFallVelocity;
			}
			Vector3 position = base.transform.position + Velocity * Time.fixedDeltaTime;
			if (tile != null)
			{
				Vector3 vector = GroundedPosition(tile);
				if (position.y <= vector.y)
				{
					tile.OnSteppedOn(Wo);
					if (!tile.Broken)
					{
						position.y = vector.y;
						IsGrounded = true;
						Wo.OnGrounded();
					}
				}
			}
			Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Wo.Coord);
			if (AirControl && positionFromCoord != Vector3.zero && positionFromCoord.x != position.x)
			{
				position.x = positionFromCoord.x + Mathf.Sign(position.x - positionFromCoord.x) * Tween.CubicEaseInOut(Mathf.Clamp01(position.y - positionFromCoord.y), 0f, 1f, 1f);
			}
			base.transform.position = position;
			if (!IsGrounded)
			{
				Vector3 position2 = base.transform.position;
				float y = position2.y;
				Vector3 positionFromCoord2 = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Wo.Coord);
				if (y < positionFromCoord2.y && tile == null)
				{
					Coord coord = Wo.Coord + Coord.Down;
					WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord + Direction));
					if (AirControl && (tile2 == null || tile2.BreakableBy(Wo)))
					{
						if (tile2 != null)
						{
							tile2.OnHit(Direction, Wo);
						}
						if (tile2 == null || tile2.Broken)
						{
							coord += Direction;
							if (Direction == Vector3.left)
							{
								Animator.SetTrigger("fallLeft");
							}
							if (Direction == Vector3.right)
							{
								Animator.SetTrigger("fallRight");
							}
						}
					}
					coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord);
					bool flag = true;
					if (SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord) && SingletonMonoBehaviour<World>.Instance.Sections[coord.Section].IsLoadingSection && !(Wo is Chicken) && !Wo.OnScreen && Wo.Breakable != null)
					{
						flag = false;
						Wo.Breakable.Break(Vector3.zero);
					}
					if (flag && SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord))
					{
						Wo.MoveCoord(coord);
					}
				}
			}
			if (!IsGrounded && Velocity.y > 0f)
			{
				Coord coordFromPosition = SingletonMonoBehaviour<World>.Instance.GetCoordFromPosition(base.transform.position);
				WorldObject tile3 = SingletonMonoBehaviour<World>.Instance.GetTile(coordFromPosition);
				if (tile3 == null && SingletonMonoBehaviour<World>.Instance.IsCoordValid(coordFromPosition))
				{
					Wo.MoveCoord(coordFromPosition);
				}
			}
		}
		else
		{
			Velocity.y = 0f;
			if (tile != null)
			{
				Vector3 vector2 = GroundedPosition(tile);
				Vector3 position3 = base.transform.position;
				position3.y = vector2.y;
				base.transform.position = position3;
			}
			Vector3 positionFromCoord3 = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(Wo.Coord);
			Coord coord2 = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + Direction);
			WorldObject tile4 = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
			Vector3 positionFromCoord4 = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord2);
			WorldObject tile5 = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord2 + Coord.Down));
			bool flag2 = Wo.CanMoveTo(coord2, tile4);
			if (tile is CrackedCube && Wo is Enemy)
			{
				flag2 = false;
			}
			if (flag2 && StopBeforeFall && (tile5 == null || !tile5.Hermetic))
			{
				flag2 = false;
				if (Direction != Vector3.zero && tile != null)
				{
					stoppedBeforeFall = true;
				}
			}
			if (Direction == Vector3.zero || !flag2 || tile == null)
			{
				if (Direction != Vector3.zero && !flag2)
				{
					moveTo = tile4;
					moveToDirection = Direction;
				}
				Direction = Vector3.zero;
				if (!trajectory.Active)
				{
					Vector3 position4 = base.transform.position;
					if (Mathf.Abs(position4.x - positionFromCoord3.x) > 0.01f)
					{
						float x = Velocity.x;
						Vector3 position5 = base.transform.position;
						if (x * (position5.x - positionFromCoord3.x) > 0f)
						{
							Velocity.x = 0f;
						}
						trajectory.Setup(this, positionFromCoord3);
					}
				}
				if (trajectory.Apply(this))
				{
					if (moveTo != null)
					{
						Wo.OnMoveTo(moveToDirection, moveTo);
						moveTo = null;
					}
					if (tile != null)
					{
						tile.OnSteppedOn(Wo);
					}
					if (stoppedBeforeFall)
					{
						Wo.StoppedBeforeFall();
						stoppedBeforeFall = false;
					}
					Vector3 position6 = base.transform.position;
					position6.x = positionFromCoord3.x;
					base.transform.position = position6;
					Velocity.x = 0f;
					trajectory.Cancel(this);
				}
			}
			else
			{
				trajectory.Cancel(this);
				Velocity += Direction * Acceleration * Time.fixedDeltaTime;
			}
			Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(MoveSpeed, Mathf.Abs(Velocity.x));
			base.transform.position += Velocity * Time.fixedDeltaTime;
			Vector3 position7 = base.transform.position;
			float num = Mathf.Abs(position7.x - positionFromCoord4.x);
			Vector3 position8 = base.transform.position;
			if (num < Mathf.Abs(position8.x - positionFromCoord3.x) && Direction != Vector3.zero && flag2)
			{
				if (tile != null)
				{
					tile.OnSteppedOn(Wo);
					if (tile.Broken)
					{
						Vector3 position9 = base.transform.position;
						position9.x = positionFromCoord3.x;
						base.transform.position = position9;
						Velocity.x = 0f;
					}
					else if (!Wo.Broken)
					{
						Wo.MoveCoord(coord2);
						if (tile5 != null)
						{
							tile5.OnSteppedOn(Wo);
						}
					}
				}
				else
				{
					Wo.MoveCoord(coord2);
				}
			}
		}
		if (Animator != null && Animator.isActiveAndEnabled)
		{
			Animator.SetFloat("velocity", Velocity.x);
			if (Velocity.y != 0f)
			{
				Animator.SetFloat("fallVelocity", Velocity.y);
			}
			Animator.SetBool("moving", Velocity.x != 0f);
			Animator.SetBool("falling", !IsGrounded);
		}
	}

	public override void Move(Vector3 direction)
	{
		if (Wo == null)
		{
			return;
		}
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + Coord.Down));
		if (!base.Moving || tile == null || !tile.Slippery)
		{
			if (Animator != null && Animator.isActiveAndEnabled && !(Wo is EnemyFollow))
			{
				Animator.SetTrigger("move");
			}
			Direction = direction;
		}
	}

	public void Stop()
	{
		if (!(Wo == null))
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Wo.Coord + Coord.Down));
			if (tile == null || !tile.Slippery)
			{
				Direction = Vector3.zero;
				moveTo = null;
				stoppedBeforeFall = false;
			}
		}
	}

	public void Jump(float velocity)
	{
		IsGrounded = false;
		Velocity.y = velocity;
	}
}
