using Flux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSource : WorldObject
{
	public GameObject BulletPrefab;

	public float Speed = 2f;

	public Vector3 Direction = Vector3.right;

	public float[] Period = new float[1]
	{
		1.5f
	};

	public float Offset;

	public AudioSource[] FireAudio;

	private int fireAudioIndex;

	private float nextTime = float.MaxValue;

	public FSequence FireSequence;

	private List<Fireball> pool = new List<Fireball>();

	private void Start()
	{
		CalcNextTime();
	}

	private void FixedUpdate()
	{
		if (!base.Broken && !Disarmed)
		{
			FlipContent.transform.localRotation = Quaternion.AngleAxis((Direction == Vector3.left) ? 180 : 0, Vector3.up);
			if (Time.fixedTime > nextTime)
			{
				CalcNextTime();
				StartCoroutine(FireCoroutine());
			}
		}
	}

	private void CalcNextTime()
	{
		float num = 0f;
		for (int i = 0; i < Period.Length; i++)
		{
			num += Period[i];
		}
		nextTime = Mathf.Floor((Time.fixedTime - Offset) / num) * num + Offset;
		for (int j = 0; j < Period.Length; j++)
		{
			if (nextTime > Time.fixedTime)
			{
				break;
			}
			nextTime += Period[j];
		}
	}

	private IEnumerator FireCoroutine()
	{
		if (FireAudio.Length > 0)
		{
			FireAudio[fireAudioIndex].Play();
			fireAudioIndex = (fireAudioIndex + 1) % FireAudio.Length;
		}
		if ((bool)FireSequence)
		{
			FireSequence.Stop();
			FireSequence.Play();
		}
		if (DebugLog)
		{
			UnityEngine.Debug.Log("Fire wait");
		}
		float waitTime = 0.333333343f;
		float startTime = Time.fixedTime;
		WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
		while (Time.fixedTime - startTime < waitTime)
		{
			yield return fixedUpdate;
		}
		if (!base.Broken && !Disarmed)
		{
			if (DebugLog)
			{
				UnityEngine.Debug.Log("Fire");
			}
			Fireball arrow;
			if (pool.Count > 0)
			{
				arrow = pool[pool.Count - 1];
				pool.RemoveAt(pool.Count - 1);
			}
			else
			{
				arrow = Object.Instantiate(BulletPrefab).GetComponent<Fireball>();
			}
			arrow.transform.parent = base.transform.parent;
			arrow.transform.position = base.transform.position;
			arrow.Fire(this, Direction, Speed);
		}
	}

	public override void Flip()
	{
		Direction = -Direction;
	}

	public void Pool(Fireball arrow)
	{
		pool.Add(arrow);
	}

	public void UnPool(Fireball arrow)
	{
		pool.Remove(arrow);
	}
}
