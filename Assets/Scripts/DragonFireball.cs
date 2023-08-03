using UnityEngine;

public class DragonFireball : WorldObject
{
	public ParticleSystem Particles;

	public float TravelTime = 2f;

	public AudioSource HitAudio;

	public AudioSource LaunchAudio;

	private bool moving;

	private float startTime;

	private Vector3 start;

	private Vector3 target;

	private void FixedUpdate()
	{
		if (!moving)
		{
			return;
		}
		float t = (Time.fixedTime - startTime) / TravelTime;
		float t2 = Tween.QuintEaseIn(t, 0f, 1f, 1f);
		base.transform.position = new Vector3(Mathf.Lerp(start.x, target.x, t), Mathf.Lerp(start.y, target.y, t2));
		Coord coordFromPosition = SingletonMonoBehaviour<World>.Instance.GetCoordFromPosition(base.transform.position);
		if (coordFromPosition != Coord.None)
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coordFromPosition);
			if (tile != null && tile != this)
			{
				tile.OnHit(Vector3.down, this);
				HitAudio.Play();
			}
			if (tile != null && tile != this && !tile.Broken)
			{
				Stop();
			}
			if (tile == null && coordFromPosition.x < SingletonMonoBehaviour<World>.Instance.Sections[coordFromPosition.Section].Columns && coordFromPosition.x >= 0)
			{
				MoveCoord(coordFromPosition);
			}
		}
	}

	public void Fire(Vector3 position, Vector3 target)
	{
		if (!base.Broken)
		{
			base.transform.position = position;
			start = position;
			this.target = target;
			startTime = Time.fixedTime;
			Particles.Play();
			moving = true;
			LaunchAudio.Play();
		}
	}

	public void Stop()
	{
		Particles.Stop();
		moving = false;
		Breakable.Break(Vector3.zero);
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if (direction == Vector3.up)
		{
			Stop();
		}
		else
		{
			Fire(hitter.transform.position + direction * 2f, hitter.transform.position + new Vector3(direction.x * 10f, 2f, 0f));
		}
	}
}
