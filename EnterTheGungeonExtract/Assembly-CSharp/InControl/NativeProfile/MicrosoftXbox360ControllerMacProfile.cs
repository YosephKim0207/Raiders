using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F5 RID: 1781
	public class MicrosoftXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002920 RID: 10528 RVA: 0x000AE92C File Offset: 0x000ACB2C
		public MicrosoftXbox360ControllerMacProfile()
		{
			base.Name = "Microsoft Xbox 360 Controller";
			base.Meta = "Microsoft Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(654)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(655)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(307)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(63233)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(672)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(672)
				}
			};
		}
	}
}
