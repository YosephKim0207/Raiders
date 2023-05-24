using System;

namespace FullSerializer.Internal
{
	// Token: 0x020005B6 RID: 1462
	public static class fsOption
	{
		// Token: 0x060022BE RID: 8894 RVA: 0x00099294 File Offset: 0x00097494
		public static fsOption<T> Just<T>(T value)
		{
			return new fsOption<T>(value);
		}
	}
}
