using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000779 RID: 1913
	public class UnityMouseAxisSource : InputControlSource
	{
		// Token: 0x06002AC2 RID: 10946 RVA: 0x000C2024 File Offset: 0x000C0224
		public UnityMouseAxisSource()
		{
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000C202C File Offset: 0x000C022C
		public UnityMouseAxisSource(string axis)
		{
			this.MouseAxisQuery = "mouse " + axis;
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000C2048 File Offset: 0x000C0248
		public float GetValue(InputDevice inputDevice)
		{
			return Input.GetAxisRaw(this.MouseAxisQuery);
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000C2058 File Offset: 0x000C0258
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04001D8C RID: 7564
		public string MouseAxisQuery;
	}
}
