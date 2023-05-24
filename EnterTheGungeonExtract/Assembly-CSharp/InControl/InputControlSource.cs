using System;

namespace InControl
{
	// Token: 0x020006A4 RID: 1700
	public interface InputControlSource
	{
		// Token: 0x0600274F RID: 10063
		float GetValue(InputDevice inputDevice);

		// Token: 0x06002750 RID: 10064
		bool GetState(InputDevice inputDevice);
	}
}
