using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F7 RID: 1783
	public class MicrosoftXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002922 RID: 10530 RVA: 0x000AEBBC File Offset: 0x000ACDBC
		public MicrosoftXboxOneControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Controller";
			base.Meta = "Microsoft Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(733)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(746)
				}
			};
		}
	}
}
