using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x020006B2 RID: 1714
	public abstract class InputDeviceManager
	{
		// Token: 0x0600283D RID: 10301
		public abstract void Update(ulong updateTick, float deltaTime);

		// Token: 0x0600283E RID: 10302 RVA: 0x000AB0E4 File Offset: 0x000A92E4
		public virtual void Destroy()
		{
		}

		// Token: 0x04001BFF RID: 7167
		protected List<InputDevice> devices = new List<InputDevice>();
	}
}
