using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001824 RID: 6180
public static class BraveUtility
{
	// Token: 0x06009211 RID: 37393 RVA: 0x003DBC50 File Offset: 0x003D9E50
	public static void DrawDebugSquare(Vector2 min, Color color)
	{
		BraveUtility.DrawDebugSquare(min.x, min.x + 1f, min.y, min.y + 1f, color);
	}

	// Token: 0x06009212 RID: 37394 RVA: 0x003DBC80 File Offset: 0x003D9E80
	public static void DrawDebugSquare(Vector2 min, Vector2 max, Color color)
	{
		BraveUtility.DrawDebugSquare(min.x, max.x, min.y, max.y, color);
	}

	// Token: 0x06009213 RID: 37395 RVA: 0x003DBCA4 File Offset: 0x003D9EA4
	public static void DrawDebugSquare(float minX, float maxX, float minY, float maxY, Color color)
	{
		Debug.DrawLine(new Vector3(minX, minY, 0f), new Vector3(maxX, minY, 0f), color);
		Debug.DrawLine(new Vector3(minX, maxY, 0f), new Vector3(maxX, maxY, 0f), color);
		Debug.DrawLine(new Vector3(minX, minY, 0f), new Vector3(minX, maxY, 0f), color);
		Debug.DrawLine(new Vector3(maxX, minY, 0f), new Vector3(maxX, maxY, 0f), color);
	}

	// Token: 0x06009214 RID: 37396 RVA: 0x003DBD30 File Offset: 0x003D9F30
	public static void DrawDebugSquare(Vector2 min, Color color, float duration)
	{
		BraveUtility.DrawDebugSquare(min.x, min.x + 1f, min.y, min.y + 1f, color, duration);
	}

	// Token: 0x06009215 RID: 37397 RVA: 0x003DBD64 File Offset: 0x003D9F64
	public static void DrawDebugSquare(Vector2 min, Vector2 max, Color color, float duration)
	{
		BraveUtility.DrawDebugSquare(min.x, max.x, min.y, max.y, color, duration);
	}

	// Token: 0x06009216 RID: 37398 RVA: 0x003DBD8C File Offset: 0x003D9F8C
	public static void DrawDebugSquare(float minX, float maxX, float minY, float maxY, Color color, float duration)
	{
		Debug.DrawLine(new Vector3(minX, minY, 0f), new Vector3(maxX, minY, 0f), color, duration);
		Debug.DrawLine(new Vector3(minX, maxY, 0f), new Vector3(maxX, maxY, 0f), color, duration);
		Debug.DrawLine(new Vector3(minX, minY, 0f), new Vector3(minX, maxY, 0f), color, duration);
		Debug.DrawLine(new Vector3(maxX, minY, 0f), new Vector3(maxX, maxY, 0f), color, duration);
	}

