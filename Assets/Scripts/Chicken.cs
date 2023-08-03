using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : WorldObject
{
	public GameObject Mesh;

	public float JumpVelocity = 5f;

	public AudioSource FallingLoop;

	private Coord lastMoveCoord;

	private float lastMoveTime;

	private List<Coord> queue = new List<Coord>();

	private bool active;

	private Digger digger => SingletonMonoBehaviour<Game>.Instance.Digger;

	private Coord LastCoord => queue[queue.Count - 1];

	private void OnDisable()
	{
		UnityEngine.Debug.Log("Chicken disable");
	}

	public override void Initialize()
	{
		Coord[] array = new Coord[3]
		{
			digger.Coord + Coord.Left,
			digger.Coord + Coord.Right,
			digger.Coord + Coord.Up
		};
		for (int i = 0; i < array.Length; i++)
		{
			Coord coord = array[i].Normalize();
			if (SingletonMonoBehaviour<World>.Instance.GetTile(coord) == null)
			{
				base.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
				MoveCoord(coord);
				queue.Add(digger.Coord);
				active = true;
				break;
			}
		}
		base.Initialize();
		StartCoroutine(IdleAnimations());
	}

	private IEnumerator IdleAnimations()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(2f, 10f));
			Animator.SetTrigger("idle" + Random.Range(1, 3));
		}
	}

	private void FixedUpdate()
	{
		if (!active)
		{
			return;
		}
		if (digger.Coord != LastCoord)
		{
			queue.Add(digger.Coord);
		}
		if (Coord.x != lastMoveCoord.x || Time.time - lastMoveTime > 0.5f)
		{
			Walker.Stop();
		}
		TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(Coord.Section);
		if (!Walker.Moving && queue.Count > 1)
		{
			Coord coord = queue[0];
			queue.RemoveAt(0);
			Vector3 vector = Vector3.zero;
			if (coord.x > Coord.x)
			{
				vector = Vector3.right;
			}
			if (coord.x < Coord.x)
			{
				vector = Vector3.left;
			}
			if (section != null && section.IsEnd && Coord == digger.Coord && vector == Vector3.right)
			{
				Vector3 normalized = new Vector3(digger.Coord.x - coord.x, 0f, 0f).normalized;
				Coord coord2 = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + normalized);
				WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
				if (CanMoveTo(coord2, tile))
				{
					vector = normalized;
				}
			}
			if (vector != Vector3.zero)
			{
				MoveSide(vector);
			}
		}
		if (section == null)
		{
			Breakable.Break(Vector3.zero);
		}
		if (SingletonMonoBehaviour<Game>.Instance.Ready && Walker.IsGrounded)
		{
			Coord coord3 = SingletonMonoBehaviour<Game>.Instance.Digger.Coord - Coord;
			if (coord3.y > 20)
			{
				Breakable.Break(Vector3.zero);
			}
		}
		if ((bool)section && section.IsLoadingSection)
		{
			Vector3 position = digger.transform.position;
			float y = position.y;
			Vector3 position2 = base.transform.position;
			if (y < position2.y && !base.OnScreen)
			{
				Camera camera = SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
				Camera camera2 = camera;
				Vector3 position3 = camera.transform.position;
				Vector3 vector2 = camera2.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f - position3.z));
				Transform transform = base.transform;
				Vector3 position4 = base.transform.position;
				float x = position4.x;
				float y2 = vector2.y + 0.5f;
				Vector3 position5 = base.transform.position;
				transform.position = new Vector3(x, y2, position5.z);
				MoveCoord(SingletonMonoBehaviour<World>.Instance.GetCoordFromPosition(base.transform.position));
			}
		}
		if (FallingLoop != null)
		{
			if (!Walker.IsGrounded)
			{
				if (!FallingLoop.isPlaying)
				{
					FallingLoop.Play();
				}
			}
			else if (FallingLoop.isPlaying)
			{
				FallingLoop.Stop();
			}
		}
		if (Walker.IsGrounded && digger.Coord == (Coord + Coord.Down).Normalize() && SingletonMonoBehaviour<World>.Instance.GetTile(Coord) == this)
		{
			RemoveCoord();
		}
	}

	public override void OnSteppedOn(WorldObject stepper)
	{
		if (stepper == digger)
		{
			RemoveCoord();
		}
		else
		{
			base.OnSteppedOn(stepper);
		}
	}

	private void MoveSide(Vector3 direction)
	{
		Walker.Move(direction);
		lastMoveCoord = Coord;
		lastMoveTime = Time.time;
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (hitter is Digger)
		{
			if (direction == Vector3.up && Walker.IsGrounded)
			{
				Walker.Jump(JumpVelocity);
			}
			return;
		}
		if ((bool)FallingLoop)
		{
			FallingLoop.Stop();
		}
		base.OnHit(direction, hitter, medium);
		Breakable.EnableRagdoll(Mesh, direction, null);
		SaveGame.Instance.SetItemActive("Chicken", value: false);
	}
}
