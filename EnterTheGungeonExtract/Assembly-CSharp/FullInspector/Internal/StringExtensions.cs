using System;

namespace FullInspector.Internal
{
	// Token: 0x020005DD RID: 1501
	internal static class StringExtensions
	{
		// Token: 0x060023A3 RID: 9123 RVA: 0x0009C4E8 File Offset: 0x0009A6E8
		public static string F(this string format, params object[] args)
		{
			return string.Format(format, args);
		}
	}
}
