using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005CB RID: 1483
	public class NotSupportedSerializationOperator : ISerializationOperator
	{
		// Token: 0x06002331 RID: 9009 RVA: 0x0009A960 File Offset: 0x00098B60
		public UnityEngine.Object RetrieveObjectReference(int storageId)
		{
			throw new NotSupportedException("UnityEngine.Object references are not supported with this serialization operator");
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0009A96C File Offset: 0x00098B6C
		public int StoreObjectReference(UnityEngine.Object obj)
		{
			throw new NotSupportedException("UnityEngine.Object references are not supported with this serialization operator (obj=" + obj + ")");
		}
	}
}
