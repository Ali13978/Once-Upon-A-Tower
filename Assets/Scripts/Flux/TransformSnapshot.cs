using UnityEngine;

namespace Flux
{
	public class TransformSnapshot
	{
		public TransformSnapshot[] ChildrenSnapshots;

		public Transform Transform
		{
			get;
			private set;
		}

		public Transform Parent
		{
			get;
			private set;
		}

		public Vector3 LocalPosition
		{
			get;
			private set;
		}

		public Quaternion LocalRotation
		{
			get;
			private set;
		}

		public Vector3 LocalScale
		{
			get;
			private set;
		}

		public TransformSnapshot(Transform transform, bool recursive = false)
		{
			Transform = transform;
			Parent = Transform.parent;
			LocalPosition = Transform.localPosition;
			LocalRotation = Transform.localRotation;
			LocalScale = Transform.localScale;
			if (recursive)
			{
				TakeChildSnapshots();
			}
		}

		public void TakeChildSnapshots()
		{
			if (ChildrenSnapshots == null)
			{
				ChildrenSnapshots = new TransformSnapshot[Transform.childCount];
				for (int i = 0; i != ChildrenSnapshots.Length; i++)
				{
					ChildrenSnapshots[i] = new TransformSnapshot(Transform.GetChild(i), recursive: true);
				}
			}
		}

		public void Restore()
		{
			if (Parent != Transform.parent)
			{
				Transform.SetParent(Parent);
			}
			Transform.localPosition = LocalPosition;
			Transform.localRotation = LocalRotation;
			Transform.localScale = LocalScale;
			if (ChildrenSnapshots != null)
			{
				for (int i = 0; i != ChildrenSnapshots.Length; i++)
				{
					ChildrenSnapshots[i].Restore();
				}
			}
		}
	}
}
