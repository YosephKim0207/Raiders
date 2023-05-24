using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005C8 RID: 1480
	public interface ISerializationOperator
	{
		// Token: 0x06002321 RID: 8993
		UnityEngine.Object RetrieveObjectReference(int storageId);

		// Token: 0x06002322 RID: 8994
		int StoreObjectReference(UnityEngine.Object obj);
	}
}
