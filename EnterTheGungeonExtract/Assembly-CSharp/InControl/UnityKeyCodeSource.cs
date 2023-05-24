using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000778 RID: 1912
	public class UnityKeyCodeSource : InputControlSource
	{
		// Token: 0x06002ABE RID: 10942 RVA: 0x000C1FB0 File Offset: 0x000C01B0
		public UnityKeyCodeSource()
		{
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000C1FB8 File Offset: 0x000C01B8
		public UnityKeyCodeSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000C1FC8 File Offset: 0x000C01C8
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000C1FE8 File Offset: 0x000C01E8
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (Input.GetKey(this.KeyCodeList[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001D8B RID: 7563
		public KeyCode[] KeyCodeList;
	}
}
