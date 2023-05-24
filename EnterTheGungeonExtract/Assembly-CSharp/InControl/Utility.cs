using System;
using System.IO;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x02000811 RID: 2065
	public static class Utility
	{
		// Token: 0x06002BC2 RID: 11202 RVA: 0x000DDE78 File Offset: 0x000DC078
		public static void DrawCircleGizmo(Vector2 center, float radius)
		{
			Vector2 vector = Utility.circleVertexList[0] * radius + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(vector, vector = Utility.circleVertexList[i] * radius + center);
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000DDEEC File Offset: 0x000DC0EC
		public static void DrawCircleGizmo(Vector2 center, float radius, Color color)
		{
			Gizmos.color = color;
			Utility.DrawCircleGizmo(center, radius);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000DDEFC File Offset: 0x000DC0FC
		public static void DrawOvalGizmo(Vector2 center, Vector2 size)
		{
			Vector2 vector = size / 2f;
			Vector2 vector2 = Vector2.Scale(Utility.circleVertexList[0], vector) + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(vector2, vector2 = Vector2.Scale(Utility.circleVertexList[i], vector) + center);
			}
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000DDF7C File Offset: 0x000DC17C
		public static void DrawOvalGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawOvalGizmo(center, size);
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000DDF8C File Offset: 0x000DC18C
		public static void DrawRectGizmo(Rect rect)
		{
			Vector3 vector = new Vector3(rect.xMin, rect.yMin);
			Vector3 vector2 = new Vector3(rect.xMax, rect.yMin);
			Vector3 vector3 = new Vector3(rect.xMax, rect.yMax);
			Vector3 vector4 = new Vector3(rect.xMin, rect.yMax);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000DE00C File Offset: 0x000DC20C
		public static void DrawRectGizmo(Rect rect, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(rect);
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000DE01C File Offset: 0x000DC21C
		public static void DrawRectGizmo(Vector2 center, Vector2 size)
		{
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			Vector3 vector = new Vector3(center.x - num, center.y - num2);
			Vector3 vector2 = new Vector3(center.x + num, center.y - num2);
			Vector3 vector3 = new Vector3(center.x + num, center.y + num2);
			Vector3 vector4 = new Vector3(center.x - num, center.y + num2);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000DE0CC File Offset: 0x000DC2CC
		public static void DrawRectGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(center, size);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000DE0DC File Offset: 0x000DC2DC
		public static bool GameObjectIsCulledOnCurrentCamera(GameObject gameObject)
		{
			return (Camera.current.cullingMask & (1 << gameObject.layer)) == 0;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000DE0F8 File Offset: 0x000DC2F8
		public static Color MoveColorTowards(Color color0, Color color1, float maxDelta)
		{
			float num = Mathf.MoveTowards(color0.r, color1.r, maxDelta);
			float num2 = Mathf.MoveTowards(color0.g, color1.g, maxDelta);
			float num3 = Mathf.MoveTowards(color0.b, color1.b, maxDelta);
			float num4 = Mathf.MoveTowards(color0.a, color1.a, maxDelta);
			return new Color(num, num2, num3, num4);
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000DE164 File Offset: 0x000DC364
		public static float ApplyDeadZone(float value, float lowerDeadZone, float upperDeadZone)
		{
			if (value < 0f)
			{
				if (value > -lowerDeadZone)
				{
					return 0f;
				}
				if (value < -upperDeadZone)
				{
					return -1f;
				}
				return (value + lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
			else
			{
				if (value < lowerDeadZone)
				{
					return 0f;
				}
				if (value > upperDeadZone)
				{
					return 1f;
				}
				return (value - lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000DE1C4 File Offset: 0x000DC3C4
		public static Vector2 ApplySeparateDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			Vector2 vector = new Vector2(Utility.ApplyDeadZone(x, lowerDeadZone, upperDeadZone), Utility.ApplyDeadZone(y, lowerDeadZone, upperDeadZone));
			return vector.normalized;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000DE1F0 File Offset: 0x000DC3F0
		public static Vector2 ApplyCircularDeadZone(Vector2 v, float lowerDeadZone, float upperDeadZone)
		{
			float magnitude = v.magnitude;
			if (magnitude < lowerDeadZone)
			{
				return Vector2.zero;
			}
			if (magnitude > upperDeadZone)
			{
				return v.normalized;
			}
			return v.normalized * ((magnitude - lowerDeadZone) / (upperDeadZone - lowerDeadZone));
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000DE234 File Offset: 0x000DC434
		public static Vector2 ApplyCircularDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			return Utility.ApplyCircularDeadZone(new Vector2(x, y), lowerDeadZone, upperDeadZone);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000DE244 File Offset: 0x000DC444
		public static float ApplySmoothing(float thisValue, float lastValue, float deltaTime, float sensitivity)
		{
			if (Utility.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			float num = deltaTime * sensitivity * 100f;
			if (Utility.IsNotZero(thisValue) && Mathf.Sign(lastValue) != Mathf.Sign(thisValue))
			{
				lastValue = 0f;
			}
			return Mathf.MoveTowards(lastValue, thisValue, num);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000DE298 File Offset: 0x000DC498
		public static float ApplySnapping(float value, float threshold)
		{
			if (value < -threshold)
			{
				return -1f;
			}
			if (value > threshold)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000DE2BC File Offset: 0x000DC4BC
		internal static bool TargetIsButton(InputControlType target)
		{
			return (target >= InputControlType.Action1 && target <= InputControlType.Action12) || (target >= InputControlType.Button0 && target <= InputControlType.Button19);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000DE2EC File Offset: 0x000DC4EC
		internal static bool TargetIsStandard(InputControlType target)
		{
			return (target >= InputControlType.LeftStickUp && target <= InputControlType.Action12) || (target >= InputControlType.Command && target <= InputControlType.DPadY);
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000DE31C File Offset: 0x000DC51C
		internal static bool TargetIsAlias(InputControlType target)
		{
			return target >= InputControlType.Command && target <= InputControlType.DPadY;
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000DE338 File Offset: 0x000DC538
		public static string ReadFromFile(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			return text;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000DE35C File Offset: 0x000DC55C
		public static void WriteToFile(string path, string data)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(data);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000DE384 File Offset: 0x000DC584
		public static float Abs(float value)
		{
			return (value >= 0f) ? value : (-value);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000DE39C File Offset: 0x000DC59C
		public static bool Approximately(float v1, float v2)
		{
			float num = v1 - v2;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000DE3C8 File Offset: 0x000DC5C8
		public static bool Approximately(Vector2 v1, Vector2 v2)
		{
			return Utility.Approximately(v1.x, v2.x) && Utility.Approximately(v1.y, v2.y);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000DE3F8 File Offset: 0x000DC5F8
		public static bool IsNotZero(float value)
		{
			return value < -1E-07f || value > 1E-07f;
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000DE410 File Offset: 0x000DC610
		public static bool IsZero(float value)
		{
			return value >= -1E-07f && value <= 1E-07f;
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000DE42C File Offset: 0x000DC62C
		public static bool AbsoluteIsOverThreshold(float value, float threshold)
		{
			return value < -threshold || value > threshold;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000DE440 File Offset: 0x000DC640
		public static float NormalizeAngle(float angle)
		{
			while (angle < 0f)
			{
				angle += 360f;
			}
			while (angle > 360f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000DE478 File Offset: 0x000DC678
		public static float VectorToAngle(Vector2 vector)
		{
			if (Utility.IsZero(vector.x) && Utility.IsZero(vector.y))
			{
				return 0f;
			}
			return Utility.NormalizeAngle(Mathf.Atan2(vector.x, vector.y) * 57.29578f);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000DE4CC File Offset: 0x000DC6CC
		public static float Min(float v0, float v1)
		{
			return (v0 < v1) ? v0 : v1;
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000DE4DC File Offset: 0x000DC6DC
		public static float Max(float v0, float v1)
		{
			return (v0 > v1) ? v0 : v1;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000DE4EC File Offset: 0x000DC6EC
		public static float Min(float v0, float v1, float v2, float v3)
		{
			float num = ((v0 < v1) ? v0 : v1);
			float num2 = ((v2 < v3) ? v2 : v3);
			return (num < num2) ? num : num2;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000DE528 File Offset: 0x000DC728
		public static float Max(float v0, float v1, float v2, float v3)
		{
			float num = ((v0 > v1) ? v0 : v1);
			float num2 = ((v2 > v3) ? v2 : v3);
			return (num > num2) ? num : num2;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000DE564 File Offset: 0x000DC764
		internal static float ValueFromSides(float negativeSide, float positiveSide)
		{
			float num = Utility.Abs(negativeSide);
			float num2 = Utility.Abs(positiveSide);
			if (Utility.Approximately(num, num2))
			{
				return 0f;
			}
			return (num <= num2) ? num2 : (-num);
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000DE5A0 File Offset: 0x000DC7A0
		internal static float ValueFromSides(float negativeSide, float positiveSide, bool invertSides)
		{
			if (invertSides)
			{
				return Utility.ValueFromSides(positiveSide, negativeSide);
			}
			return Utility.ValueFromSides(negativeSide, positiveSide);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000DE5B8 File Offset: 0x000DC7B8
		public static void ArrayResize<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				Array.Resize<T>(ref array, Utility.NextPowerOfTwo(capacity));
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000DE5D8 File Offset: 0x000DC7D8
		public static void ArrayExpand<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				array = new T[Utility.NextPowerOfTwo(capacity)];
			}
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
		public static int NextPowerOfTwo(int value)
		{
			if (value > 0)
			{
				value--;
				value |= value >> 1;
				value |= value >> 2;
				value |= value >> 4;
				value |= value >> 8;
				value |= value >> 16;
				value++;
				return value;
			}
			return 0;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x000DE634 File Offset: 0x000DC834
		internal static bool Is32Bit
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x000DE640 File Offset: 0x000DC840
		internal static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000DE64C File Offset: 0x000DC84C
		public static string HKLM_GetString(string path, string key)
		{
			string text;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path);
				if (registryKey == null)
				{
					text = string.Empty;
				}
				else
				{
					text = (string)registryKey.GetValue(key);
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000DE6A4 File Offset: 0x000DC8A4
		public static string GetWindowsVersion()
		{
			string text = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
			if (text != null)
			{
				string text2 = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
				string text3 = ((!Utility.Is32Bit) ? "64Bit" : "32Bit");
				int systemBuildNumber = Utility.GetSystemBuildNumber();
				return string.Concat(new object[]
				{
					text,
					(text2 == null) ? string.Empty : (" " + text2),
					" ",
					text3,
					" Build ",
					systemBuildNumber
				});
			}
			return SystemInfo.operatingSystem;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000DE748 File Offset: 0x000DC948
		public static int GetSystemBuildNumber()
		{
			return Environment.OSVersion.Version.Build;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000DE75C File Offset: 0x000DC95C
		internal static void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000DE764 File Offset: 0x000DC964
		internal static string PluginFileExtension()
		{
			return ".dll";
		}

		// Token: 0x04001DEC RID: 7660
		public const float Epsilon = 1E-07f;

		// Token: 0x04001DED RID: 7661
		private static Vector2[] circleVertexList = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0.2588f, 0.9659f),
			new Vector2(0.5f, 0.866f),
			new Vector2(0.7071f, 0.7071f),
			new Vector2(0.866f, 0.5f),
			new Vector2(0.9659f, 0.2588f),
			new Vector2(1f, 0f),
			new Vector2(0.9659f, -0.2588f),
			new Vector2(0.866f, -0.5f),
			new Vector2(0.7071f, -0.7071f),
			new Vector2(0.5f, -0.866f),
			new Vector2(0.2588f, -0.9659f),
			new Vector2(0f, -1f),
			new Vector2(-0.2588f, -0.9659f),
			new Vector2(-0.5f, -0.866f),
			new Vector2(-0.7071f, -0.7071f),
			new Vector2(-0.866f, -0.5f),
			new Vector2(-0.9659f, -0.2588f),
			new Vector2(-1f, -0f),
			new Vector2(-0.9659f, 0.2588f),
			new Vector2(-0.866f, 0.5f),
			new Vector2(-0.7071f, 0.7071f),
			new Vector2(-0.5f, 0.866f),
			new Vector2(-0.2588f, 0.9659f),
			new Vector2(0f, 1f)
		};
	}
}
