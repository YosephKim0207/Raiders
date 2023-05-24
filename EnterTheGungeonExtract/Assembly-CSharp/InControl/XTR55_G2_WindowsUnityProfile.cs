using System;

namespace InControl
{
	// Token: 0x02000802 RID: 2050
	[AutoDiscover]
	public class XTR55_G2_WindowsUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002B58 RID: 11096 RVA: 0x000DBB58 File Offset: 0x000D9D58
		public XTR55_G2_WindowsUnityProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			this.JoystickNames = new string[] { "SAILI Simulator --- XTR5.5+G2+FMS Controller" };
		}
	}
}
