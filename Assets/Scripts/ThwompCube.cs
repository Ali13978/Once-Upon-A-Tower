using System.Collections;
using UnityEngine;

public class ThwompCube : WorldObject
{
	public Vector3 Direction = Vector3.down;

	public float AccelerationMagnitude = 160f;

	public float ShakeTime = 0.5f;

	public float AfterShakeTime = 0.1f;

	public float RetreatDelay = 0.5f;

	public float Period = 1.5f;

	public float Offset;

	public int MaxLength = 5;

	public AudioSource TrembleSound;

	public AudioSource SwingSound;

	public AudioSource ImpactSound;

	public AudioSource FoldSound;

	private Vector3 direction;

	public GameObject ThwompPrefab;

	private Thwomp thwomp;

	public Animation ShakeAnimation;

	private IEnumerator fireCoroutine;

	public override void Initialize()
	{
		base.Initialize();
		if (SingletonMonoBehaviour<World>.Instance.Sections[Coord.Section].Flipped)
		{
			Direction.x *= -1f;
		}
		direction = Direction;
		thwomp = Object.Instantiate(ThwompPrefab, base.transform, worldPositionStays: true).GetComponent<Thwomp>();
		thwomp.Coord = Coord;
		thwomp.transform.position = base.transform.position;
		thwomp.Source = this;
		thwomp.MaxLength = MaxLength;
		thwomp.Initialize();
		thwomp.transform.localRotation = Quaternion.LookRotation(Vector3.forward, -direction);
	}

	private void FixedUpdate()
	{
		if (!base.Broken && !Disarmed && (Time.fixedTime - Offset) % Period < Time.deltaTime)
		{
			fireCoroutine = FireCoroutine();
			StartCoroutine(fireCoroutine);
		}
	}

	public override void Disarm()
	{
		base.Disarm();
		if (fireCoroutine != null)
		{
			StopCoroutine(fireCoroutine);
			fireCoroutine = null;
		}
		thwomp.Disarm();
	}

	public void Retreat()
	{
		if (fireCoroutine != null)
		{
			StopCoroutine(fireCoroutine);
			fireCoroutine = null;
		}
		StartCoroutine(RetreatCoroutine());
	}

	private IEnumerator RetreatCoroutine()
	{
		FoldSound.Play();
		thwomp.Retreat(-direction, AccelerationMagnitude);
		yield return new WaitForFixedUpdate();
		while (thwomp.Moving)
		{
			yield return new WaitForFixedUpdate();
		}
	}

	private IEnumerator FireCoroutine()
	{
		TrembleSound.Play();
		ShakeAnimation.Play();
		yield return new WaitForSeconds(ShakeTime + AfterShakeTime);
		yield return new WaitForFixedUpdate();
		thwomp.Fire(direction, AccelerationMagnitude);
		SwingSound.Play();
		float startTime = Time.time;
		yield return new WaitForFixedUpdate();
		while (thwomp.Moving)
		{
			yield return new WaitForFixedUpdate();
		}
		SwingSound.Stop();
		ImpactSound.Play();
		while (Time.time < startTime + RetreatDelay)
		{
			yield return new WaitForFixedUpdate();
		}
		yield return RetreatCoroutine();
		fireCoroutine = null;
	}
}