	// Token: 0x06009217 RID: 37399 RVA: 0x003DBE20 File Offset: 0x003DA020
	public static Vector3 GetMousePosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(Vector3.back, Vector3.zero);
		float num;
		plane.Raycast(ray, out num);
		return ray.GetPoint(num);
	}

	// Token: 0x06009218 RID: 37400 RVA: 0x003DBE64 File Offset: 0x003DA064
	public static Vector3 ViewportToWorldpoint(Vector2 viewportPos, ViewportType viewportType)
	{
		if (viewportType == ViewportType.Camera)
		{
			Ray ray = Camera.main.ViewportPointToRay(viewportPos);
			Plane plane = new Plane(Vector3.back, Vector3.zero);
			float num;
			plane.Raycast(ray, out num);
			return ray.GetPoint(num);
		}
		if (viewportType == ViewportType.Gameplay)
		{
			Vector2 vector = BraveUtility.ScreenCenterWorldPoint();
			Vector2 vector2 = new Vector2(30f * (viewportPos.x - 0.5f), 16.875f * (viewportPos.y - 0.5f));
			float overrideZoomScale = GameManager.Instance.MainCameraController.OverrideZoomScale;
			if (overrideZoomScale != 1f && overrideZoomScale != 0f)
			{
				vector2 /= overrideZoomScale;
			}
			return vector + vector2;
		}
		throw new ArgumentException("Unknown viewport type: " + viewportType);
	}

	// Token: 0x06009219 RID: 37401 RVA: 0x003DBF44 File Offset: 0x003DA144
	public static Vector2 WorldPointToViewport(Vector3 worldPoint, ViewportType viewportType)
	{
		if (viewportType == ViewportType.Camera)
		{
			return Camera.main.WorldToViewportPoint(worldPoint);
		}
		if (viewportType == ViewportType.Gameplay)
		{
			Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
			Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
			Vector2 vector3 = vector2 - vector;
			return new Vector2((worldPoint.x - vector.x) / vector3.x, (worldPoint.y - vector.y) / vector3.y);
		}
		throw new ArgumentException("Unknown viewport type: " + viewportType);
	}

	// Token: 0x0600921A RID: 37402 RVA: 0x003DBFF8 File Offset: 0x003DA1F8
	public static Vector3 ScreenCenterWorldPoint()
	{
		return BraveUtility.ViewportToWorldpoint(new Vector2(0.5f, 0.5f), ViewportType.Camera);
	}

	// Token: 0x0600921B RID: 37403 RVA: 0x003DC010 File Offset: 0x003DA210
	public static bool PointIsVisible(Vector2 flatPoint, float percentBuffer, ViewportType viewportType)
	{
		Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), viewportType);
		Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), viewportType);
		Vector2 vector3 = (vector2 - vector) * percentBuffer;
		return flatPoint.x > vector.x - vector3.x && flatPoint.x < vector2.x + vector3.x && flatPoint.y > vector.y - vector3.y && flatPoint.y < vector2.y + vector3.y;
	}

	// Token: 0x0600921C RID: 37404 RVA: 0x003DC0D0 File Offset: 0x003DA2D0
	public static Vector3 GetMinimapViewportPosition(Vector2 pos)
	{
		float num = pos.x / (float)Screen.width;
		float num2 = pos.y / (float)Screen.height;
		num = (num - 0.5f) / BraveCameraUtility.GetRect().width + 0.5f;
		num2 = (num2 - 0.5f) / BraveCameraUtility.GetRect().height + 0.5f;
		return new Vector2(num, num2);
	}

	// Token: 0x0600921D RID: 37405 RVA: 0x003DC140 File Offset: 0x003DA340
	public static Vector2[] ResizeArray(Vector2[] a, int size)
	{
		Vector2[] array = new Vector2[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = a[i];
		}
		return array;
	}

	// Token: 0x0600921E RID: 37406 RVA: 0x003DC180 File Offset: 0x003DA380
	public static Vector2 GetClosestPoint(Vector2 a, Vector2 b, Vector2 p)
	{
		Vector2 vector = p - a;
		Vector2 vector2 = b - a;
		float num = Vector2.Dot(vector, vector2) / vector2.sqrMagnitude;
		return a + vector2 * num;
	}

	// Token: 0x0600921F RID: 37407 RVA: 0x003DC1BC File Offset: 0x003DA3BC
	public static bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
	{
		intersection = Vector2.zero;
		Vector2 vector = a2 - a1;
		Vector2 vector2 = b2 - b1;
		float num = vector.x * vector2.y - vector.y * vector2.x;
		if (num == 0f)
		{
			return false;
		}
		Vector2 vector3 = b1 - a1;
		float num2 = (vector3.x * vector2.y - vector3.y * vector2.x) / num;
		if (num2 < 0f || num2 > 1f)
		{
			return false;
		}
		float num3 = (vector3.x * vector.y - vector3.y * vector.x) / num;
		if (num3 < 0f || num3 > 1f)
		{
			return false;
		}
		intersection = a1 + num2 * vector;
		return true;
	}

	// Token: 0x06009220 RID: 37408 RVA: 0x003DC2AC File Offset: 0x003DA4AC
	public static bool LineIntersectsAABB(Vector2 l1, Vector2 l2, Vector2 bOrigin, Vector2 bSize, out Vector2 intersection)
	{
		intersection = default(Vector2);
		float num = float.MaxValue;
		Vector2 vector;
		if (BraveUtility.LineIntersectsLine(l1, l2, bOrigin, bOrigin + new Vector2(0f, bSize.y), out vector))
		{
			float sqrMagnitude = (l1 - vector).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				intersection = vector;
				num = sqrMagnitude;
			}
		}
		if (BraveUtility.LineIntersectsLine(l1, l2, bOrigin + new Vector2(0f, bSize.y), bOrigin + bSize, out vector))
		{
			float sqrMagnitude2 = (l1 - vector).sqrMagnitude;
			if (sqrMagnitude2 < num)
			{
				intersection = vector;
				num = sqrMagnitude2;
			}
		}
		if (BraveUtility.LineIntersectsLine(l1, l2, bOrigin + bSize, bOrigin + new Vector2(bSize.x, 0f), out vector))
		{
			float sqrMagnitude3 = (l1 - vector).sqrMagnitude;
			if (sqrMagnitude3 < num)
			{
				intersection = vector;
				num = sqrMagnitude3;
			}
		}
		if (BraveUtility.LineIntersectsLine(l1, l2, bOrigin + new Vector2(bSize.x, 0f), bOrigin, out vector))
		{
			float sqrMagnitude4 = (l1 - vector).sqrMagnitude;
			if (sqrMagnitude4 < num)
			{
				intersection = vector;
				num = sqrMagnitude4;
			}
		}
		return num != float.MaxValue;
	}

	// Token: 0x06009221 RID: 37409 RVA: 0x003DC408 File Offset: 0x003DA608
	public static bool GreaterThanAlongMajorAxis(Vector2 lhs, Vector2 rhs, Vector2 axis)
	{
		Vector2 majorAxis = BraveUtility.GetMajorAxis(axis);
		Vector2 vector = Vector2.Scale(lhs, majorAxis);
		Vector2 vector2 = Vector2.Scale(rhs, majorAxis);
		float num = lhs.x + lhs.y;
		float num2 = rhs.x + rhs.y;
		return num > num2;
	}

	// Token: 0x06009222 RID: 37410 RVA: 0x003DC454 File Offset: 0x003DA654
	public static Vector2 GetMajorAxis(Vector2 vector)
	{
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
		{
			return new Vector2(Mathf.Sign(vector.x), 0f);
		}
		return new Vector2(0f, Mathf.Sign(vector.y));
	}

	// Token: 0x06009223 RID: 37411 RVA: 0x003DC4AC File Offset: 0x003DA6AC
	public static IntVector2 GetMajorAxis(IntVector2 vector)
	{
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
		{
			return new IntVector2(Math.Sign(vector.x), 0);
		}
		return new IntVector2(0, Math.Sign(vector.y));
	}

	// Token: 0x06009224 RID: 37412 RVA: 0x003DC4FC File Offset: 0x003DA6FC
	public static IntVector2 GetIntMajorAxis(IntVector2 vector)
	{
		return BraveUtility.GetIntMajorAxis(vector.ToVector2());
	}

	// Token: 0x06009225 RID: 37413 RVA: 0x003DC50C File Offset: 0x003DA70C
	public static IntVector2 GetIntMajorAxis(Vector2 vector)
	{
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
		{
			return new IntVector2(Math.Sign(vector.x), 0);
		}
		return new IntVector2(0, Math.Sign(vector.y));
	}

	// Token: 0x06009226 RID: 37414 RVA: 0x003DC55C File Offset: 0x003DA75C
	public static Vector2 GetMinorAxis(Vector2 vector)
	{
		if (Mathf.Abs(vector.x) <= Mathf.Abs(vector.y))
		{
			return new Vector2(Mathf.Sign(vector.x), 0f);
		}
		return new Vector2(0f, Mathf.Sign(vector.y));
	}

	// Token: 0x06009227 RID: 37415 RVA: 0x003DC5B4 File Offset: 0x003DA7B4
	public static IntVector2 GetMinorAxis(IntVector2 vector)
	{
		if (Mathf.Abs(vector.x) <= Mathf.Abs(vector.y))
		{
			return new IntVector2(Math.Sign(vector.x), 0);
		}
		return new IntVector2(0, Math.Sign(vector.y));
	}

	// Token: 0x06009228 RID: 37416 RVA: 0x003DC604 File Offset: 0x003DA804
	public static IntVector2 GetIntMinorAxis(Vector2 vector)
	{
		if (Mathf.Abs(vector.x) <= Mathf.Abs(vector.y))
		{
			return new IntVector2(Math.Sign(vector.x), 0);
		}
		return new IntVector2(0, Math.Sign(vector.y));
	}

	// Token: 0x06009229 RID: 37417 RVA: 0x003DC654 File Offset: 0x003DA854
	public static Vector2 GetPerp(Vector2 v)
	{
		return new Vector2(-v.y, v.x);
	}

	// Token: 0x0600922A RID: 37418 RVA: 0x003DC66C File Offset: 0x003DA86C
	public static Vector2 QuantizeVector(Vector2 vec)
	{
		int num = ((!(PhysicsEngine.Instance == null)) ? PhysicsEngine.Instance.PixelsPerUnit : 16);
		return BraveUtility.QuantizeVector(vec, (float)num);
	}

	// Token: 0x0600922B RID: 37419 RVA: 0x003DC6A4 File Offset: 0x003DA8A4
	public static Vector2 QuantizeVector(Vector2 vec, float unitsPerUnit)
	{
		return new Vector2(Mathf.Round(vec.x * unitsPerUnit), Mathf.Round(vec.y * unitsPerUnit)) / unitsPerUnit;
	}

	// Token: 0x0600922C RID: 37420 RVA: 0x003DC6D0 File Offset: 0x003DA8D0
	public static Vector3 QuantizeVector(Vector3 vec)
	{
		return BraveUtility.QuantizeVector(vec, (float)PhysicsEngine.Instance.PixelsPerUnit);
	}

	// Token: 0x0600922D RID: 37421 RVA: 0x003DC6E4 File Offset: 0x003DA8E4
	public static Vector3 QuantizeVector(Vector3 vec, float unitsPerUnit)
	{
		return new Vector3(Mathf.Round(vec.x * unitsPerUnit), Mathf.Round(vec.y * unitsPerUnit), Mathf.Round(vec.z * unitsPerUnit)) / unitsPerUnit;
	}

	// Token: 0x0600922E RID: 37422 RVA: 0x003DC71C File Offset: 0x003DA91C
	public static int GCD(int a, int b)
	{
		while (b != 0)
		{
			int num = a % b;
			a = b;
			b = num;
		}
		return a;
	}

	// Token: 0x0600922F RID: 37423 RVA: 0x003DC740 File Offset: 0x003DA940
	public static int GetTileMapLayerByName(string name, tk2dTileMap tileMap)
	{
		for (int i = 0; i < tileMap.data.tileMapLayers.Count; i++)
		{
			if (tileMap.data.tileMapLayers[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06009230 RID: 37424 RVA: 0x003DC794 File Offset: 0x003DA994
	public static T GetClosestToPosition<T>(List<T> sources, Vector2 pos, params T[] excluded) where T : BraveBehaviour
	{
		return BraveUtility.GetClosestToPosition<T>(sources, pos, null, excluded);
	}

	// Token: 0x06009231 RID: 37425 RVA: 0x003DC7A0 File Offset: 0x003DA9A0
	public static T GetClosestToPosition<T>(List<T> sources, Vector2 pos, Func<T, bool> isValid, params T[] excluded) where T : BraveBehaviour
	{
		return BraveUtility.GetClosestToPosition<T>(sources, pos, isValid, -1f, excluded);
	}

	// Token: 0x06009232 RID: 37426 RVA: 0x003DC7B0 File Offset: 0x003DA9B0
	public static T GetClosestToPosition<T>(List<T> sources, Vector2 pos, Func<T, bool> isValid, float maxDistance, params T[] excluded) where T : BraveBehaviour
	{
		T t = (T)((object)null);
		float num = float.MaxValue;
		if (sources == null)
		{
			return t;
		}
		for (int i = 0; i < sources.Count; i++)
		{
			if (sources[i])
			{
				if (excluded == null || excluded.Length >= sources.Count || Array.IndexOf<T>(excluded, sources[i]) == -1)
				{
					if (isValid == null || isValid(sources[i]))
					{
						T t2 = sources[i];
						float num2;
						if (t2.sprite != null)
						{
							T t3 = sources[i];
							num2 = Vector2.SqrMagnitude(t3.sprite.WorldCenter - pos);
						}
						else
						{
							T t4 = sources[i];
							num2 = Vector2.SqrMagnitude(t4.transform.position.XY() - pos);
						}
						if (maxDistance <= 0f || num2 <= maxDistance)
						{
							if (num2 < num)
							{
								t = sources[i];
								num = num2;
							}
						}
					}
				}
			}
		}
		return t;
	}

	// Token: 0x06009233 RID: 37427 RVA: 0x003DC8F8 File Offset: 0x003DAAF8
	public static T[][] MultidimensionalArrayResize<T>(T[][] original, int oldWidth, int oldHeight, int newWidth, int newHeight)
	{
		T[][] array = new T[newWidth][];
		for (int i = 0; i < newWidth; i++)
		{
			array[i] = new T[newHeight];
		}
		int num = Mathf.Min(oldWidth, newWidth);
		int num2 = Mathf.Min(oldHeight, newHeight);
		for (int j = 0; j < num; j++)
		{
			for (int k = 0; k < num2; k++)
			{
				array[j][k] = original[j][k];
			}
		}
		return array;
	}

	// Token: 0x06009234 RID: 37428 RVA: 0x003DC97C File Offset: 0x003DAB7C
	public static T[,] MultidimensionalArrayResize<T>(T[,] original, int rows, int cols)
	{
		T[,] array = new T[rows, cols];
		int num = Mathf.Min(rows, original.GetLength(0));
		int num2 = Mathf.Min(cols, original.GetLength(1));
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i, j] = original[i, j];
			}
		}
		return array;
	}

	// Token: 0x06009235 RID: 37429 RVA: 0x003DC9E8 File Offset: 0x003DABE8
	public static int[] ParsePageNums(string str)
	{
		string[] array = str.Split(new char[] { ',' });
		List<int> list = new List<int>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			int num;
			if (int.TryParse(text, out num))
			{
				list.Add(num);
			}
			else
			{
				string[] array2 = text.Split(new char[] { '-' });
				int num2;
				int num3;
				if (array2.Length > 1 && int.TryParse(array2[0], out num2) && int.TryParse(array2[1], out num3) && num3 >= num2)
				{
					for (int j = num2; j <= num3; j++)
					{
						list.Add(j);
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06009236 RID: 37430 RVA: 0x003DCAB4 File Offset: 0x003DACB4
	public static int EnumFlagsContains(uint data, uint valToFind)
	{
		if ((data & valToFind) == valToFind)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06009237 RID: 37431 RVA: 0x003DCAC4 File Offset: 0x003DACC4
	public static void Assert(bool assert, string s, bool pauseEditor = false)
	{
		if (assert)
		{
			BraveUtility.Log(s, Color.red, BraveUtility.LogVerbosity.IMPORTANT);
		}
	}

	// Token: 0x06009238 RID: 37432 RVA: 0x003DCAD8 File Offset: 0x003DACD8
	public static void Log(string s, Color c, BraveUtility.LogVerbosity v = BraveUtility.LogVerbosity.VERBOSE)
	{
	}

	// Token: 0x06009239 RID: 37433 RVA: 0x003DCADC File Offset: 0x003DACDC
	public static List<T> GenerationShuffle<T>(this List<T> input)
	{
		for (int i = input.Count - 1; i > 1; i--)
		{
			int num = BraveRandom.GenerationRandomRange(0, i);
			T t = input[i];
			input[i] = input[num];
			input[num] = t;
		}
		return input;
	}

	// Token: 0x0600923A RID: 37434 RVA: 0x003DCB2C File Offset: 0x003DAD2C
	public static List<T> Shuffle<T>(this List<T> input)
	{
		for (int i = input.Count - 1; i > 1; i--)
		{
			int num = UnityEngine.Random.Range(0, i);
			T t = input[i];
			input[i] = input[num];
			input[num] = t;
		}
		return input;
	}

	// Token: 0x0600923B RID: 37435 RVA: 0x003DCB7C File Offset: 0x003DAD7C
	public static List<T> SafeShuffle<T>(this List<T> input)
	{
		System.Random random = new System.Random();
		for (int i = input.Count - 1; i > 1; i--)
		{
			int num = random.Next(i);
			T t = input[i];
			input[i] = input[num];
			input[num] = t;
		}
		return input;
	}

	// Token: 0x0600923C RID: 37436 RVA: 0x003DCBD0 File Offset: 0x003DADD0
	public static T RandomElement<T>(List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600923D RID: 37437 RVA: 0x003DCBE4 File Offset: 0x003DADE4
	public static T RandomElement<T>(T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	// Token: 0x0600923E RID: 37438 RVA: 0x003DCBF8 File Offset: 0x003DADF8
	public static bool RandomBool()
	{
		return UnityEngine.Random.value >= 0.5f;
	}

	// Token: 0x0600923F RID: 37439 RVA: 0x003DCC0C File Offset: 0x003DAE0C
	public static float RandomSign()
	{
		return (float)((UnityEngine.Random.value <= 0.5f) ? (-1) : 1);
	}

	// Token: 0x06009240 RID: 37440 RVA: 0x003DCC28 File Offset: 0x003DAE28
	public static float RandomAngle()
	{
		return UnityEngine.Random.Range(0f, 360f);
	}

	// Token: 0x06009241 RID: 37441 RVA: 0x003DCC3C File Offset: 0x003DAE3C
	public static Vector2 RandomVector2(Vector2 min, Vector2 max)
	{
		return new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
	}

	// Token: 0x06009242 RID: 37442 RVA: 0x003DCC6C File Offset: 0x003DAE6C
	public static Vector2 RandomVector2(Vector2 min, Vector2 max, Vector2 padding)
	{
		if (padding.x < 0f && padding.y < 0f)
		{
			if (BraveUtility.RandomBool())
			{
				padding.x *= -1f;
			}
			else
			{
				padding.y *= -1f;
			}
		}
		float num;
		if (padding.x >= 0f)
		{
			num = UnityEngine.Random.Range(min.x + padding.x, max.x - padding.x);
		}
		else
		{
			num = ((!BraveUtility.RandomBool()) ? UnityEngine.Random.Range(max.x + padding.x, max.x) : UnityEngine.Random.Range(min.x, min.x - padding.x));
		}
		float num2;
		if (padding.y >= 0f)
		{
			num2 = UnityEngine.Random.Range(min.y + padding.y, max.y - padding.y);
		}
		else
		{
			num2 = ((!BraveUtility.RandomBool()) ? UnityEngine.Random.Range(max.y + padding.y, max.y) : UnityEngine.Random.Range(min.y, min.y - padding.y));
		}
		return new Vector2(num, num2);
	}

	// Token: 0x06009243 RID: 37443 RVA: 0x003DCDD8 File Offset: 0x003DAFD8
	public static void RandomizeList<T>(List<T> list, int startIndex = 0, int length = -1)
	{
		int num = ((length >= 0) ? (startIndex + length) : list.Count);
		for (int i = startIndex; i < num - 1; i++)
		{
			int num2 = UnityEngine.Random.Range(i + 1, num);
			T t = list[i];
			list[i] = list[num2];
			list[num2] = t;
		}
	}

	// Token: 0x06009244 RID: 37444 RVA: 0x003DCE38 File Offset: 0x003DB038
	public static void RandomizeArray<T>(T[] array, int startIndex = 0, int length = -1)
	{
		int num = ((length >= 0) ? (startIndex + length) : array.Length);
		for (int i = startIndex; i < num - 1; i++)
		{
			int num2 = UnityEngine.Random.Range(i + 1, num);
			T t = array[i];
			array[i] = array[num2];
			array[num2] = t;
		}
	}

	// Token: 0x170015DE RID: 5598
	// (get) Token: 0x06009245 RID: 37445 RVA: 0x003DCE94 File Offset: 0x003DB094
	public static bool isLoadingLevel
	{
		get
		{
			return Application.isLoadingLevel;
		}
	}

	// Token: 0x06009246 RID: 37446 RVA: 0x003DCE9C File Offset: 0x003DB09C
	public static void DrawDebugSquare(IntVector2 pos, Color col)
	{
		Debug.DrawLine(pos.ToVector2(), pos.ToVector2() + Vector2.up, col, 1000f);
		Debug.DrawLine(pos.ToVector2(), pos.ToVector2() + Vector2.right, col, 1000f);
		Debug.DrawLine(pos.ToVector2() + Vector2.up, pos.ToVector2() + Vector2.right + Vector2.up, col, 1000f);
		Debug.DrawLine(pos.ToVector2() + Vector2.right, pos.ToVector2() + Vector2.right + Vector2.up, col, 1000f);
	}

	// Token: 0x06009247 RID: 37447 RVA: 0x003DCF88 File Offset: 0x003DB188
	private static string ColorToHex(Color col)
	{
		float num = col.r * 255f;
		float num2 = col.g * 255f;
		float num3 = col.b * 255f;
		string hex = BraveUtility.GetHex(Mathf.FloorToInt(num / 16f));
		string hex2 = BraveUtility.GetHex(Mathf.RoundToInt(num % 16f));
		string hex3 = BraveUtility.GetHex(Mathf.FloorToInt(num2 / 16f));
		string hex4 = BraveUtility.GetHex(Mathf.RoundToInt(num2 % 16f));
		string hex5 = BraveUtility.GetHex(Mathf.FloorToInt(num3 / 16f));
		string hex6 = BraveUtility.GetHex(Mathf.RoundToInt(num3 % 16f));
		return string.Concat(new string[] { hex, hex2, hex3, hex4, hex5, hex6 });
	}

	// Token: 0x06009248 RID: 37448 RVA: 0x003DD05C File Offset: 0x003DB25C
	public static string ColorToHexWithAlpha(Color col)
	{
		float num = col.r * 255f;
		float num2 = col.g * 255f;
		float num3 = col.b * 255f;
		float num4 = col.a * 255f;
		string hex = BraveUtility.GetHex(Mathf.FloorToInt(num / 16f));
		string hex2 = BraveUtility.GetHex(Mathf.RoundToInt(num % 16f));
		string hex3 = BraveUtility.GetHex(Mathf.FloorToInt(num2 / 16f));
		string hex4 = BraveUtility.GetHex(Mathf.RoundToInt(num2 % 16f));
		string hex5 = BraveUtility.GetHex(Mathf.FloorToInt(num3 / 16f));
		string hex6 = BraveUtility.GetHex(Mathf.RoundToInt(num3 % 16f));
		string hex7 = BraveUtility.GetHex(Mathf.FloorToInt(num4 / 16f));
		string hex8 = BraveUtility.GetHex(Mathf.RoundToInt(num4 % 16f));
		return string.Concat(new string[] { hex, hex2, hex3, hex4, hex5, hex6, hex7, hex8 });
	}

	// Token: 0x06009249 RID: 37449 RVA: 0x003DD170 File Offset: 0x003DB370
	public static void AssignPositionalSoundTracking(GameObject obj)
	{
		if (obj.GetComponent<AkGameObj>() == null)
		{
		}
	}

	// Token: 0x0600924A RID: 37450 RVA: 0x003DD184 File Offset: 0x003DB384
	public static bool DX11Supported()
	{
		return SystemInfo.graphicsShaderLevel >= 50;
	}

	// Token: 0x0600924B RID: 37451 RVA: 0x003DD194 File Offset: 0x003DB394
	private static string GetHex(int d)
	{
		d = Mathf.Min(15, Mathf.Max(0, d));
		string text = "0123456789ABCDEF";
		return string.Empty + text[d];
	}

	// Token: 0x0600924C RID: 37452 RVA: 0x003DD1D0 File Offset: 0x003DB3D0
	public static string DecrementString(string str)
	{
		string text;
		string text2;
		BraveUtility.SplitNumericSuffix(str, out text, out text2);
		if (text2.Length == 0)
		{
			return str;
		}
		int num = int.Parse(text2);
		int num2 = Mathf.Max(0, num - 1);
		return text + num2.ToString("X" + text2.Length);
	}

	// Token: 0x0600924D RID: 37453 RVA: 0x003DD228 File Offset: 0x003DB428
	public static string IncrementString(string str)
	{
		string text;
		string text2;
		BraveUtility.SplitNumericSuffix(str, out text, out text2);
		if (text2.Length == 0)
		{
			return str;
		}
		int num = int.Parse(text2);
		int num2 = Mathf.Max(0, num + 1);
		return text + num2.ToString("X" + text2.Length);
	}

	// Token: 0x0600924E RID: 37454 RVA: 0x003DD280 File Offset: 0x003DB480
	public static void SplitNumericSuffix(string str, out string baseStr, out string suffixStr)
	{
		int num = 0;
		for (int i = str.Length - 1; i >= 0; i--)
		{
			if (!char.IsDigit(str[i]))
			{
				num = i + 1;
				break;
			}
		}
		if (num >= str.Length)
		{
			baseStr = str;
			suffixStr = string.Empty;
			return;
		}
		baseStr = str.Substring(0, num);
		suffixStr = str.Substring(num);
	}

	// Token: 0x0600924F RID: 37455 RVA: 0x003DD2EC File Offset: 0x003DB4EC
	public static List<int> GetPathCorners(List<IntVector2> path)
	{
		List<int> list = new List<int>();
		for (int i = 1; i < path.Count - 1; i++)
		{
			IntVector2 intVector = path[i - 1];
			IntVector2 intVector2 = path[i];
			IntVector2 intVector3 = path[i + 1];
			IntVector2 intVector4 = intVector2 - intVector;
			IntVector2 intVector5 = intVector3 - intVector2;
			if (intVector4 != intVector5)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x06009250 RID: 37456 RVA: 0x003DD360 File Offset: 0x003DB560
	public static IEnumerable<T> Zip<A, B, T>(this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
	{
		if (seqA == null)
		{
			throw new ArgumentNullException("seqA");
		}
		if (seqB == null)
		{
			throw new ArgumentNullException("seqB");
		}
		using (IEnumerator<A> iteratorA = seqA.GetEnumerator())
		{
			using (IEnumerator<B> iteratorB = seqB.GetEnumerator())
			{
				while (iteratorA.MoveNext() && iteratorB.MoveNext())
				{
					yield return func(iteratorA.Current, iteratorB.Current);
				}
			}
		}
		yield break;
	}

	// Token: 0x06009251 RID: 37457 RVA: 0x003DD394 File Offset: 0x003DB594
	public static int GetNthIndexOf(string s, char t, int n)
	{
		int num = 0;
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == t)
			{
				num++;
				if (num == n)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06009252 RID: 37458 RVA: 0x003DD3D8 File Offset: 0x003DB5D8
	public static void Swap<T>(ref T v1, ref T v2)
	{
		T t = v1;
		v1 = v2;
		v2 = t;
	}

	// Token: 0x06009253 RID: 37459 RVA: 0x003DD400 File Offset: 0x003DB600
	public static Color GetRainbowLerp(float t)
	{
		t %= 1f;
		t *= 6f;
		if (t < 1f)
		{
			return Color.Lerp(Color.red, new Color(1f, 0.5f, 0f), t % 1f);
		}
		if (t < 2f)
		{
			return Color.Lerp(new Color(1f, 0.5f, 0f), Color.yellow, t % 1f);
		}
		if (t < 3f)
		{
			return Color.Lerp(Color.yellow, Color.green, t % 1f);
		}
		if (t < 4f)
		{
			return Color.Lerp(Color.green, Color.blue, t % 1f);
		}
		if (t < 5f)
		{
			return Color.Lerp(Color.blue, new Color(0.5f, 0f, 1f), t % 1f);
		}
		if (t < 6f)
		{
			return Color.Lerp(new Color(0.5f, 0f, 1f), Color.red, t % 1f);
		}
		return Color.red;
	}

	// Token: 0x06009254 RID: 37460 RVA: 0x003DD52C File Offset: 0x003DB72C
	public static Color GetRainbowColor(int index)
	{
		switch (index)
		{
		case 0:
			return Color.red;
		case 1:
			return new Color(1f, 0.5f, 0f, 1f);
		case 2:
			return Color.yellow;
		case 3:
			return Color.green;
		case 4:
			return Color.blue;
		case 5:
			return Color.magenta;
		case 6:
			return new Color(0.5f, 0f, 1f);
		case 7:
			return Color.grey;
		case 8:
			return Color.white;
		default:
			return Color.white;
		}
	}

	// Token: 0x06009255 RID: 37461 RVA: 0x003DD5C8 File Offset: 0x003DB7C8
	public static T[] AppendArray<T>(T[] oldArray, T newElement)
	{
		T[] array = new T[oldArray.Length + 1];
		Array.Copy(oldArray, array, oldArray.Length);
		array[array.Length - 1] = newElement;
		return array;
	}

	// Token: 0x06009256 RID: 37462 RVA: 0x003DD5F8 File Offset: 0x003DB7F8
	public static int SequentialRandomRange(int min, int max, int lastValue, int? maxDistFromLast = null, bool excludeLastValue = false)
	{
		if (maxDistFromLast != null)
		{
			min = Mathf.Max(min, lastValue - maxDistFromLast.Value);
			max = Mathf.Min(max, lastValue + maxDistFromLast.Value + 1);
		}
		if (excludeLastValue)
		{
			max--;
		}
		int num = UnityEngine.Random.Range(min, max);
		if (excludeLastValue && num >= lastValue)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06009257 RID: 37463 RVA: 0x003DD65C File Offset: 0x003DB85C
	public static int SmartListResizer(int currentSize, int desiredSize, int minGrowingSize = 100, int forceMultipleOf = 0)
	{
		int num;
		if (currentSize == 0)
		{
			num = desiredSize;
		}
		else if (currentSize < minGrowingSize && desiredSize < minGrowingSize)
		{
			num = minGrowingSize;
		}
		else if (desiredSize < currentSize * 2)
		{
			num = currentSize * 2;
		}
		else
		{
			num = desiredSize + currentSize;
		}
		if (forceMultipleOf > 0 && num % forceMultipleOf > 0)
		{
			num += forceMultipleOf - num % forceMultipleOf;
		}
		return num;
	}

	// Token: 0x06009258 RID: 37464 RVA: 0x003DD6BC File Offset: 0x003DB8BC
	public static void EnableEmission(ParticleSystem ps, bool enabled)
	{
		ps.emission.enabled = enabled;
	}

	// Token: 0x06009259 RID: 37465 RVA: 0x003DD6D8 File Offset: 0x003DB8D8
	public static float GetEmissionRate(ParticleSystem ps)
	{
		return ps.emission.rate.constant;
	}

	// Token: 0x0600925A RID: 37466 RVA: 0x003DD6FC File Offset: 0x003DB8FC
	public static void SetEmissionRate(ParticleSystem ps, float emissionRate)
	{
		ps.emission.rate = emissionRate;
	}

	// Token: 0x040099EA RID: 39402
	private const float c_screenWidthTiles = 30f;

	// Token: 0x040099EB RID: 39403
	private const float c_screenHeightTiles = 16.875f;

	// Token: 0x040099EC RID: 39404
	public static BraveUtility.LogVerbosity verbosity = BraveUtility.LogVerbosity.IMPORTANT;

	// Token: 0x02001825 RID: 6181
	public enum LogVerbosity
	{
		// Token: 0x040099EE RID: 39406
		NONE,
		// Token: 0x040099EF RID: 39407
		IMPORTANT,
		// Token: 0x040099F0 RID: 39408
		CHATTY,
		// Token: 0x040099F1 RID: 39409
		VERBOSE
	}
}
