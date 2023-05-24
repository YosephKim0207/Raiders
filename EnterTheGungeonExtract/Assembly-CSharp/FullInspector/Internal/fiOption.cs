using System;

namespace FullInspector.Internal
{
	// Token: 0x02000576 RID: 1398
	public static class fiOption
	{
		// Token: 0x06002100 RID: 8448 RVA: 0x000919A8 File Offset: 0x0008FBA8
		public static fiOption<T> Just<T>(T value)
		{
			return new fiOption<T>(value);
		}
	}
}
