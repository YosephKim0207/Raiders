using System;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x020004ED RID: 1261
	public class TweenEasingFunctions
	{
		// Token: 0x06001E2E RID: 7726 RVA: 0x0008A1B4 File Offset: 0x000883B4
		public static float Linear(float t)
		{
			return t;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0008A1B8 File Offset: 0x000883B8
		public static float Spring(float t)
		{
			float num = Mathf.Clamp01(t);
			return (Mathf.Sin(num * 3.1415927f * (0.2f + 2.5f * num * num * num)) * Mathf.Pow(1f - num, 2.2f) + num) * (1f + 1.2f * (1f - num));
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0008A214 File Offset: 0x00088414
		public static float EaseInQuad(float t)
		{
			return t * t;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0008A21C File Offset: 0x0008841C
		public static float EaseOutQuad(float t)
		{
			return -1f * t * (t - 2f);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0008A230 File Offset: 0x00088430
		public static float EaseInOutQuad(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return 0.5f * t * t;
			}
			t -= 1f;
			return -0.5f * (t * (t - 2f) - 1f);
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0008A270 File Offset: 0x00088470
		public static float EaseInCubic(float t)
		{
			return t * t * t;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0008A278 File Offset: 0x00088478
		public static float EaseOutCubic(float t)
		{
			t -= 1f;
			return t * t * t + 1f;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0008A290 File Offset: 0x00088490
		public static float EaseInOutCubic(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return 0.5f * t * t * t;
			}
			t -= 2f;
			return 0.5f * (t * t * t + 2f);
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0008A2CC File Offset: 0x000884CC
		public static float EaseInQuart(float t)
		{
			return t * t * t * t;
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0008A2D8 File Offset: 0x000884D8
		public static float EaseOutQuart(float t)
		{
			t -= 1f;
			return -1f * (t * t * t * t - 1f);
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0008A2F8 File Offset: 0x000884F8
		public static float EaseInOutQuart(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return 0.5f * t * t * t * t;
			}
			t -= 2f;
			return -0.5f * (t * t * t * t - 2f);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0008A338 File Offset: 0x00088538
		public static float EaseInQuint(float t)
		{
			return t * t * t * t * t;
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0008A344 File Offset: 0x00088544
		public static float EaseOutQuint(float t)
		{
			t -= 1f;
			return t * t * t * t * t + 1f;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0008A360 File Offset: 0x00088560
		public static float EaseInOutQuint(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return 0.5f * t * t * t * t * t;
			}
			t -= 2f;
			return 0.5f * (t * t * t * t * t + 2f);
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0008A3B0 File Offset: 0x000885B0
		public static float EaseInSine(float t)
		{
			return -1f * Mathf.Cos(t / 1f * 1.5707964f) + 1f;
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0008A3D0 File Offset: 0x000885D0
		public static float EaseOutSine(float t)
		{
			return Mathf.Sin(t / 1f * 1.5707964f);
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0008A3E4 File Offset: 0x000885E4
		public static float EaseInOutSine(float t)
		{
			return -0.5f * (Mathf.Cos(3.1415927f * t / 1f) - 1f);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0008A404 File Offset: 0x00088604
		public static float EaseInExpo(float t)
		{
			return Mathf.Pow(2f, 10f * (t / 1f - 1f));
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0008A424 File Offset: 0x00088624
		public static float EaseOutExpo(float t)
		{
			return -Mathf.Pow(2f, -10f * t / 1f) + 1f;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0008A444 File Offset: 0x00088644
		public static float EaseInOutExpo(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return 0.5f * Mathf.Pow(2f, 10f * (t - 1f));
			}
			t -= 1f;
			return 0.5f * (-Mathf.Pow(2f, -10f * t) + 2f);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0008A4AC File Offset: 0x000886AC
		public static float EaseInCirc(float t)
		{
			return -1f * (Mathf.Sqrt(1f - t * t) - 1f);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0008A4C8 File Offset: 0x000886C8
		public static float EaseOutCirc(float t)
		{
			t -= 1f;
			return Mathf.Sqrt(1f - t * t);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0008A4E4 File Offset: 0x000886E4
		public static float EaseInOutCirc(float t)
		{
			t /= 0.5f;
			if (t < 1f)
			{
				return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
			}
			t -= 2f;
			return 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0008A544 File Offset: 0x00088744
		public static float EaseInBack(float t)
		{
			t /= 1f;
			float num = 1.70158f;
			return t * t * ((num + 1f) * t - num);
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0008A570 File Offset: 0x00088770
		public static float EaseOutBack(float t)
		{
			float num = 1.70158f;
			t = t / 1f - 1f;
			return t * t * ((num + 1f) * t + num) + 1f;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0008A5A8 File Offset: 0x000887A8
		public static float EaseInOutBack(float t)
		{
			float num = 1.70158f;
			t /= 0.5f;
			if (t < 1f)
			{
				num *= 1.525f;
				return 0.5f * (t * t * ((num + 1f) * t - num));
			}
			t -= 2f;
			num *= 1.525f;
			return 0.5f * (t * t * ((num + 1f) * t + num) + 2f);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0008A61C File Offset: 0x0008881C
		public static float Bounce(float t)
		{
			t /= 1f;
			if (t < 0.36363637f)
			{
				return 7.5625f * t * t;
			}
			if (t < 0.72727275f)
			{
				t -= 0.54545456f;
				return 7.5625f * t * t + 0.75f;
			}
			if ((double)t < 0.9090909090909091)
			{
				t -= 0.8181818f;
				return 7.5625f * t * t + 0.9375f;
			}
			t -= 0.95454544f;
			return 7.5625f * t * t + 0.984375f;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0008A6AC File Offset: 0x000888AC
		public static float Punch(float t)
		{
			if (t == 0f)
			{
				return 0f;
			}
			if (t == 1f)
			{
				return 0f;
			}
			float num = 0.3f;
			float num2 = num / 6.2831855f * Mathf.Asin(0f);
			return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 1f - num2) * 6.2831855f / num);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0008A724 File Offset: 0x00088924
		public static TweenEasingCallback GetFunction(EasingType easeType)
		{
			switch (easeType)
			{
			case EasingType.Linear:
				return new TweenEasingCallback(TweenEasingFunctions.Linear);
			case EasingType.Bounce:
				return new TweenEasingCallback(TweenEasingFunctions.Bounce);
			case EasingType.BackEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInBack);
			case EasingType.BackEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutBack);
			case EasingType.BackEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutBack);
			case EasingType.CircEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInCirc);
			case EasingType.CircEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutCirc);
			case EasingType.CircEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutCirc);
			case EasingType.CubicEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInCubic);
			case EasingType.CubicEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutCubic);
			case EasingType.CubicEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutCubic);
			case EasingType.ExpoEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInExpo);
			case EasingType.ExpoEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutExpo);
			case EasingType.ExpoEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutExpo);
			case EasingType.QuadEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInQuad);
			case EasingType.QuadEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutQuad);
			case EasingType.QuadEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutQuad);
			case EasingType.QuartEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInQuart);
			case EasingType.QuartEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutQuart);
			case EasingType.QuartEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutQuart);
			case EasingType.QuintEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInQuint);
			case EasingType.QuintEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutQuint);
			case EasingType.QuintEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutQuint);
			case EasingType.SineEaseIn:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInSine);
			case EasingType.SineEaseOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseOutSine);
			case EasingType.SineEaseInOut:
				return new TweenEasingCallback(TweenEasingFunctions.EaseInOutSine);
			case EasingType.Spring:
				return new TweenEasingCallback(TweenEasingFunctions.Spring);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
