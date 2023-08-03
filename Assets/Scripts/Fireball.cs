using System.Collections;
using UnityEngine;

public class Fireball : WorldObject
{
	public float Timeout = 10f;

	private WorldObject source;

	public ParticleSystem Explosion;

	public AudioSource HitAudio;

	public AudioSource ParryAudio;

	private IEnumerator destroyCoroutine;

	public void Fire(WorldObject source, Vector3 direction, float speed)
	{
		base.gameObject.SetActive(value: true);
		base.transform.localRotation = Quaternion.LookRotation(Vector3.forward, direction);
		this.source = source;
		Coord = source.Coord;
		Projectile.Move(Vector3.zero, direction * speed);
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		HitOther(direction, wo);
		return false;
	}

	private void HitOther(Vector3 direction, WorldObject wo)
	{
		wo.OnHit(direction, this);
		if (wo.Broken || wo is PushCube)
		{
			wo.Breakable.LightFire();
		}
		if (destroyCoroutine == null)
		{
			source.StartCoroutine(destroyCoroutine = DestroyCoroutine());
		}
	}

	public override void OnSteppedOn(WorldObject stepper)
	{
		if (destroyCoroutine == null)
		{
			source.StartCoroutine(destroyCoroutine = DestroyCoroutine());
		}
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (hitter is Digger)
		{
			if (destroyCoroutine != null)
			{
				source.StopCoroutine(destroyCoroutine);
				destroyCoroutine = null;
				Breakable.Unbreak();
			}
			RemoveCoord();
			base.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(hitter.Coord + direction);
			Fire(hitter, direction, Projectile.MoveSpeed);
			if ((bool)ParryAudio)
			{
				ParryAudio.Play();
			}
			if (Vector3.left == direction || Vector3.right == direction)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.HitBackFireball);
			}
		}
		else
		{
			base.OnHit(direction, hitter, medium);
		}
	}

	private IEnumerator DestroyCoroutine()
	{
		if ((bool)Breakable)
		{
			Breakable.Break(Vector3.zero);
		}
		if ((bool)HitAudio)
		{
			HitAudio.Play();
		}
		yield return new WaitForSeconds(0.7f);
		if (!(this == null) && !(base.gameObject == null))
		{
			destroyCoroutine = null;
			if (source != null && source is ArrowSource && Breakable != null)
			{
				base.gameObject.SetActive(value: false);
				Breakable.Unbreak();
				((ArrowSource)source).Pool(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public override void Disarm()
	{
		base.Disarm();
		if (destroyCoroutine == null)
		{
			source.StartCoroutine(destroyCoroutine = DestroyCoroutine());
		}
	}

	public override void RemoveCoord()
	{
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(Coord);
		if (!(tile != this) || !(tile is ArrowSource))
		{
			base.RemoveCoord();
		}
	}
}
