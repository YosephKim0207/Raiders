using System;

namespace InControl
{
	// Token: 0x02000801 RID: 2049
	[AutoDiscover]
	public class XTR55_G2_MacUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002B57 RID: 11095 RVA: 0x000DBB00 File Offset: 0x000D9D00
		public XTR55_G2_MacUnityProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			this.JoystickNames = new string[] { "              SAILI Simulator --- XTR5.5+G2+FMS Controller" };
		}
	}
}
