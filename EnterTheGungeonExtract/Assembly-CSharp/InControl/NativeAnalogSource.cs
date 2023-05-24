using System;

namespace InControl
{
	// Token: 0x020006B8 RID: 1720
	public class NativeAnalogSource : InputControlSource
	{
		// Token: 0x060028DF RID: 10463 RVA: 0x000AD07C File Offset: 0x000AB27C
		public NativeAnalogSource(int analogIndex)
		{
			this.AnalogIndex = analogIndex;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000AD08C File Offset: 0x000AB28C
		public float GetValue(InputDevice inputDevice)
		{
			NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
			return nativeInputDevice.ReadRawAnalogValue(this.AnalogIndex);
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000AD0AC File Offset: 0x000AB2AC
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04001C66 RID: 7270
		public int AnalogIndex;
	}
}
