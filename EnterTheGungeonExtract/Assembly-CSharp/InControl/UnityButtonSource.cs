using System;

namespace InControl
{
	// Token: 0x02000773 RID: 1907
	public class UnityButtonSource : InputControlSource
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x000C1D80 File Offset: 0x000BFF80
		public UnityButtonSource(int buttonIndex)
		{
			this.ButtonIndex = buttonIndex;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000C1D90 File Offset: 0x000BFF90
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000C1DB0 File Offset: 0x000BFFB0
		public bool GetState(InputDevice inputDevice)
		{
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			return unityInputDevice.ReadRawButtonState(this.ButtonIndex);
		}

		// Token: 0x04001D82 RID: 7554
		public int ButtonIndex;
	}
}
