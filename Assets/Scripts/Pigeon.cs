using System;
using UnityEngine;

public class Pigeon : WorldObject
{
	public AudioSource FlyAway;

	private bool awake;

	private void OnEnable()
	{
		Game instance = SingletonMonoBehaviour<Game>.Instance;
		instance.OnNoise = (Action<Noise>)Delegate.Combine(instance.OnNoise, new Action<Noise>(OnNoise));
	}

	private void OnDisable()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance())
		{
			Game instance = SingletonMonoBehaviour<Game>.Instance;
			instance.OnNoise = (Action<Noise>)Delegate.Remove(instance.OnNoise, new Action<Noise>(OnNoise));
		}
	}

	private void OnNoise(Noise noise)
	{
		float num = Vector3.Distance(noise.Position, base.transform.position);
		if (num < 2f && !awake)
		{
			FlyAway.Play();
			Animator.SetBool("awake", value: true);
			awake = true;
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.ScarePidgeon);
		}
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		base.OnHit(direction, hitter, medium);
		if (hitter is Dragon)
		{
			SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.DragonKillPigeon);
		}
	}
}
