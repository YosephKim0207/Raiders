using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000712 RID: 1810
	public class RazerSabertoothEliteControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293D RID: 10557 RVA: 0x000AF958 File Offset: 0x000ADB58
		public RazerSabertoothEliteControllerMacProfile()
		{
			base.Name = "Razer Sabertooth Elite Controller";
			base.Meta = "Razer Sabertooth Elite Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(65024)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23812)
				}
			};
		}
	}
}
