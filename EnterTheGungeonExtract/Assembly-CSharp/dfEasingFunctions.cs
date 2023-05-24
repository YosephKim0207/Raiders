using System;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class dfEasingFunctions
{
	// Token: 0x06001C66 RID: 7270 RVA: 0x00085A3C File Offset: 0x00083C3C
	public static dfEasingFunctions.EasingFunction GetFunction(dfEasingType easeType)
	{
		switch (easeType)
		{
		case dfEasingType.Linear:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.linear);
		case dfEasingType.Bounce:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.bounce);
		case dfEasingType.BackEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInBack);
		case dfEasingType.BackEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutBack);
		case dfEasingType.BackEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutBack);
		case dfEasingType.CircEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInCirc);
		case dfEasingType.CircEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutCirc);
		case dfEasingType.CircEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutCirc);
		case dfEasingType.CubicEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInCubic);
		case dfEasingType.CubicEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutCubic);
		case dfEasingType.CubicEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutCubic);
		case dfEasingType.ExpoEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInExpo);
		case dfEasingType.ExpoEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutExpo);
		case dfEasingType.ExpoEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutExpo);
		case dfEasingType.QuadEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuad);
		case dfEasingType.QuadEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuad);
		case dfEasingType.QuadEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuad);
		case dfEasingType.QuartEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuart);
		case dfEasingType.QuartEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuart);
		case dfEasingType.QuartEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuart);
		case dfEasingType.QuintEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInQuint);
		case dfEasingType.QuintEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutQuint);
		case dfEasingType.QuintEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutQuint);
		case dfEasingType.SineEaseIn:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInSine);
		case dfEasingType.SineEaseOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeOutSine);
		case dfEasingType.SineEaseInOut:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.easeInOutSine);
		case dfEasingType.Spring:
			return new dfEasingFunctions.EasingFunction(dfEasingFunctions.spring);
		default:
			throw new NotImplementedException();
		}
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x00085DF0 File Offset: 0x00083FF0
	private static float linear(float start, float end, float time)
	{
		return Mathf.Lerp(start, end, time);
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x00085DFC File Offset: 0x00083FFC
	private static float clerp(float start, float end, float time)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * time;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * time;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * time;
		}
		return num5;
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x00085E74 File Offset: 0x00084074
	private static float spring(float start, float end, float time)
	{
		time = Mathf.Clamp01(time);
		time = (Mathf.Sin(time * 3.1415927f * (0.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + 1.2f * (1f - time));
		return start + (end - start) * time;
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x00085ED8 File Offset: 0x000840D8
	private static float easeInQuad(float start, float end, float time)
	{
		end -= start;
		return end * time * time + start;
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x00085EE8 File Offset: 0x000840E8
	private static float easeOutQuad(float start, float end, float time)
	{
		end -= start;
		return -end * time * (time - 2f) + start;
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x00085F00 File Offset: 0x00084100
	private static float easeInOutQuad(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return end / 2f * time * time + start;
		}
		time -= 1f;
		return -end / 2f * (time * (time - 2f) - 1f) + start;
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x00085F58 File Offset: 0x00084158
	private static float easeInCubic(float start, float end, float time)
	{
		end -= start;
		return end * time * time * time + start;
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x00085F68 File Offset: 0x00084168
	private static float easeOutCubic(float start, float end, float time)
	{
		time -= 1f;
		end -= start;
		return end * (time * time * time + 1f) + start;
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x00085F88 File Offset: 0x00084188
	private static float easeInOutCubic(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return end / 2f * time * time * time + start;
		}
		time -= 2f;
		return end / 2f * (time * time * time + 2f) + start;
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x00085FDC File Offset: 0x000841DC
	private static float easeInQuart(float start, float end, float time)
	{
		end -= start;
		return end * time * time * time * time + start;
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x00085FF0 File Offset: 0x000841F0
	private static float easeOutQuart(float start, float end, float time)
	{
		time -= 1f;
		end -= start;
		return -end * (time * time * time * time - 1f) + start;
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x00086014 File Offset: 0x00084214
	private static float easeInOutQuart(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return end / 2f * time * time * time * time + start;
		}
		time -= 2f;
		return -end / 2f * (time * time * time * time - 2f) + start;
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00086070 File Offset: 0x00084270
	private static float easeInQuint(float start, float end, float time)
	{
		end -= start;
		return end * time * time * time * time * time + start;
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x00086084 File Offset: 0x00084284
	private static float easeOutQuint(float start, float end, float time)
	{
		time -= 1f;
		end -= start;
		return end * (time * time * time * time * time + 1f) + start;
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x000860A8 File Offset: 0x000842A8
	private static float easeInOutQuint(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return end / 2f * time * time * time * time * time + start;
		}
		time -= 2f;
		return end / 2f * (time * time * time * time * time + 2f) + start;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x00086104 File Offset: 0x00084304
	private static float easeInSine(float start, float end, float time)
	{
		end -= start;
		return -end * Mathf.Cos(time / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x00086124 File Offset: 0x00084324
	private static float easeOutSine(float start, float end, float time)
	{
		end -= start;
		return end * Mathf.Sin(time / 1f * 1.5707964f) + start;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00086144 File Offset: 0x00084344
	private static float easeInOutSine(float start, float end, float time)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * time / 1f) - 1f) + start;
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x00086170 File Offset: 0x00084370
	private static float easeInExpo(float start, float end, float time)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (time / 1f - 1f)) + start;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00086198 File Offset: 0x00084398
	private static float easeOutExpo(float start, float end, float time)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * time / 1f) + 1f) + start;
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000861C4 File Offset: 0x000843C4
	private static float easeInOutExpo(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (time - 1f)) + start;
		}
		time -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * time) + 2f) + start;
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00086238 File Offset: 0x00084438
	private static float easeInCirc(float start, float end, float time)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - time * time) - 1f) + start;
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00086258 File Offset: 0x00084458
	private static float easeOutCirc(float start, float end, float time)
	{
		time -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - time * time) + start;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x0008627C File Offset: 0x0008447C
	private static float easeInOutCirc(float start, float end, float time)
	{
		time /= 0.5f;
		end -= start;
		if (time < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - time * time) - 1f) + start;
		}
		time -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - time * time) + 1f) + start;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000862EC File Offset: 0x000844EC
	private static float bounce(float start, float end, float time)
	{
		time /= 1f;
		end -= start;
		if (time < 0.36363637f)
		{
			return end * (7.5625f * time * time) + start;
		}
		if (time < 0.72727275f)
		{
			time -= 0.54545456f;
			return end * (7.5625f * time * time + 0.75f) + start;
		}
		if ((double)time < 0.9090909090909091)
		{
			time -= 0.8181818f;
			return end * (7.5625f * time * time + 0.9375f) + start;
		}
		time -= 0.95454544f;
		return end * (7.5625f * time * time + 0.984375f) + start;
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00086394 File Offset: 0x00084594
	private static float easeInBack(float start, float end, float time)
	{
		end -= start;
		time /= 1f;
		float num = 1.70158f;
		return end * time * time * ((num + 1f) * time - num) + start;
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000863C8 File Offset: 0x000845C8
	private static float easeOutBack(float start, float end, float time)
	{
		float num = 1.70158f;
		end -= start;
		time = time / 1f - 1f;
		return end * (time * time * ((num + 1f) * time + num) + 1f) + start;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00086408 File Offset: 0x00084608
	private static float easeInOutBack(float start, float end, float time)
	{
		float num = 1.70158f;
		end -= start;
		time /= 0.5f;
		if (time < 1f)
		{
			num *= 1.525f;
			return end / 2f * (time * time * ((num + 1f) * time - num)) + start;
		}
		time -= 2f;
		num *= 1.525f;
		return end / 2f * (time * time * ((num + 1f) * time + num) + 2f) + start;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00086488 File Offset: 0x00084688
	private static float punch(float amplitude, float time)
	{
		if (time == 0f)
		{
			return 0f;
		}
		if (time == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x020004C1 RID: 1217
	// (Invoke) Token: 0x06001C85 RID: 7301
	public delegate float EasingFunction(float start, float end, float time);
}
