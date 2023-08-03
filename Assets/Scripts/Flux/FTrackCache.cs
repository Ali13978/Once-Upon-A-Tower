namespace Flux
{
	public abstract class FTrackCache
	{
		private FTrack _track;

		private bool _isBuilt;

		public FTrack Track
		{
			get
			{
				return _track;
			}
			protected set
			{
				_track = value;
			}
		}

		public bool IsBuilt => _isBuilt;

		public FTrackCache(FTrack track)
		{
			_track = track;
		}

		public void Build(bool rebuild)
		{
			if (IsBuilt)
			{
				if (!rebuild)
				{
					return;
				}
				Clear();
			}
			_isBuilt = BuildInternal();
		}

		public void Build()
		{
			Build(rebuild: true);
		}

		protected abstract bool BuildInternal();

		public void Clear()
		{
			if (IsBuilt)
			{
				_isBuilt = !ClearInternal();
			}
		}

		protected abstract bool ClearInternal();

		public abstract void GetPlaybackAt(float sequenceTime);
	}
}
