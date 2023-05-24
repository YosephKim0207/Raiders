using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	// Token: 0x0200054D RID: 1357
	public interface ICullableDictionary<TKey, TValue>
	{
		// Token: 0x17000641 RID: 1601
		TValue this[TKey key] { get; set; }

		// Token: 0x0600203B RID: 8251
		bool TryGetValue(TKey key, out TValue value);

		// Token: 0x0600203C RID: 8252
		void BeginCullZone();

		// Token: 0x0600203D RID: 8253
		void EndCullZone();

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600203E RID: 8254
		IEnumerable<KeyValuePair<TKey, TValue>> Items { get; }

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600203F RID: 8255
		bool IsEmpty { get; }
	}
}
