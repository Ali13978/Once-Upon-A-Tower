using System.Collections.Generic;
using UnityEngine;

public class Thwomp : WorldObject
{
	public ThwompCube Source;

	public int MaxLength;

	public List<GameObject> PossiblePieces;

	private List<GameObject> pieces;

	public GameObject Separator;

	private Vector3 direction;

	private Coord startCoord;

	private bool retreating;

	public bool Moving => Projectile.Moving;

	public override void Initialize()
	{
		base.Initialize();
		pieces = new List<GameObject>(MaxLength);
		for (int i = 0; i < MaxLength; i++)
		{
			int index = Random.Range(0, PossiblePieces.Count - 1);
			pieces.Add(Object.Instantiate(PossiblePieces[index], base.transform, worldPositionStays: true));
			GameObject gameObject = pieces[i];
			gameObject.SetActive(value: false);
			GameObject gameObject2 = Object.Instantiate(Separator, gameObject.transform, worldPositionStays: true);
			gameObject2.SetActive(value: true);
			gameObject2.transform.position = gameObject.transform.position + new Vector3(0f, -0.5f, 0f);
			if (i == 0)
			{
				WorldObject component = gameObject.GetComponent<WorldObject>();
				UnityEngine.Object.Destroy(component);
				BoxCollider component2 = gameObject.GetComponent<BoxCollider>();
				UnityEngine.Object.Destroy(component2);
			}
		}
		UpdateCubes();
	}

	public void Fire(Vector3 direction, float AccelerationMagnitude)
	{
		this.direction = direction;
		base.gameObject.SetActive(value: true);
		Coord = Source.Coord;
		startCoord = Coord;
		retreating = false;
		Projectile.Move(AccelerationMagnitude * direction, Vector3.zero);
		UpdateCubes();
	}

	public void Retreat(Vector3 direction, float AccelerationMagnitude)
	{
		this.direction = direction;
		Projectile.Move(AccelerationMagnitude * direction, Vector3.zero);
		retreating = true;
		UpdateCubes();
	}

	private int getNumberOfCubes()
	{
		int a = Mathf.CeilToInt(Vector3.Distance(Source.transform.position, base.transform.position));
		return Mathf.Max(a, 1);
	}

	public override void Disarm()
	{
		base.Disarm();
		Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(startCoord);
		for (int num = pieces.Count - 1; num > 0; num--)
		{
			GameObject gameObject = pieces[num];
			if (gameObject.activeSelf)
			{
				WorldObject component = pieces[num].GetComponent<WorldObject>();
				if (component.Coord != startCoord)
				{
					component.RemoveCoord();
				}
				gameObject.SetActive(value: false);
			}
		}
		base.transform.position = positionFromCoord;
		if (Coord != startCoord)
		{
			RemoveCoord();
		}
		Projectile.Stop();
	}

	private void UpdateCubes()
	{
		int numberOfCubes = getNumberOfCubes();
		for (int i = 0; i < pieces.Count; i++)
		{
			GameObject gameObject = pieces[i];
			bool flag = i < numberOfCubes;
			if (!gameObject.activeSelf && flag)
			{
				if (i == 0)
				{
					gameObject.transform.position = Source.transform.position;
				}
				else
				{
					gameObject.transform.position = pieces[i - 1].transform.position - direction.normalized;
					WorldObject component = gameObject.GetComponent<WorldObject>();
					component.Coord = Source.Coord;
				}
			}
			gameObject.SetActive(flag);
		}
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (!CanMoveTo(wo.Coord, wo))
		{
			return false;
		}
		for (int i = 1; i < pieces.Count; i++)
		{
			WorldObject component = pieces[i].GetComponent<WorldObject>();
			if (wo == component)
			{
				return true;
			}
		}
		if (wo == this)
		{
			return true;
		}
		if (wo == Source)
		{
			return true;
		}
		if (wo != null && (wo.Coord == (Source.Coord + direction).Normalize() || wo is Digger || wo is Enemy || wo is Coin || wo is Fireball || wo is Chicken))
		{
			wo.OnHit(direction, this);
			if (wo.Broken)
			{
				return true;
			}
		}
		return false;
	}

	public override void MoveCoord(Coord newCoord)
	{
		UpdateCubes();
		if (retreating)
		{
			for (int num = pieces.Count - 1; num > 0; num--)
			{
				GameObject gameObject = pieces[num];
				if (gameObject.activeSelf)
				{
					WorldObject component = pieces[num].GetComponent<WorldObject>();
					Coord coord = (component.Coord + direction.normalized).Normalize();
					if (coord == startCoord)
					{
						component.RemoveCoord();
					}
					else
					{
						SingletonMonoBehaviour<World>.Instance.Move(component, coord);
						component.Coord = coord;
					}
				}
			}
		}
		if (startCoord == newCoord)
		{
			RemoveCoord();
		}
		else
		{
			SingletonMonoBehaviour<World>.Instance.Move(this, newCoord);
		}
		Coord = newCoord;
		if (retreating)
		{
			return;
		}
		for (int i = 1; i < pieces.Count; i++)
		{
			GameObject gameObject2 = pieces[i];
			if (gameObject2.activeSelf)
			{
				WorldObject component2 = pieces[i].GetComponent<WorldObject>();
				Coord coord2 = (component2.Coord + direction.normalized).Normalize();
				if (startCoord != coord2)
				{
					SingletonMonoBehaviour<World>.Instance.Move(component2, coord2);
				}
				component2.Coord = coord2;
			}
		}
	}

	public override bool CanMoveTo(Coord newCoord, WorldObject _)
	{
		if (!retreating && getNumberOfCubes() == MaxLength)
		{
			return false;
		}
		if (retreating && startCoord == Coord)
		{
			return false;
		}
		return true;
	}

	public override bool BreakableBy(WorldObject hitter)
	{
		return hitter is Digger;
	}

	public override void OnHit(Vector3 hitDirection, WorldObject hitter, WorldObject medium = null)
	{
		if (hitter is Digger && hitDirection.normalized == -direction.normalized && !retreating)
		{
			Source.Retreat();
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.HitBackThwomp);
		}
	}
}
