using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000776 RID: 1910
	public class UnityKeyCodeAxisSource : InputControlSource
	{
		// Token: 0x06002AB6 RID: 10934 RVA: 0x000C1ED0 File Offset: 0x000C00D0
		public UnityKeyCodeAxisSource()
		{
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000C1ED8 File Offset: 0x000C00D8
		public UnityKeyCodeAxisSource(KeyCode negativeKeyCode, KeyCode positiveKeyCode)
		{
			this.NegativeKeyCode = negativeKeyCode;
			this.PositiveKeyCode = positiveKeyCode;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000C1EF0 File Offset: 0x000C00F0
		public float GetValue(InputDevice inputDevice)
		{
			int num = 0;
			if (Input.GetKey(this.NegativeKeyCode))
			{
				num--;
			}
			if (Input.GetKey(this.PositiveKeyCode))
			{
				num++;
			}
			return (float)num;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000C1F2C File Offset: 0x000C012C
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04001D88 RID: 7560
		public KeyCode NegativeKeyCode;

		// Token: 0x04001D89 RID: 7561
		public KeyCode PositiveKeyCode;
	}
}
