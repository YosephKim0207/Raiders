using System;
using FullInspector.Serializers.FullSerializer;

namespace FullInspector.Internal
{
	// Token: 0x02000673 RID: 1651
	public class fiLoadedSerializers : fiILoadedSerializers
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000A1240 File Offset: 0x0009F440
		public Type DefaultSerializerProvider
		{
			get
			{
				return typeof(FullSerializerMetadata);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000A124C File Offset: 0x0009F44C
		public Type[] AllLoadedSerializerProviders
		{
			get
			{
				return new Type[] { typeof(FullSerializerMetadata) };
			}
		}
	}
}
