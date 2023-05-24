using System;

namespace FullInspector.Internal
{
	// Token: 0x02000604 RID: 1540
	public interface fiIValueProxyAPI
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600240D RID: 9229
		// (set) Token: 0x0600240E RID: 9230
		object Value { get; set; }

		// Token: 0x0600240F RID: 9231
		void SaveState();

		// Token: 0x06002410 RID: 9232
		void LoadState();
	}
}
