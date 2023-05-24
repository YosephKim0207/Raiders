using System;

namespace FullInspector.Internal
{
	// Token: 0x020005CD RID: 1485
	public interface fiILoadedSerializers
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002339 RID: 9017
		Type DefaultSerializerProvider { get; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600233A RID: 9018
		Type[] AllLoadedSerializerProviders { get; }
	}
}
