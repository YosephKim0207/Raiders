using System;

namespace InControl
{
	// Token: 0x02000772 RID: 1906
	public class UnityAnalogSource : InputControlSource
	{
		// Token: 0x06002AA8 RID: 10920 RVA: 0x000C1D40 File Offset: 0x000BFF40
		public UnityAnalogSource(int analogIndex)
		{
			this.AnalogIndex = analogIndex;
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000C1D50 File Offset: 0x000BFF50
		public float GetValue(InputDevice inputDevice)
		{
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			return unityInputDevice.ReadRawAnalogValue(this.AnalogIndex);
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000C1D70 File Offset: 0x000BFF70
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04001D81 RID: 7553
		public int AnalogIndex;
	}
}
