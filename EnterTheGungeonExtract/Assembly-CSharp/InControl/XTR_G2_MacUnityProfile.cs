using System;

namespace InControl
{
	// Token: 0x020007FF RID: 2047
	[AutoDiscover]
	public class XTR_G2_MacUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002B55 RID: 11093 RVA: 0x000DBA50 File Offset: 0x000D9C50
		public XTR_G2_MacUnityProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			this.JoystickNames = new string[] { "FeiYing Model KMODEL Simulator - XTR+G2+FMS Controller" };
		}
	}
}
