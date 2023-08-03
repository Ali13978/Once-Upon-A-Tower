using UnityEngine;

namespace Flux
{
	[FEvent("Particle System/Play Particle", typeof(FParticleTrack))]
	public class FPlayParticleEvent : FEvent
	{
		[SerializeField]
		[Tooltip("True: ParticleSystem playback speed will be adjusted to match event length\nFalse: ParticleSystem plays at normal speed, i.e. doesn't scale based on event length")]
		private bool _normalizeToEventLength;

		[SerializeField]
		[HideInInspector]
		[Tooltip("Seed to randomize the particle system, 0 = always randomize")]
		private uint _randomSeed = 1u;

		private ParticleSystem _particleSystem;

		private float _previousTimeSinceTrigger;

		private float _previousSpeed;

		protected override void OnInit()
		{
			_particleSystem = ((FParticleTrack)base.Track).ParticleSystem;
			if (_particleSystem != null)
			{
				if (!_particleSystem.isPlaying)
				{
					_particleSystem.randomSeed = _randomSeed;
				}
				ParticleSystem.MainModule main = _particleSystem.main;
			}
			_previousTimeSinceTrigger = 0f;
			_previousSpeed = Sequence.Speed;
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_particleSystem.Play(withChildren: true);
		}

		protected override void OnFinish()
		{
			if (_particleSystem != null)
			{
				_particleSystem.Stop(withChildren: true);
			}
		}

		protected override void OnStop()
		{
			if (_particleSystem != null)
			{
				_particleSystem.Stop(withChildren: true);
				_particleSystem.Clear(withChildren: true);
			}
		}

		protected override void OnPause()
		{
			if (_particleSystem != null)
			{
				_particleSystem.Pause();
			}
		}

		protected override void OnResume()
		{
			if (_particleSystem != null && Sequence.IsPlayingForward)
			{
				_particleSystem.Play(withChildren: true);
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (_particleSystem == null)
			{
				return;
			}
			if (!Sequence.IsPlaying || !Sequence.IsPlayingForward)
			{
				_previousSpeed = 1f;
				ParticleSystem.MainModule main = _particleSystem.main;
				float num = timeSinceTrigger - _previousTimeSinceTrigger;
				_previousTimeSinceTrigger = timeSinceTrigger;
				if (Sequence.IsPlayingForward && num > 0f)
				{
					_particleSystem.Simulate(num, withChildren: true, restart: false);
					return;
				}
				float t = (!_normalizeToEventLength) ? Mathf.Clamp(timeSinceTrigger, 0f, _particleSystem.main.duration) : (timeSinceTrigger / base.LengthTime * _particleSystem.main.duration);
				_particleSystem.Simulate(t, withChildren: true, restart: true);
			}
			else if (_previousSpeed != Sequence.Speed)
			{
				_previousSpeed = Sequence.Speed;
				ParticleSystem.MainModule main2 = _particleSystem.main;
			}
		}
	}
}
