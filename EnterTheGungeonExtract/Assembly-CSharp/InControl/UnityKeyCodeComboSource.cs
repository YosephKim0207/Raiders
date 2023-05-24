using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000777 RID: 1911
	public class UnityKeyCodeComboSource : InputControlSource
	{
		// Token: 0x06002ABA RID: 10938 RVA: 0x000C1F3C File Offset: 0x000C013C
		public UnityKeyCodeComboSource()
		{
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000C1F44 File Offset: 0x000C0144
		public UnityKeyCodeComboSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000C1F54 File Offset: 0x000C0154
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000C1F74 File Offset: 0x000C0174
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (!Input.GetKey(this.KeyCodeList[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001D8A RID: 7562
		public KeyCode[] KeyCodeList;
	}
}
