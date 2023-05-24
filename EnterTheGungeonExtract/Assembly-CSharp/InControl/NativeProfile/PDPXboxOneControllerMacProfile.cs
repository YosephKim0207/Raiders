using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000705 RID: 1797
	public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x000AF2F8 File Offset: 0x000AD4F8
		public PDPXboxOneControllerMacProfile()
		{
			base.Name = "PDP Xbox One Controller";
			base.Meta = "PDP Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(314)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(354)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(353)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(355)
				}
			};
		}
	}
}
