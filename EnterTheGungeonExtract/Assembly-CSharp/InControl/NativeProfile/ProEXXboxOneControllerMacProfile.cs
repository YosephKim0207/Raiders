using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070D RID: 1805
	public class ProEXXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002938 RID: 10552 RVA: 0x000AF6F4 File Offset: 0x000AD8F4
		public ProEXXboxOneControllerMacProfile()
		{
			base.Name = "Pro EX Xbox One Controller";
			base.Meta = "Pro EX Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21562)
				}
			};
		}
	}
}
