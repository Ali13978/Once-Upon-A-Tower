using UnityEngine;

namespace Flux
{
	public class FParticleTrack : FTrack
	{
		public override CacheMode AllowedCacheMode => (CacheMode)0;

		public override CacheMode RequiredCacheMode => (CacheMode)0;

		public ParticleSystem ParticleSystem
		{
			get;
			private set;
		}

		public override void Init()
		{
			ParticleSystem = Owner.GetComponentInChildren<ParticleSystem>();
			base.Init();
		}
	}
}
