using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000667 RID: 1639
	public interface tkIControl
	{
		// Token: 0x06002577 RID: 9591
		object Edit(Rect rect, object obj, object context, fiGraphMetadata metadata);

		// Token: 0x06002578 RID: 9592
		float GetHeight(object obj, object context, fiGraphMetadata metadata);

		// Token: 0x06002579 RID: 9593
		void InitializeId(ref int nextId);

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600257A RID: 9594
		Type ContextType { get; }
	}
}
