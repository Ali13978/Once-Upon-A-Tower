using UnityEngine;

namespace Flux
{
	public class FUtility
	{
		public const int FLUX_VERSION = 210;

		public static bool IsAnimationEditable(AnimationClip clip)
		{
			return clip == null || ((clip.hideFlags & HideFlags.NotEditable) == HideFlags.None && !clip.isLooping);
		}

		public static void ResizeAnimationCurve(AnimationCurve curve, float newLength)
		{
			float num = 60f;
			float num2 = (curve.length != 0) ? curve.keys[curve.length - 1].time : 0f;
			if (num2 == 0f)
			{
				curve.AddKey(0f, 1f);
				curve.AddKey(newLength, 1f);
				return;
			}
			float num3 = newLength / num2;
			float num4 = 1f / num3;
			int num5 = 0;
			int num6 = curve.length;
			int num7 = 1;
			if (num3 > 1f)
			{
				num5 = num6 - 1;
				num6 = -1;
				num7 = -1;
			}
			for (int i = num5; i != num6; i += num7)
			{
				Keyframe key = new Keyframe((float)Mathf.RoundToInt(curve.keys[i].time * num3 * num) / num, curve.keys[i].value, curve.keys[i].inTangent * num4, curve.keys[i].outTangent * num4);
				key.tangentMode = curve.keys[i].tangentMode;
				curve.MoveKey(i, key);
			}
		}
	}
}
