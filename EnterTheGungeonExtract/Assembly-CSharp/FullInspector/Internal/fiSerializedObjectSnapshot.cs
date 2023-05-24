using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200057D RID: 1405
	public class fiSerializedObjectSnapshot
	{
		// Token: 0x06002149 RID: 8521 RVA: 0x00092D5C File Offset: 0x00090F5C
		public fiSerializedObjectSnapshot(ISerializedObject obj)
		{
			this._keys = new List<string>(obj.SerializedStateKeys);
			this._values = new List<string>(obj.SerializedStateValues);
			this._objectReferences = new List<UnityEngine.Object>(obj.SerializedObjectReferences);
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00092D98 File Offset: 0x00090F98
		public void RestoreSnapshot(ISerializedObject target)
		{
			target.SerializedStateKeys = new List<string>(this._keys);
			target.SerializedStateValues = new List<string>(this._values);
			target.SerializedObjectReferences = new List<UnityEngine.Object>(this._objectReferences);
			target.RestoreState();
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x00092DD4 File Offset: 0x00090FD4
		public bool IsEmpty
		{
			get
			{
				return this._keys.Count == 0 || this._values.Count == 0;
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00092DF8 File Offset: 0x00090FF8
		public override bool Equals(object obj)
		{
			fiSerializedObjectSnapshot fiSerializedObjectSnapshot = obj as fiSerializedObjectSnapshot;
			return !object.ReferenceEquals(fiSerializedObjectSnapshot, null) && (fiSerializedObjectSnapshot.AreEqual<string>(this._keys, fiSerializedObjectSnapshot._keys) && fiSerializedObjectSnapshot.AreEqual<string>(this._values, fiSerializedObjectSnapshot._values)) && fiSerializedObjectSnapshot.AreEqual<UnityEngine.Object>(this._objectReferences, fiSerializedObjectSnapshot._objectReferences);
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x00092E5C File Offset: 0x0009105C
		public override int GetHashCode()
		{
			int num = 13;
			num = num * 7 + this._keys.GetHashCode();
			num = num * 7 + this._values.GetHashCode();
			return num * 7 + this._objectReferences.GetHashCode();
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00092EA0 File Offset: 0x000910A0
		public static bool operator ==(fiSerializedObjectSnapshot a, fiSerializedObjectSnapshot b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00092EAC File Offset: 0x000910AC
		public static bool operator !=(fiSerializedObjectSnapshot a, fiSerializedObjectSnapshot b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00092EB8 File Offset: 0x000910B8
		private static bool AreEqual<T>(List<T> a, List<T> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040017FE RID: 6142
		private readonly List<string> _keys;

		// Token: 0x040017FF RID: 6143
		private readonly List<string> _values;

		// Token: 0x04001800 RID: 6144
		private readonly List<UnityEngine.Object> _objectReferences;
	}
}
