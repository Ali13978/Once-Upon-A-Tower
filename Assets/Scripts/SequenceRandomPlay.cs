using Flux;
using UnityEngine;

public class SequenceRandomPlay : MonoBehaviour
{
	public FSequence Sequence;

	public float MinTime = 2f;

	public float MaxTime = 4f;

	private float nextTime;

	private void Start()
	{
		nextTime = Time.time + UnityEngine.Random.Range(MinTime, MaxTime);
	}

	private void Update()
	{
		if (Time.time > nextTime)
		{
			if (Sequence != null && SingletonMonoBehaviour<Game>.Instance.Ready)
			{
				Sequence.Stop();
				Sequence.Play();
			}
			nextTime = Time.time + UnityEngine.Random.Range(MinTime, MaxTime);
		}
	}
}
