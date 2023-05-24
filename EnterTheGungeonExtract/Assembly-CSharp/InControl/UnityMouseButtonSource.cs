using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200077A RID: 1914
	public class UnityMouseButtonSource : InputControlSource
	{
		// Token: 0x06002AC6 RID: 10950 RVA: 0x000C2068 File Offset: 0x000C0268
		public UnityMouseButtonSource()
		{
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000C2070 File Offset: 0x000C0270
		public UnityMouseButtonSource(int buttonId)
		{
			this.ButtonId = buttonId;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000C2080 File Offset: 0x000C0280
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000C20A0 File Offset: 0x000C02A0
		public bool GetState(InputDevice inputDevice)
		{
			return Input.GetMouseButton(this.ButtonId);
		}

		// Token: 0x04001D8D RID: 7565
		public int ButtonId;
	}
}
