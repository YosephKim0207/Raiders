using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000583 RID: 1411
	public static class fiUtility
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x00093320 File Offset: 0x00091520
		public static string CombinePaths(string a, string b)
		{
			return Path.Combine(a, b).Replace('\\', '/');
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00093334 File Offset: 0x00091534
		public static string CombinePaths(string a, string b, string c)
		{
			return Path.Combine(Path.Combine(a, b), c).Replace('\\', '/');
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0009334C File Offset: 0x0009154C
		public static string CombinePaths(string a, string b, string c, string d)
		{
			return Path.Combine(Path.Combine(Path.Combine(a, b), c), d).Replace('\\', '/');
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0009336C File Offset: 0x0009156C
		public static bool NearlyEqual(float a, float b)
		{
			return fiUtility.NearlyEqual(a, b, float.Epsilon);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0009337C File Offset: 0x0009157C
		public static bool NearlyEqual(float a, float b, float epsilon)
		{
			float num = Math.Abs(a);
			float num2 = Math.Abs(b);
			float num3 = Math.Abs(a - b);
			if (a == b)
			{
				return true;
			}
			if (a == 0f || b == 0f || num3 < -3.4028235E+38f)
			{
				return (double)num3 < (double)epsilon * double.MinValue;
			}
			return num3 / (num + num2) < epsilon;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000933E4 File Offset: 0x000915E4
		public static void DestroyObject(UnityEngine.Object obj)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj, true);
			}
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00093404 File Offset: 0x00091604
		public static void DestroyObject<T>(ref T obj) where T : UnityEngine.Object
		{
			fiUtility.DestroyObject(obj);
			obj = (T)((object)null);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00093424 File Offset: 0x00091624
		public static string StripLeadingWhitespace(this string s)
		{
			Regex regex = new Regex("^\\s+", RegexOptions.Multiline);
			return regex.Replace(s, string.Empty);
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x0009344C File Offset: 0x0009164C
		public static bool IsEditor
		{
			get
			{
				if (fiUtility._cachedIsEditor == null)
				{
					fiUtility._cachedIsEditor = new bool?(Type.GetType("UnityEditor.Editor, UnityEditor", false) != null);
				}
				return fiUtility._cachedIsEditor.Value;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00093484 File Offset: 0x00091684
		public static bool IsMainThread
		{
			get
			{
				if (!fiUtility.IsEditor)
				{
					throw new InvalidOperationException("Only available in the editor");
				}
				return Thread.CurrentThread.ManagedThreadId == 1;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000934A8 File Offset: 0x000916A8
		public static bool IsUnity4
		{
			get
			{
				if (fiUtility._isUnity4 == null)
				{
					fiUtility._isUnity4 = new bool?(Type.GetType("UnityEngine.RuntimeInitializeOnLoadMethodAttribute, UnityEngine", false) == null);
				}
				return fiUtility._isUnity4.Value;
			}
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000934DC File Offset: 0x000916DC
		public static Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(IList<TKey> keys, IList<TValue> values)
		{
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			if (keys != null && values != null)
			{
				for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
				{
					if (!object.ReferenceEquals(keys[i], null))
					{
						dictionary[keys[i]] = values[i];
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00093550 File Offset: 0x00091750
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0400181B RID: 6171
		private static bool? _cachedIsEditor;

		// Token: 0x0400181C RID: 6172
		private static bool? _isUnity4;
	}
}
