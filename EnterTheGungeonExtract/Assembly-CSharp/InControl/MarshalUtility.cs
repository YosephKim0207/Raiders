using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x0200080C RID: 2060
	public static class MarshalUtility
	{
		// Token: 0x06002BAE RID: 11182 RVA: 0x000DD718 File Offset: 0x000DB918
		public static void Copy(IntPtr source, uint[] destination, int length)
		{
			Utility.ArrayExpand<int>(ref MarshalUtility.buffer, length);
			Marshal.Copy(source, MarshalUtility.buffer, 0, length);
			Buffer.BlockCopy(MarshalUtility.buffer, 0, destination, 0, 4 * length);
		}

		// Token: 0x04001DE0 RID: 7648
		private static int[] buffer = new int[32];
	}
}
