using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000722 RID: 1826
	public class XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x0600294D RID: 10573 RVA: 0x000B0078 File Offset: 0x000AE278
		public XboxOneControllerMacProfile()
		{
			base.Name = "Xbox One Controller";
			base.Meta = "Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21786)
				}
			};
		}
	}
}
