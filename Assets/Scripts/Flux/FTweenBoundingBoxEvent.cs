using UnityEngine;

namespace Flux
{
	[FEvent("Gui/Bounding Box Tween", typeof(FGuiTrack))]
	public class FTweenBoundingBoxEvent : FEvent
	{
		public Transform Target;

		protected override void OnTrigger(float timeSinceTrigger)
		{
			OnUpdateEvent(timeSinceTrigger);
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			float t = timeSinceTrigger / base.LengthTime;
			t = Tween.CubicEaseInOut(t, 0f, 1f, 1f);
			Vector3 center = Vector3.Lerp(Owner.transform.position, Target.transform.position, t);
			Vector3 size = Vector3.Lerp(Owner.transform.localScale, Target.transform.localScale, t);
			size.z = 0f;
			Owner.GetComponent<BoundingBox>().ApplyWithBounds(new Bounds(center, size));
		}
	}
}
