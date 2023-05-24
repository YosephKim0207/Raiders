using System;

namespace InControl
{
	// Token: 0x020006B9 RID: 1721
	public class NativeButtonSource : InputControlSource
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000AD0BC File Offset: 0x000AB2BC
		public NativeButtonSource(int buttonIndex)
		{
			this.ButtonIndex = buttonIndex;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000AD0CC File Offset: 0x000AB2CC
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000AD0EC File Offset: 0x000AB2EC
		public bool GetState(InputDevice inputDevice)
		{
			NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
			return nativeInputDevice.ReadRawButtonState(this.ButtonIndex);
		}

		// Token: 0x04001C67 RID: 7271
		public int ButtonIndex;
	}
}
