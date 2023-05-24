using System;

namespace InControl
{
	// Token: 0x02000730 RID: 1840
	[AutoDiscover]
	public class XTR_G2_MacNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x0600295B RID: 10587 RVA: 0x000B3204 File Offset: 0x000B1404
		public XTR_G2_MacNativeProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2971),
					ProductID = new ushort?(16402),
					NameLiterals = new string[] { "KMODEL Simulator - XTR+G2+FMS Controller" }
				}
			};
		}
	}
}
