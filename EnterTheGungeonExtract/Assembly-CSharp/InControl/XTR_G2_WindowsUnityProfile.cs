using System;

namespace InControl
{
	// Token: 0x02000800 RID: 2048
	[AutoDiscover]
	public class XTR_G2_WindowsUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002B56 RID: 11094 RVA: 0x000DBAA8 File Offset: 0x000D9CA8
		public XTR_G2_WindowsUnityProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			this.JoystickNames = new string[] { "KMODEL Simulator - XTR+G2+FMS Controller" };
		}
	}
}
