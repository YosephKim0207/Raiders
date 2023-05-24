using System;

namespace InControl
{
	// Token: 0x020006A1 RID: 1697
	public interface IInputControl
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002737 RID: 10039
		bool HasChanged { get; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002738 RID: 10040
		bool IsPressed { get; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002739 RID: 10041
		bool WasPressed { get; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600273A RID: 10042
		bool WasReleased { get; }

		// Token: 0x0600273B RID: 10043
		void ClearInputState();
	}
}
