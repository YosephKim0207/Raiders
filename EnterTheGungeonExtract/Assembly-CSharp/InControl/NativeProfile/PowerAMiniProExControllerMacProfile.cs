using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000709 RID: 1801
	public class PowerAMiniProExControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002934 RID: 10548 RVA: 0x000AF520 File Offset: 0x000AD720
		public PowerAMiniProExControllerMacProfile()
		{
			base.Name = "PowerA Mini Pro Ex Controller";
			base.Meta = "PowerA Mini Pro Ex Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16128)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21274)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21248)
				}
			};
		}
	}
}
