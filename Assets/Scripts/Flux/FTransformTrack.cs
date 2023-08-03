using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FTransformTrack : FTrack
	{
		private static Dictionary<FSequence, Dictionary<Transform, TransformSnapshot>> _snapshots = new Dictionary<FSequence, Dictionary<Transform, TransformSnapshot>>();

		protected TransformSnapshot _snapshot;

		public TransformSnapshot Snapshot => _snapshot;

		public static TransformSnapshot GetSnapshot(FSequence sequence, Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			Dictionary<Transform, TransformSnapshot> value = null;
			if (!_snapshots.TryGetValue(sequence, out value))
			{
				value = new Dictionary<Transform, TransformSnapshot>();
				_snapshots.Add(sequence, value);
			}
			TransformSnapshot value2 = null;
			if (!value.TryGetValue(transform, out value2))
			{
				value2 = new TransformSnapshot(transform);
				value.Add(transform, value2);
			}
			return value2;
		}

		public override void Init()
		{
			base.Init();
			_snapshot = GetSnapshot(Sequence, Owner);
		}

		public override void Stop()
		{
			base.Stop();
			if (_snapshot != null)
			{
				_snapshot.Restore();
			}
		}
	}
}
