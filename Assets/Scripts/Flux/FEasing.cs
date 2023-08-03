using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Flux
{
	public static class FEasing
	{
		private static Func<float, float, float, float>[] _tweens;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache4;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache5;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache6;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache7;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache8;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache9;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheA;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheB;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheC;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheD;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheE;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cacheF;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache10;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache11;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache12;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache13;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache14;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache15;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache16;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache17;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache18;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache19;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1A;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1B;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1C;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1D;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1E;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache1F;

		[CompilerGenerated]
		private static Func<float, float, float, float> _003C_003Ef__mg_0024cache20;

		static FEasing()
		{
			_tweens = new Func<float, float, float, float>[33]
			{
				Linear,
				Clerp,
				Spring,
				EaseInQuad,
				EaseOutQuad,
				EaseInOutQuad,
				EaseInCubic,
				EaseOutCubic,
				EaseInOutCubic,
				EaseInQuart,
				EaseOutQuart,
				EaseInOutQuart,
				EaseInQuint,
				EaseOutQuint,
				EaseInOutQuint,
				EaseInSine,
				EaseOutSine,
				EaseInOutSine,
				EaseInExpo,
				EaseOutExpo,
				EaseInOutExpo,
				EaseInCirc,
				EaseOutCirc,
				EaseInOutCirc,
				EaseInBounce,
				EaseOutBounce,
				EaseInOutBounce,
				EaseInBack,
				EaseOutBack,
				EaseInOutBack,
				EaseInElastic,
				EaseOutElastic,
				EaseInOutElastic
			};
		}

		public static float Tween(float start, float end, float t, FEasingType easingType)
		{
			return _tweens[(int)easingType](start, end, t);
		}

		public static float Linear(float start, float end, float t)
		{
			end -= start;
			return end * t + start;
		}

		public static float Clerp(float start, float end, float t)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) * 0.5f);
			float num4 = 0f;
			float num5 = 0f;
			if (end - start < 0f - num3)
			{
				num5 = (num2 - start + end) * t;
				return start + num5;
			}
			if (end - start > num3)
			{
				num5 = (0f - (num2 - end + start)) * t;
				return start + num5;
			}
			return start + (end - start) * t;
		}

		public static float Spring(float start, float end, float t)
		{
			t = Mathf.Clamp01(t);
			t = (Mathf.Sin(t * (float)Math.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + 1.2f * (1f - t));
			return start + (end - start) * t;
		}

		public static float EaseInQuad(float start, float end, float t)
		{
			end -= start;
			return end * t * t + start;
		}

		public static float EaseOutQuad(float start, float end, float t)
		{
			end -= start;
			return (0f - end) * t * (t - 2f) + start;
		}

		public static float EaseInOutQuad(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return end * 0.5f * t * t + start;
			}
			t -= 1f;
			return (0f - end) * 0.5f * (t * (t - 2f) - 1f) + start;
		}

		public static float EaseInCubic(float start, float end, float t)
		{
			end -= start;
			return end * t * t * t + start;
		}

		public static float EaseOutCubic(float start, float end, float t)
		{
			t -= 1f;
			end -= start;
			return end * (t * t * t + 1f) + start;
		}

		public static float EaseInOutCubic(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return end * 0.5f * t * t * t + start;
			}
			t -= 2f;
			return end * 0.5f * (t * t * t + 2f) + start;
		}

		public static float EaseInQuart(float start, float end, float t)
		{
			end -= start;
			return end * t * t * t * t + start;
		}

		public static float EaseOutQuart(float start, float end, float t)
		{
			t -= 1f;
			end -= start;
			return (0f - end) * (t * t * t * t - 1f) + start;
		}

		public static float EaseInOutQuart(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return end * 0.5f * t * t * t * t + start;
			}
			t -= 2f;
			return (0f - end) * 0.5f * (t * t * t * t - 2f) + start;
		}

		public static float EaseInQuint(float start, float end, float t)
		{
			end -= start;
			return end * t * t * t * t * t + start;
		}

		public static float EaseOutQuint(float start, float end, float t)
		{
			t -= 1f;
			end -= start;
			return end * (t * t * t * t * t + 1f) + start;
		}

		public static float EaseInOutQuint(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return end * 0.5f * t * t * t * t * t + start;
			}
			t -= 2f;
			return end * 0.5f * (t * t * t * t * t + 2f) + start;
		}

		public static float EaseInSine(float start, float end, float t)
		{
			end -= start;
			return (0f - end) * Mathf.Cos(t * ((float)Math.PI / 2f)) + end + start;
		}

		public static float EaseOutSine(float start, float end, float t)
		{
			end -= start;
			return end * Mathf.Sin(t * ((float)Math.PI / 2f)) + start;
		}

		public static float EaseInOutSine(float start, float end, float t)
		{
			end -= start;
			return (0f - end) * 0.5f * (Mathf.Cos((float)Math.PI * t) - 1f) + start;
		}

		public static float EaseInExpo(float start, float end, float t)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (t - 1f)) + start;
		}

		public static float EaseOutExpo(float start, float end, float t)
		{
			end -= start;
			return end * (0f - Mathf.Pow(2f, -10f * t) + 1f) + start;
		}

		public static float EaseInOutExpo(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return end * 0.5f * Mathf.Pow(2f, 10f * (t - 1f)) + start;
			}
			t -= 1f;
			return end * 0.5f * (0f - Mathf.Pow(2f, -10f * t) + 2f) + start;
		}

		public static float EaseInCirc(float start, float end, float t)
		{
			end -= start;
			return (0f - end) * (Mathf.Sqrt(1f - t * t) - 1f) + start;
		}

		public static float EaseOutCirc(float start, float end, float t)
		{
			t -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - t * t) + start;
		}

		public static float EaseInOutCirc(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1f)
			{
				return (0f - end) * 0.5f * (Mathf.Sqrt(1f - t * t) - 1f) + start;
			}
			t -= 2f;
			return end * 0.5f * (Mathf.Sqrt(1f - t * t) + 1f) + start;
		}

		public static float EaseInBounce(float start, float end, float t)
		{
			end -= start;
			float num = 1f;
			return end - EaseOutBounce(0f, end, num - t) + start;
		}

		public static float EaseOutBounce(float start, float end, float t)
		{
			t /= 1f;
			end -= start;
			if (t < 0.363636374f)
			{
				return end * (7.5625f * t * t) + start;
			}
			if (t < 0.727272749f)
			{
				t -= 0.545454562f;
				return end * (7.5625f * t * t + 0.75f) + start;
			}
			if ((double)t < 0.90909090909090906)
			{
				t -= 0.8181818f;
				return end * (7.5625f * t * t + 0.9375f) + start;
			}
			t -= 21f / 22f;
			return end * (7.5625f * t * t + 63f / 64f) + start;
		}

		public static float EaseInOutBounce(float start, float end, float t)
		{
			end -= start;
			float num = 1f;
			if (t < num * 0.5f)
			{
				return EaseInBounce(0f, end, t * 2f) * 0.5f + start;
			}
			return EaseOutBounce(0f, end, t * 2f - num) * 0.5f + end * 0.5f + start;
		}

		public static float EaseInBack(float start, float end, float t)
		{
			end -= start;
			t /= 1f;
			float num = 1.70158f;
			return end * t * t * ((num + 1f) * t - num) + start;
		}

		public static float EaseOutBack(float start, float end, float t)
		{
			float num = 1.70158f;
			end -= start;
			t -= 1f;
			return end * (t * t * ((num + 1f) * t + num) + 1f) + start;
		}

		public static float EaseInOutBack(float start, float end, float t)
		{
			float num = 1.70158f;
			end -= start;
			t /= 0.5f;
			if (t < 1f)
			{
				num *= 1.525f;
				return end * 0.5f * (t * t * ((num + 1f) * t - num)) + start;
			}
			t -= 2f;
			num *= 1.525f;
			return end * 0.5f * (t * t * ((num + 1f) * t + num) + 2f) + start;
		}

		public static float EaseInElastic(float start, float end, float t)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (t == 0f)
			{
				return start;
			}
			if ((t /= num) == 1f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
			}
			return 0f - num4 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * num - num3) * ((float)Math.PI * 2f) / num2) + start;
		}

		public static float EaseOutElastic(float start, float end, float t)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (t == 0f)
			{
				return start;
			}
			if ((t /= num) == 1f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 * 0.25f;
			}
			else
			{
				num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
			}
			return num4 * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * num - num3) * ((float)Math.PI * 2f) / num2) + end + start;
		}

		public static float EaseInOutElastic(float start, float end, float t)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (t == 0f)
			{
				return start;
			}
			if ((t /= num * 0.5f) == 2f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
			}
			if (t < 1f)
			{
				return -0.5f * (num4 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * num - num3) * ((float)Math.PI * 2f) / num2)) + start;
			}
			return num4 * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * num - num3) * ((float)Math.PI * 2f) / num2) * 0.5f + end + start;
		}

		public static float Punch(float amplitude, float t)
		{
			float num = 9f;
			if (t == 0f)
			{
				return 0f;
			}
			if (t == 1f)
			{
				return 0f;
			}
			float num2 = 0.3f;
			num = num2 / ((float)Math.PI * 2f) * Mathf.Asin(0f);
			return amplitude * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 1f - num) * ((float)Math.PI * 2f) / num2);
		}
	}
}
