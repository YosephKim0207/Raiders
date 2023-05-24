using System;
using System.Collections.Generic;
using TestSimpleRNG;
using UnityEngine;

// Token: 0x0200181F RID: 6175
public static class BraveMathCollege
{
	// Token: 0x060091AC RID: 37292 RVA: 0x003D9EB8 File Offset: 0x003D80B8
	public static float GetLowDiscrepancyRandom(int iterator)
	{
		return BraveMathCollege.LowDiscrepancyPseudoRandoms[iterator % BraveMathCollege.LowDiscrepancyPseudoRandoms.Length];
	}

	// Token: 0x060091AD RID: 37293 RVA: 0x003D9ECC File Offset: 0x003D80CC
	private static float ANG_NoiseInternal(float freq)
	{
		float num = UnityEngine.Random.Range(0f, 6.2831855f);
		return Mathf.Sin(6.2831855f * freq + num);
	}

	// Token: 0x060091AE RID: 37294 RVA: 0x003D9EF8 File Offset: 0x003D80F8
	private static float ANG_WeightedSumNoise(float[] amplitudes, float[] noises)
	{
		float num = 0f;
		for (int i = 0; i < noises.Length; i++)
		{
			num += amplitudes[i] * noises[i];
		}
		return num;
	}

	// Token: 0x060091AF RID: 37295 RVA: 0x003D9F2C File Offset: 0x003D812C
	private static float AdvancedNoiseGenerator(Func<float, float> amplitudeLambda)
	{
		float[] array = new float[]
		{
			1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f,
			11f, 12f, 13f, 14f, 15f, 16f, 17f, 18f, 19f, 20f,
			21f, 22f, 23f, 24f, 25f, 26f, 27f, 28f, 29f, 30f
		};
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = amplitudeLambda(array[i] / (float)array.Length);
		}
		float[] array3 = new float[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array3[j] = BraveMathCollege.ANG_NoiseInternal(array[j] / (float)array.Length);
		}
		return BraveMathCollege.ANG_WeightedSumNoise(array2, array3);
	}

	// Token: 0x060091B0 RID: 37296 RVA: 0x003D9FB8 File Offset: 0x003D81B8
	public static float GetRedNoise()
	{
		return BraveMathCollege.AdvancedNoiseGenerator((float f) => 1f / f / f);
	}

	// Token: 0x060091B1 RID: 37297 RVA: 0x003D9FDC File Offset: 0x003D81DC
	public static float GetPinkNoise()
	{
		return BraveMathCollege.AdvancedNoiseGenerator((float f) => 1f / f);
	}

	// Token: 0x060091B2 RID: 37298 RVA: 0x003DA000 File Offset: 0x003D8200
	public static float GetWhiteNoise()
	{
		return BraveMathCollege.AdvancedNoiseGenerator((float f) => 1f);
	}

	// Token: 0x060091B3 RID: 37299 RVA: 0x003DA024 File Offset: 0x003D8224
	public static float GetBlueNoise()
	{
		return BraveMathCollege.AdvancedNoiseGenerator((float f) => f);
	}

	// Token: 0x060091B4 RID: 37300 RVA: 0x003DA048 File Offset: 0x003D8248
	public static float GetVioletNoise()
	{
		return BraveMathCollege.AdvancedNoiseGenerator((float f) => f * f);
	}

	// Token: 0x060091B5 RID: 37301 RVA: 0x003DA06C File Offset: 0x003D826C
	public static float GetRandomByNormalDistribution(float mean, float stddev)
	{
		return (float)SimpleRNG.GetNormal((double)mean, (double)stddev);
	}

	// Token: 0x060091B6 RID: 37302 RVA: 0x003DA078 File Offset: 0x003D8278
	public static float NormalDistributionAtPosition(float x, float mean, float stddev)
	{
		float num = 0.15915494f;
		float num2 = 2f * stddev;
		return BraveMathCollege.NormalDistributionAtPosition(x, num, mean, num2);
	}

	// Token: 0x060091B7 RID: 37303 RVA: 0x003DA09C File Offset: 0x003D829C
	public static float NormalDistributionAtPosition(float x, float oneOverTwoPi, float mean, float twoTimeStdDev)
	{
		return oneOverTwoPi * Mathf.Exp(-(x - mean) * (x - mean) / (2f * twoTimeStdDev));
	}

	// Token: 0x060091B8 RID: 37304 RVA: 0x003DA0B8 File Offset: 0x003D82B8
	public static float UnboundedLerp(float from, float to, float t)
	{
		return (to - from) * t + from;
	}

	// Token: 0x060091B9 RID: 37305 RVA: 0x003DA0C4 File Offset: 0x003D82C4
	public static float SmoothLerp(float from, float to, float t)
	{
		return Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
	}

	// Token: 0x060091BA RID: 37306 RVA: 0x003DA0E0 File Offset: 0x003D82E0
	public static float Bilerp(float x0y0, float x1y0, float x0y1, float x1y1, float u, float v)
	{
		float num = Mathf.Lerp(x0y0, x1y0, u);
		float num2 = Mathf.Lerp(x0y1, x1y1, u);
		return Mathf.Lerp(num, num2, v);
	}

	// Token: 0x060091BB RID: 37307 RVA: 0x003DA10C File Offset: 0x003D830C
	public static float DoubleLerp(float from, float intermediary, float to, float t)
	{
		return (t >= 0.5f) ? Mathf.Lerp(intermediary, to, t * 2f - 1f) : Mathf.Lerp(from, intermediary, t * 2f);
	}

	// Token: 0x060091BC RID: 37308 RVA: 0x003DA140 File Offset: 0x003D8340
	public static Vector2 DoubleLerp(Vector2 from, Vector2 intermediary, Vector2 to, float t)
	{
		return (t >= 0.5f) ? Vector2.Lerp(intermediary, to, t * 2f - 1f) : Vector2.Lerp(from, intermediary, t * 2f);
	}

	// Token: 0x060091BD RID: 37309 RVA: 0x003DA174 File Offset: 0x003D8374
	public static Vector3 DoubleLerp(Vector3 from, Vector3 intermediary, Vector3 to, float t)
	{
		return (t >= 0.5f) ? Vector3.Lerp(intermediary, to, t * 2f - 1f) : Vector3.Lerp(from, intermediary, t * 2f);
	}

	// Token: 0x060091BE RID: 37310 RVA: 0x003DA1A8 File Offset: 0x003D83A8
	public static float DoubleLerpSmooth(float from, float intermediary, float to, float t)
	{
		return (t >= 0.5f) ? Mathf.Lerp(intermediary, to, Mathf.SmoothStep(0f, 1f, t * 2f - 1f)) : Mathf.Lerp(from, intermediary, Mathf.SmoothStep(0f, 1f, t * 2f));
	}

	// Token: 0x060091BF RID: 37311 RVA: 0x003DA208 File Offset: 0x003D8408
	public static Vector2 DoubleLerpSmooth(Vector2 from, Vector2 intermediary, Vector2 to, float t)
	{
		return (t >= 0.5f) ? Vector2.Lerp(intermediary, to, Mathf.SmoothStep(0f, 1f, t * 2f - 1f)) : Vector2.Lerp(from, intermediary, Mathf.SmoothStep(0f, 1f, t * 2f));
	}

	// Token: 0x060091C0 RID: 37312 RVA: 0x003DA268 File Offset: 0x003D8468
	public static Vector3 DoubleLerpSmooth(Vector3 from, Vector3 intermediary, Vector3 to, float t)
	{
		return (t >= 0.5f) ? Vector3.Lerp(intermediary, to, Mathf.SmoothStep(0f, 1f, t * 2f - 1f)) : Vector3.Lerp(from, intermediary, Mathf.SmoothStep(0f, 1f, t * 2f));
	}

	// Token: 0x060091C1 RID: 37313 RVA: 0x003DA2C8 File Offset: 0x003D84C8
	public static Vector2 VectorToCone(Vector2 source, float angleVariance)
	{
		return (Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * source.ToVector3ZUp(0f)).XY();
	}

	// Token: 0x060091C2 RID: 37314 RVA: 0x003DA2F8 File Offset: 0x003D84F8
	public static float ActualSign(float f)
	{
		if (f < 0f)
		{
			return -1f;
		}
		if (f > 0f)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x060091C3 RID: 37315 RVA: 0x003DA324 File Offset: 0x003D8524
	public static int AngleToQuadrant(float angle)
	{
		angle = (angle + 360f) % 360f;
		angle += 45f;
		angle %= 360f;
		int num = Mathf.FloorToInt(angle / 90f);
		return (3 - num + 2) % 4;
	}

	// Token: 0x060091C4 RID: 37316 RVA: 0x003DA368 File Offset: 0x003D8568
	public static int VectorToQuadrant(Vector2 inVec)
	{
		return BraveMathCollege.AngleToQuadrant(inVec.ToAngle());
	}

	// Token: 0x060091C5 RID: 37317 RVA: 0x003DA378 File Offset: 0x003D8578
	public static int VectorToOctant(Vector2 inVec)
	{
		float num = Mathf.Atan2(inVec.y, inVec.x) * 57.29578f;
		num = (num + 360f) % 360f;
		num += 22.5f;
		num %= 360f;
		int num2 = Mathf.FloorToInt(num / 45f);
		return (7 - num2 + 3) % 8;
	}

	// Token: 0x060091C6 RID: 37318 RVA: 0x003DA3D4 File Offset: 0x003D85D4
	public static int VectorToSextant(Vector2 inVec)
	{
		float num = Mathf.Atan2(inVec.y, inVec.x) * 57.29578f;
		num = (num + 360f) % 360f;
		num %= 360f;
		int num2 = Mathf.FloorToInt(num / 60f);
		return (5 - num2 + 2) % 6;
	}

	// Token: 0x060091C7 RID: 37319 RVA: 0x003DA428 File Offset: 0x003D8628
	public static int GreatestCommonDivisor(int a, int b)
	{
		return (b != 0) ? BraveMathCollege.GreatestCommonDivisor(b, a % b) : a;
	}

	// Token: 0x060091C8 RID: 37320 RVA: 0x003DA440 File Offset: 0x003D8640
	public static int AngleToOctant(float angle)
	{
		return (int)((472.5f - angle) / 45f) % 8;
	}

	// Token: 0x060091C9 RID: 37321 RVA: 0x003DA454 File Offset: 0x003D8654
	public static Vector2 ReflectVectorAcrossNormal(Vector2 vector, Vector2 normal)
	{
		float num = (Mathf.Atan2(normal.y, normal.x) - Mathf.Atan2(vector.y, vector.x)) * 57.29578f;
		return Quaternion.Euler(0f, 0f, 180f + 2f * num) * vector;
	}

	// Token: 0x060091CA RID: 37322 RVA: 0x003DA4BC File Offset: 0x003D86BC
	public static Vector2 CircleCenterFromThreePoints(Vector2 a, Vector2 b, Vector2 c)
	{
		float num = b.y - a.y;
		float num2 = b.x - a.x;
		float num3 = c.y - b.y;
		float num4 = c.x - b.x;
		float num5 = num / num2;
		float num6 = num3 / num4;
		float num7 = (num5 * num6 * (a.y - c.y) + num6 * (a.x + b.x) - num5 * (b.x + c.x)) / (2f * (num6 - num5));
		float num8 = -1f * (num7 - (a.x + b.x) / 2f) / num5 + (a.y + b.y) / 2f;
		return new Vector2(num7, num8);
	}

	// Token: 0x060091CB RID: 37323 RVA: 0x003DA5A0 File Offset: 0x003D87A0
	public static float QuantizeFloat(float input, float multiplesOf)
	{
		return Mathf.Round(input / multiplesOf) * multiplesOf;
	}

	// Token: 0x060091CC RID: 37324 RVA: 0x003DA5AC File Offset: 0x003D87AC
	public static float LinearToSmoothStepInterpolate(float from, float to, float t, int iterations)
	{
		float num = t;
		for (int i = 0; i < iterations; i++)
		{
			num = BraveMathCollege.LinearToSmoothStepInterpolate(from, to, num);
		}
		return num;
	}

	// Token: 0x060091CD RID: 37325 RVA: 0x003DA5D8 File Offset: 0x003D87D8
	public static float LinearToSmoothStepInterpolate(float from, float to, float t)
	{
		return Mathf.Lerp(Mathf.Lerp(from, to, t), Mathf.SmoothStep(from, to, t), t);
	}

	// Token: 0x060091CE RID: 37326 RVA: 0x003DA5F0 File Offset: 0x003D87F0
	public static float SmoothStepToLinearStepInterpolate(float from, float to, float t)
	{
		return Mathf.Lerp(Mathf.SmoothStep(from, to, t), Mathf.Lerp(from, to, t), t);
	}

	// Token: 0x060091CF RID: 37327 RVA: 0x003DA608 File Offset: 0x003D8808
	public static float HermiteInterpolation(float t)
	{
		return -t * t * t * 2f + t * t * 3f;
	}

	// Token: 0x060091D0 RID: 37328 RVA: 0x003DA620 File Offset: 0x003D8820
	public static bool LineSegmentRectangleIntersection(Vector2 p0, Vector2 p1, Vector2 rectMin, Vector2 rectMax, ref Vector2 result)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		Vector2 zero4 = Vector2.zero;
		bool flag = BraveMathCollege.LineSegmentIntersection(p0, p1, rectMin, rectMin.WithX(rectMax.x), ref zero);
		bool flag2 = BraveMathCollege.LineSegmentIntersection(p0, p1, rectMin.WithX(rectMax.x), rectMax, ref zero2);
		bool flag3 = BraveMathCollege.LineSegmentIntersection(p0, p1, rectMax, rectMin.WithY(rectMax.y), ref zero3);
		bool flag4 = BraveMathCollege.LineSegmentIntersection(p0, p1, rectMin, rectMin.WithY(rectMax.y), ref zero4);
		float num = float.MaxValue;
		bool flag5 = false;
		result = Vector2.zero;
		if (flag && Vector2.Distance(p0, zero) < num)
		{
			num = Vector2.Distance(p0, zero);
			result = zero;
			flag5 = true;
		}
		if (flag2 && Vector2.Distance(p0, zero2) < num)
		{
			num = Vector2.Distance(p0, zero2);
			result = zero2;
			flag5 = true;
		}
		if (flag3 && Vector2.Distance(p0, zero3) < num)
		{
			num = Vector2.Distance(p0, zero3);
			result = zero3;
			flag5 = true;
		}
		if (flag4 && Vector2.Distance(p0, zero4) < num)
		{
			num = Vector2.Distance(p0, zero4);
			result = zero4;
			flag5 = true;
		}
		return flag5;
	}

	// Token: 0x060091D1 RID: 37329 RVA: 0x003DA768 File Offset: 0x003D8968
	public static bool LineSegmentIntersection(Vector2 p0, Vector2 p1, Vector2 q0, Vector2 q1, ref Vector2 result)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		zero.x = p1.x - p0.x;
		zero.y = p1.y - p0.y;
		zero2.x = q1.x - q0.x;
		zero2.y = q1.y - q0.y;
		float num = (-zero.y * (p0.x - q0.x) + zero.x * (p0.y - q0.y)) / (-zero2.x * zero.y + zero.x * zero2.y);
		float num2 = (zero2.x * (p0.y - q0.y) - zero2.y * (p0.x - q0.x)) / (-zero2.x * zero.y + zero.x * zero2.y);
		result = Vector2.zero;
		if (num >= 0f && num <= 1f && num2 >= 0f && num2 <= 1f)
		{
			result.x = p0.x + num2 * zero.x;
			result.y = p0.y + num2 * zero.y;
			return true;
		}
		return false;
	}

	// Token: 0x060091D2 RID: 37330 RVA: 0x003DA8E8 File Offset: 0x003D8AE8
	public static Vector2 ClosestPointOnLineSegment(Vector2 p, Vector2 v, Vector2 w)
	{
		float sqrMagnitude = (w - v).sqrMagnitude;
		if ((double)sqrMagnitude == 0.0)
		{
			return v;
		}
		float num = Vector2.Dot(p - v, w - v) / sqrMagnitude;
		if (num < 0f)
		{
			return v;
		}
		if (num > 1f)
		{
			return w;
		}
		return v + num * (w - v);
	}

	// Token: 0x060091D3 RID: 37331 RVA: 0x003DA95C File Offset: 0x003D8B5C
	public static Vector2 ClosestPointOnRectangle(Vector2 point, Vector2 origin, Vector2 dimensions)
	{
		Vector2 vector = origin;
		Vector2 vector2 = new Vector2(origin.x + dimensions.x, origin.y);
		Vector2 vector3 = origin + dimensions;
		Vector2 vector4 = new Vector2(origin.x, origin.y + dimensions.y);
		Vector2 vector5 = BraveMathCollege.ClosestPointOnLineSegment(point, vector, vector2);
		float num = Vector2.Distance(point, vector5);
		Vector2 vector6 = vector5;
		vector5 = BraveMathCollege.ClosestPointOnLineSegment(point, vector2, vector3);
		float num2 = Vector2.Distance(point, vector5);
		if (num2 < num)
		{
			num = num2;
			vector6 = vector5;
		}
		vector5 = BraveMathCollege.ClosestPointOnLineSegment(point, vector3, vector4);
		num2 = Vector2.Distance(point, vector5);
		if (num2 < num)
		{
			num = num2;
			vector6 = vector5;
		}
		vector5 = BraveMathCollege.ClosestPointOnLineSegment(point, vector4, vector);
		num2 = Vector2.Distance(point, vector5);
		if (num2 < num)
		{
			vector6 = vector5;
		}
		return vector6;
	}

	// Token: 0x060091D4 RID: 37332 RVA: 0x003DAA38 File Offset: 0x003D8C38
	public static Vector2 ClosestPointOnPolygon(List<Vector2> polygon, Vector2 point)
	{
		Vector2 vector = Vector2.zero;
		float num = float.MaxValue;
		for (int i = 0; i < polygon.Count; i++)
		{
			Vector2 vector2 = polygon[i];
			Vector2 vector3 = polygon[(i + 1) % polygon.Count];
			Vector2 vector4 = BraveMathCollege.ClosestPointOnLineSegment(point, vector2, vector3);
			float num2 = Vector2.Distance(point, vector4);
			if (num2 < num)
			{
				num = num2;
				vector = vector4;
			}
		}
		return vector;
	}

	// Token: 0x060091D5 RID: 37333 RVA: 0x003DAAA8 File Offset: 0x003D8CA8
	public static float DistToRectangle(Vector2 point, Vector2 origin, Vector2 dimensions)
	{
		Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(point, origin, dimensions);
		return Vector2.Distance(point, vector);
	}

	// Token: 0x060091D6 RID: 37334 RVA: 0x003DAAC8 File Offset: 0x003D8CC8
	public static float DistBetweenRectangles(Vector2 o1, Vector2 d1, Vector2 o2, Vector2 d2)
	{
		Vector2 vector = Vector2.Min(o1, o2);
		Vector2 vector2 = Vector2.Max(o1 + d1, o2 + d2);
		Vector2 vector3 = vector2 - vector;
		float num = vector3.x - (d1.x + d2.x);
		float num2 = vector3.y - (d1.y + d2.y);
		if (num > 0f && num2 > 0f)
		{
			return Mathf.Sqrt(num * num + num2 * num2);
		}
		if (num > 0f)
		{
			return num;
		}
		if (num2 > 0f)
		{
			return num2;
		}
		return 0f;
	}

	// Token: 0x060091D7 RID: 37335 RVA: 0x003DAB74 File Offset: 0x003D8D74
	public static float ClampAngle360(float angleDeg)
	{
		angleDeg %= 360f;
		if (angleDeg < 0f)
		{
			angleDeg += 360f;
		}
		return angleDeg;
	}

	// Token: 0x060091D8 RID: 37336 RVA: 0x003DAB94 File Offset: 0x003D8D94
	public static float ClampAngle180(float angleDeg)
	{
		angleDeg %= 360f;
		if (angleDeg < -180f)
		{
			angleDeg += 360f;
		}
		else if (angleDeg > 180f)
		{
			angleDeg -= 360f;
		}
		return angleDeg;
	}

	// Token: 0x060091D9 RID: 37337 RVA: 0x003DABD0 File Offset: 0x003D8DD0
	public static float ClampAngle2Pi(float angleRad)
	{
		angleRad %= 6.2831855f;
		if (angleRad < 0f)
		{
			angleRad += 6.2831855f;
		}
		return angleRad;
	}

	// Token: 0x060091DA RID: 37338 RVA: 0x003DABF0 File Offset: 0x003D8DF0
	public static float ClampAnglePi(float angleRad)
	{
		angleRad %= 6.2831855f;
		if (angleRad < -3.1415927f)
		{
			angleRad += 6.2831855f;
		}
		else if (angleRad > 3.1415927f)
		{
			angleRad -= 6.2831855f;
		}
		return angleRad;
	}

	// Token: 0x060091DB RID: 37339 RVA: 0x003DAC2C File Offset: 0x003D8E2C
	public static float Atan2Degrees(float y, float x)
	{
		return Mathf.Atan2(y, x) * 57.29578f;
	}

	// Token: 0x060091DC RID: 37340 RVA: 0x003DAC3C File Offset: 0x003D8E3C
	public static float Atan2Degrees(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x) * 57.29578f;
	}

	// Token: 0x060091DD RID: 37341 RVA: 0x003DAC58 File Offset: 0x003D8E58
	public static float AbsAngleBetween(float a, float b)
	{
		return Mathf.Abs(BraveMathCollege.ClampAngle180(a - b));
	}

	// Token: 0x060091DE RID: 37342 RVA: 0x003DAC68 File Offset: 0x003D8E68
	public static Vector2 DegreesToVector(float angle, float magnitude = 1f)
	{
		float num = angle * 0.017453292f;
		return new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * magnitude;
	}

	// Token: 0x060091DF RID: 37343 RVA: 0x003DAC94 File Offset: 0x003D8E94
	public static float GetNearestAngle(float angle, float[] options)
	{
		if (options == null || options.Length == 0)
		{
			return angle;
		}
		int num = 0;
		float num2 = BraveMathCollege.AbsAngleBetween(angle, options[0]);
		for (int i = 1; i < options.Length; i++)
		{
			float num3 = BraveMathCollege.AbsAngleBetween(angle, options[i]);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return options[num];
	}

	// Token: 0x060091E0 RID: 37344 RVA: 0x003DACEC File Offset: 0x003D8EEC
	public static float EstimateBezierPathLength(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int divisions)
	{
		float num = 1f / (float)divisions;
		float num2 = 0f;
		for (int i = 0; i < divisions; i++)
		{
			Vector2 vector = BraveMathCollege.CalculateBezierPoint(num * (float)i, p0, p1, p2, p3);
			Vector2 vector2 = BraveMathCollege.CalculateBezierPoint(num * (float)(i + 1), p0, p1, p2, p3);
			num2 += (vector2 - vector).magnitude;
		}
		return num2;
	}

	// Token: 0x060091E1 RID: 37345 RVA: 0x003DAD50 File Offset: 0x003D8F50
	public static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float num4 = num3 * num;
		float num5 = num2 * t;
		Vector2 vector = num4 * p0;
		vector += 3f * num3 * t * p1;
		vector += 3f * num * num2 * p2;
		return vector + num5 * p3;
	}

	// Token: 0x060091E2 RID: 37346 RVA: 0x003DADC4 File Offset: 0x003D8FC4
	public static int LineCircleIntersections(Vector2 center, float radius, Vector2 p1, Vector2 p2, out Vector2 i1, out Vector2 i2)
	{
		float num = p2.x - p1.x;
		float num2 = p2.y - p1.y;
		float num3 = num * num + num2 * num2;
		float num4 = 2f * (num * (p1.x - center.x) + num2 * (p1.y - center.y));
		float num5 = (p1.x - center.x) * (p1.x - center.x) + (p1.y - center.y) * (p1.y - center.y) - radius * radius;
		float num6 = num4 * num4 - 4f * num3 * num5;
		if (num3 <= 1E-07f || num6 < 0f)
		{
			i1 = new Vector2(float.NaN, float.NaN);
			i2 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		float num7;
		if (num6 == 0f)
		{
			num7 = -num4 / (2f * num3);
			i1 = new Vector2(p1.x + num7 * num, p1.y + num7 * num2);
			i2 = new Vector2(float.NaN, float.NaN);
			return 1;
		}
		num7 = (float)(((double)(-(double)num4) + Math.Sqrt((double)num6)) / (double)(2f * num3));
		i1 = new Vector2(p1.x + num7 * num, p1.y + num7 * num2);
		num7 = (float)(((double)(-(double)num4) - Math.Sqrt((double)num6)) / (double)(2f * num3));
		i2 = new Vector2(p1.x + num7 * num, p1.y + num7 * num2);
		return 2;
	}

	// Token: 0x060091E3 RID: 37347 RVA: 0x003DAF6C File Offset: 0x003D916C
	public static int LineSegmentCircleIntersections(Vector2 center, float radius, Vector2 p1, Vector2 p2, out Vector2 i1, out Vector2 i2)
	{
		int num = BraveMathCollege.LineCircleIntersections(center, radius, p1, p2, out i1, out i2);
		if (num == 0)
		{
			i1 = new Vector2(float.NaN, float.NaN);
			i2 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		Vector2 vector = Vector2.Min(p1, p2);
		Vector2 vector2 = Vector2.Max(p1, p2);
		int num2 = 0;
		if (num >= 1)
		{
			if (vector.x <= i1.x && vector2.x >= i1.x && vector.y <= i1.y && vector2.y >= i1.y)
			{
				num2++;
			}
			else
			{
				i1 = new Vector2(float.NaN, float.NaN);
			}
		}
		if (num >= 2)
		{
			if (vector.x <= i2.x && vector2.x >= i2.x && vector.y <= i2.y && vector2.y >= i2.y)
			{
				num2++;
				if (num2 == 1)
				{
					i1 = i2;
					i2 = new Vector2(float.NaN, float.NaN);
				}
			}
			else
			{
				i2 = new Vector2(float.NaN, float.NaN);
			}
		}
		return num2;
	}

	// Token: 0x060091E4 RID: 37348 RVA: 0x003DB0C4 File Offset: 0x003D92C4
	public static Vector2 ClosestLineCircleIntersect(Vector2 center, float radius, Vector2 lineStart, Vector2 lineEnd)
	{
		Vector2 vector;
		Vector2 vector2;
		int num = BraveMathCollege.LineCircleIntersections(center, radius, lineStart, lineEnd, out vector, out vector2);
		if (num == 1)
		{
			return vector;
		}
		if (num == 2)
		{
			return (Vector2.Distance(vector, lineStart) >= Vector2.Distance(vector2, lineStart)) ? vector2 : vector;
		}
		return Vector2.zero;
	}

	// Token: 0x060091E5 RID: 37349 RVA: 0x003DB110 File Offset: 0x003D9310
	public static float SliceProbability(float chancePerSecond, float tickTime)
	{
		return 1f - Mathf.Pow(1f - chancePerSecond, tickTime);
	}

	// Token: 0x060091E6 RID: 37350 RVA: 0x003DB128 File Offset: 0x003D9328
	public static bool AABBContains(Vector2 min, Vector2 max, Vector2 point)
	{
		return point.x >= min.x && point.x <= max.x && point.y >= min.y && point.y <= max.y;
	}

	// Token: 0x060091E7 RID: 37351 RVA: 0x003DB184 File Offset: 0x003D9384
	public static float AABBDistance(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
	{
		Vector2 vector = new Vector2((bMin.x + bMax.x) / 2f, (bMin.y + bMax.y) / 2f);
		Vector2 vector2 = new Vector2((aMin.x + aMax.x) / 2f, (aMin.y + aMax.y) / 2f);
		if (vector.x < aMin.x)
		{
			vector.x = aMin.x;
		}
		if (vector.x > aMax.x)
		{
			vector.x = aMax.x;
		}
		if (vector.y < aMin.y)
		{
			vector.y = aMin.y;
		}
		if (vector.y > aMax.y)
		{
			vector.y = aMax.y;
		}
		if (vector2.x < aMin.x)
		{
			vector2.x = aMin.x;
		}
		if (vector2.x > aMax.x)
		{
			vector2.x = aMax.x;
		}
		if (vector2.y < aMin.y)
		{
			vector2.y = aMin.y;
		}
		if (vector2.y > aMax.y)
		{
			vector2.y = aMax.y;
		}
		return Vector2.Distance(vector, vector2);
	}

	// Token: 0x060091E8 RID: 37352 RVA: 0x003DB304 File Offset: 0x003D9504
	public static float AABBDistanceSquared(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
	{
		Vector2 vector = new Vector2((bMin.x + bMax.x) / 2f, (bMin.y + bMax.y) / 2f);
		Vector2 vector2 = new Vector2((aMin.x + aMax.x) / 2f, (aMin.y + aMax.y) / 2f);
		if (vector.x < aMin.x)
		{
			vector.x = aMin.x;
		}
		if (vector.x > aMax.x)
		{
			vector.x = aMax.x;
		}
		if (vector.y < aMin.y)
		{
			vector.y = aMin.y;
		}
		if (vector.y > aMax.y)
		{
			vector.y = aMax.y;
		}
		if (vector2.x < bMin.x)
		{
			vector2.x = bMin.x;
		}
		if (vector2.x > bMax.x)
		{
			vector2.x = bMax.x;
		}
		if (vector2.y < bMin.y)
		{
			vector2.y = bMin.y;
		}
		if (vector2.y > bMax.y)
		{
			vector2.y = bMax.y;
		}
		return Vector2.SqrMagnitude(vector - vector2);
	}

	// Token: 0x060091E9 RID: 37353 RVA: 0x003DB488 File Offset: 0x003D9688
	public static Vector2 GetPredictedPosition(Vector2 targetOrigin, Vector2 targetVelocity, float time)
	{
		return targetOrigin + targetVelocity * time;
	}

	// Token: 0x060091EA RID: 37354 RVA: 0x003DB498 File Offset: 0x003D9698
	public static Vector2 GetPredictedPosition(Vector2 targetOrigin, Vector2 targetVelocity, Vector2 aimOrigin, float firingSpeed)
	{
		float magnitude = targetVelocity.magnitude;
		if (magnitude < 1E-05f)
		{
			return targetOrigin;
		}
		Vector2 vector = aimOrigin - targetOrigin;
		float num = targetVelocity.ToAngle() - vector.ToAngle();
		float num2 = Mathf.Asin(magnitude * Mathf.Sin(num * 0.017453292f) / firingSpeed) * 57.29578f;
		if (float.IsNaN(num2))
		{
			return targetOrigin;
		}
		float num3 = BraveMathCollege.ClampAngle360((targetOrigin - aimOrigin).ToAngle());
		float num4 = BraveMathCollege.ClampAngle360(180f - num2 - num);
		if ((double)num4 < 0.0001 || num4 > 359.9999f)
		{
			return targetOrigin;
		}
		float num5 = Vector2.Distance(aimOrigin, targetOrigin) * Mathf.Sin(num2 * 0.017453292f) / Mathf.Sin(num4 * 0.017453292f) / magnitude;
		if (num5 < 0f)
		{
			return targetOrigin;
		}
		return aimOrigin + BraveMathCollege.DegreesToVector(num3 - num2, firingSpeed * num5);
	}

	// Token: 0x060091EB RID: 37355 RVA: 0x003DB584 File Offset: 0x003D9784
	public static bool NextPermutation(ref int[] numList)
	{
		int num = -1;
		for (int i = numList.Length - 2; i >= 0; i--)
		{
			if (numList[i] < numList[i + 1])
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return false;
		}
		int num2 = -1;
		for (int j = numList.Length - 1; j >= 0; j--)
		{
			if (numList[num] < numList[j])
			{
				num2 = j;
				break;
			}
		}
		int num3 = numList[num];
		numList[num] = numList[num2];
		numList[num2] = num3;
		int k = num + 1;
		int num4 = numList.Length - 1;
		while (k < num4)
		{
			num3 = numList[k];
			numList[k] = numList[num4];
			numList[num4] = num3;
			k++;
			num4--;
		}
		return true;
	}

	// Token: 0x060091EC RID: 37356 RVA: 0x003DB64C File Offset: 0x003D984C
	public static Vector2 ClampToBounds(Vector2 value, Vector2 min, Vector2 max)
	{
		return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
	}

	// Token: 0x060091ED RID: 37357 RVA: 0x003DB688 File Offset: 0x003D9888
	public static Vector2 ClampSafe(Vector2 value, float min, float max)
	{
		return new Vector2(BraveMathCollege.ClampSafe(value.x, min, max), BraveMathCollege.ClampSafe(value.y, min, max));
	}

	// Token: 0x060091EE RID: 37358 RVA: 0x003DB6AC File Offset: 0x003D98AC
	public static float ClampSafe(float value, float min, float max)
	{
		if (float.IsNaN(value))
		{
			return 0f;
		}
		return Mathf.Clamp(value, min, max);
	}

	// Token: 0x060091EF RID: 37359 RVA: 0x003DB6C8 File Offset: 0x003D98C8
	public static float WeightedAverage(float newValue, ref float prevAverage, ref int prevCount)
	{
		prevCount++;
		prevAverage = prevAverage * (((float)prevCount - 1f) / (float)prevCount) + newValue / (float)prevCount;
		return prevAverage;
	}

	// Token: 0x060091F0 RID: 37360 RVA: 0x003DB6EC File Offset: 0x003D98EC
	public static Vector2 WeightedAverage(Vector2 newValue, ref Vector2 prevAverage, ref int prevCount)
	{
		prevCount++;
		prevAverage = prevAverage * (((float)prevCount - 1f) / (float)prevCount) + newValue / (float)prevCount;
		return prevAverage;
	}

	// Token: 0x060091F1 RID: 37361 RVA: 0x003DB728 File Offset: 0x003D9928
	public static float MovingAverage(float avg, float newValue, int n)
	{
		if (avg == 0f)
		{
			return newValue;
		}
		float num = 1f / (float)n;
		return avg + num * (newValue - avg);
	}

	// Token: 0x060091F2 RID: 37362 RVA: 0x003DB754 File Offset: 0x003D9954
	public static Vector2 MovingAverage(Vector2 avg, Vector2 newValue, int n)
	{
		if (avg == Vector2.zero)
		{
			return newValue;
		}
		float num = 1f / (float)n;
		return avg + num * (newValue - avg);
	}

	// Token: 0x060091F3 RID: 37363 RVA: 0x003DB790 File Offset: 0x003D9990
	public static float MovingAverageSpeed(float movingAverage, float newSpeed, float newDeltaTime, float n)
	{
		if (newDeltaTime <= 0f)
		{
			return movingAverage;
		}
		if (movingAverage == 0f || newDeltaTime >= n)
		{
			return newSpeed;
		}
		float num = newDeltaTime / n;
		return movingAverage + num * (newSpeed - movingAverage);
	}

	// Token: 0x060091F4 RID: 37364 RVA: 0x003DB7CC File Offset: 0x003D99CC
	public static Vector3 LShapedMoveTowards(Vector3 current, Vector3 target, float maxDeltaX, float maxDeltaY)
	{
		if (Mathf.RoundToInt(current.x) != Mathf.RoundToInt(target.x) && Mathf.RoundToInt(current.y) != Mathf.RoundToInt(target.y))
		{
			if (target.y > current.y)
			{
				return Vector3.MoveTowards(current, target.WithX(current.x), maxDeltaX);
			}
			return Vector3.MoveTowards(current, target.WithY(current.y), maxDeltaY);
		}
		else
		{
			if (Mathf.RoundToInt(current.y) == Mathf.RoundToInt(target.y))
			{
				return Vector3.MoveTowards(current, target, maxDeltaX);
			}
			return Vector3.MoveTowards(current, target, maxDeltaY);
		}
	}

	// Token: 0x060091F5 RID: 37365 RVA: 0x003DB880 File Offset: 0x003D9A80
	public static bool IsAngleWithinSweepArea(float testAngle, float startAngle, float sweepAngle)
	{
		if (sweepAngle > 360f || sweepAngle < -360f)
		{
			return true;
		}
		float num = Mathf.Sign(sweepAngle);
		float num2 = BraveMathCollege.ClampAngle180(testAngle - startAngle);
		if (Mathf.Sign(num2) != num)
		{
			num2 += num * 360f;
		}
		if (num > 0f)
		{
			return num2 < sweepAngle;
		}
		return num2 > sweepAngle;
	}

	// Token: 0x060091F6 RID: 37366 RVA: 0x003DB8E0 File Offset: 0x003D9AE0
	public static Vector2 GetEllipsePoint(Vector2 center, float a, float b, float angle)
	{
		Vector2 vector = center;
		float num = BraveMathCollege.ClampAngle360(angle);
		float num2 = Mathf.Tan(num * 0.017453292f);
		float num3 = (float)((num < 90f || num >= 270f) ? 1 : (-1));
		float num4 = Mathf.Sqrt(b * b + a * a * (num2 * num2));
		vector.x += num3 * a * b / num4;
		vector.y += num3 * a * b * num2 / num4;
		return vector;
	}

	// Token: 0x060091F7 RID: 37367 RVA: 0x003DB964 File Offset: 0x003D9B64
	public static Vector2 GetEllipsePointSmooth(Vector2 center, float a, float b, float angle)
	{
		return center + Vector2.Scale(new Vector2(a, b), BraveMathCollege.DegreesToVector(angle, 1f));
	}

	// Token: 0x040099DA RID: 39386
	private static float[] LowDiscrepancyPseudoRandoms = new float[]
	{
		0.546f, 0.153f, 0.925f, 0.471f, 0.739f, 0.062f, 0.383f, 0.817f, 0.696f, 0.205f,
		0.554f, 0.847f, 0.075f, 0.639f, 0.261f, 0.938f, 0.617f, 0.183f, 0.304f, 0.795f
	};
}
