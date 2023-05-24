using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector.BackupService
{
	// Token: 0x020005F0 RID: 1520
	public class fiSerializationOperator : ISerializationOperator
	{
		// Token: 0x060023CA RID: 9162 RVA: 0x0009CBD0 File Offset: 0x0009ADD0
		public UnityEngine.Object RetrieveObjectReference(int storageId)
		{
			if (this.SerializedObjects == null)
			{
				throw new InvalidOperationException("SerializedObjects cannot be  null");
			}
			if (storageId < 0 || storageId >= this.SerializedObjects.Count)
			{
				return null;
			}
			fiUnityObjectReference fiUnityObjectReference = this.SerializedObjects[storageId];
			if (fiUnityObjectReference == null || fiUnityObjectReference.Target == null)
			{
				return null;
			}
			return fiUnityObjectReference.Target;
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0009CC3C File Offset: 0x0009AE3C
		public int StoreObjectReference(UnityEngine.Object obj)
		{
			if (this.SerializedObjects == null)
			{
				throw new InvalidOperationException("SerializedObjects cannot be null");
			}
			if (obj == null)
			{
				return -1;
			}
			int count = this.SerializedObjects.Count;
			this.SerializedObjects.Add(new fiUnityObjectReference(obj));
			return count;
		}

		// Token: 0x040018E4 RID: 6372
		public List<fiUnityObjectReference> SerializedObjects;
	}
}
