using System;

namespace InControl
{
	// Token: 0x0200068B RID: 1675
	public interface BindingSourceListener
	{
		// Token: 0x06002623 RID: 9763
		void Reset();

		// Token: 0x06002624 RID: 9764
		BindingSource Listen(BindingListenOptions listenOptions, InputDevice device);
	}
}
