using UnityEngine;

namespace Flux
{
	public class FSequenceTrack : FTrack
	{
		private FSequence _ownerSequence;

		private FSequence OwnerSequence
		{
			get
			{
				if (_ownerSequence == null)
				{
					_ownerSequence = Owner.GetComponent<FSequence>();
				}
				return _ownerSequence;
			}
		}

		public override CacheMode RequiredCacheMode => CacheMode.Editor | CacheMode.RuntimeBackwards;

		public override CacheMode AllowedCacheMode => RequiredCacheMode | CacheMode.RuntimeForward;

		public override void CreateCache()
		{
			if (!base.HasCache)
			{
				base.Cache = new FSequenceTrackCache(this);
				base.Cache.Build();
			}
		}

		public override void ClearCache()
		{
			if (base.HasCache)
			{
				base.Cache.Clear();
				base.Cache = null;
			}
		}

		public override void Init()
		{
			base.Init();
			if (Application.isPlaying)
			{
				base.enabled = true;
			}
		}
	}
}
