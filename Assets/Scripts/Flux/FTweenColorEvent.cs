using UnityEngine;

namespace Flux
{
	[FEvent("Script/Tween Color")]
	public class FTweenColorEvent : FTweenVariableEvent<FTweenColor>
	{
		protected override void SetDefaultValues()
		{
			_tween = new FTweenColor(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f));
		}

		protected override object GetValueAt(float t)
		{
			return _tween.GetValue(t);
		}
	}
}
