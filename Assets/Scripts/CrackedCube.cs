using System.Collections;
using UnityEngine;

public class CrackedCube : WorldObject
{
	public float BreakDelay = 0.5f;

	public AudioSource CrackingAudio;

	public ParticleSystem CrackingParticles;

	private bool breaking;

	public override void OnSteppedOn(WorldObject entity)
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(AutoBreakCoroutine(entity));
		}
	}

	private IEnumerator AutoBreakCoroutine(WorldObject breaker)
	{
		if (!breaking)
		{
			breaking = true;
			GetComponent<Animation>().Play();
			CrackingAudio.Play();
			CrackingParticles.Play();
			yield return new WaitForSeconds(BreakDelay);
			CrackingAudio.Stop();
			Breakable.Break(Vector3.zero, breaker);
		}
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		CrackingAudio.Stop();
		base.OnHit(direction, hitter, medium);
	}
}
