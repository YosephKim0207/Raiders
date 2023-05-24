using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x020005CA RID: 1482
	public class ListSerializationOperator : ISerializationOperator
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x0009A8CC File Offset: 0x00098ACC
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
			return this.SerializedObjects[storageId];
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0009A90C File Offset: 0x00098B0C
		public int StoreObjectReference(UnityEngine.Object obj)
		{
			if (this.SerializedObjects == null)
			{
				throw new InvalidOperationException("SerializedObjects cannot be null");
			}
			if (object.ReferenceEquals(obj, null))
			{
				return -1;
			}
			int count = this.SerializedObjects.Count;
			this.SerializedObjects.Add(obj);
			return count;
		}

		// Token: 0x04001894 RID: 6292
		public List<UnityEngine.Object> SerializedObjects;
	}
}
