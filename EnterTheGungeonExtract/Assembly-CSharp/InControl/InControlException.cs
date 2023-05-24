using System;

namespace InControl
{
	// Token: 0x020006AB RID: 1707
	[Serializable]
	public class InControlException : Exception
	{
		// Token: 0x060027B3 RID: 10163 RVA: 0x000A947C File Offset: 0x000A767C
		public InControlException()
		{
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000A9484 File Offset: 0x000A7684
		public InControlException(string message)
			: base(message)
		{
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000A9490 File Offset: 0x000A7690
		public InControlException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
