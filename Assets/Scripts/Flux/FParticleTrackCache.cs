using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FParticleTrackCache : FTrackCache
	{
		private List<KeyValuePair<int, ParticleSystem.Particle[]>> _particles = new List<KeyValuePair<int, ParticleSystem.Particle[]>>();

		public FParticleTrackCache(FTrack track)
			: base(track)
		{
		}

		protected override bool BuildInternal()
		{
			FSequence sequence = base.Track.Sequence;
			FParticleTrack fParticleTrack = (FParticleTrack)base.Track;
			float currentTime = sequence.CurrentTime;
			if (currentTime >= 0f)
			{
				sequence.Stop();
			}
			_particles.Clear();
			int i = 0;
			for (int num = sequence.Length + 1; i != num; i++)
			{
				fParticleTrack.UpdateEvents(i, (float)i * fParticleTrack.Sequence.InverseFrameRate);
				ParticleSystem.Particle[] array = new ParticleSystem.Particle[fParticleTrack.ParticleSystem.main.maxParticles];
				KeyValuePair<int, ParticleSystem.Particle[]> item = new KeyValuePair<int, ParticleSystem.Particle[]>(fParticleTrack.ParticleSystem.GetParticles(array), array);
				_particles.Add(item);
			}
			sequence.Stop();
			if (currentTime >= 0f)
			{
				sequence.SetCurrentTime(currentTime);
			}
			return true;
		}

		protected override bool ClearInternal()
		{
			_particles.Clear();
			return true;
		}

		public override void GetPlaybackAt(float sequenceTime)
		{
			FParticleTrack fParticleTrack = (FParticleTrack)base.Track;
			int index = (int)(sequenceTime * (float)fParticleTrack.Sequence.FrameRate);
			fParticleTrack.ParticleSystem.SetParticles(_particles[index].Value, _particles[index].Key);
		}
	}
}
